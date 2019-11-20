using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;
using TestApp.Utilities;

namespace TestApp.Services
{
    public class CartService
    {
        private readonly ICartDao _dao;
        private readonly ProductService _productService;
        private readonly ILogger _logger;

        public CartService(ICartDao dao, ProductService productService, ILogger logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _productService = productService ?? throw new ArgumentException(nameof(productService));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<Cart>> Get(CartGetOptions options) => await _dao.Get(options);

        public async Task<Cart> AddToCart(Cart model)
        {
            var carts = await _dao.Get(new CartGetOptions { ProductId = model.ProductId, Ordered = model.OrderId.HasValue });
            var oldCart = carts.FirstOrDefault();
            if (oldCart != null)
            {
                oldCart.Count += model.Count;
                await _dao.Update(oldCart);
            }
            else
            {
                await _dao.Create(model);
            }
            if (!model.OrderId.HasValue)
            {
                var products = await _productService.Get(new ProductGetOptions { Id = model.ProductId });
                if (products.Count() > 0)
                {
                    var product = products.FirstOrDefault();
                    product.Count -= model.Count;
                    await _productService.Update(product);
                }
            }
            return model;
        }

        public async Task PlaceToOrder(Cart model)
        {
            await _dao.Update(model);
        }

        public async Task<Cart> Update(Cart model)
        {
            var carts = await _dao.Get(new CartGetOptions { ProductId = model.ProductId, Ordered = model.OrderId.HasValue });
            var oldCart = carts.FirstOrDefault();
            int countDelta = 0;
            if (oldCart != null)
            {
                countDelta = oldCart.Count - model.Count;
                if (model.Count == 0)
                {
                    await _dao.Delete(new List<int> { model.Id });
                }
                else
                {
                    await _dao.Update(model);
                }
            }
            else
            {
                countDelta -= model.Count;
                await _dao.Create(model);
            }
            if (!model.OrderId.HasValue)
            {
                var products = await _productService.Get(new ProductGetOptions { Id = model.ProductId });
                if (products.Count() > 0)
                {
                    var product = products.FirstOrDefault();
                    product.Count += countDelta;
                    await _productService.Update(product);
                }
            }
            return model;
        }

        public async Task Delete(IReadOnlyList<int> ids)
        {
            var productsInCart = await _dao.Get(new CartGetOptions { Ids = ids });
            var productsIds = productsInCart.Where(o => o.ProductId != null).Select(o => o.ProductId.Value);
            var products = await _productService.Get(new ProductGetOptions { Ids = productsIds.ToList() });
            foreach (var productInCart in productsInCart)
            {
                var product = products.FirstOrDefault(o => o.Id == productInCart.ProductId);
                product.Count += productInCart.Count;
                await _productService.Update(product);
            }

            await _dao.Delete(ids);
        }

        public string Validate(Cart model)
        {
            try
            {
                _logger.LogInformation("Start cart validating.");
                StringBuilder builder = new StringBuilder();

                if (!ValidationUtilities.MoreThanValueRule(model.Count))
                    builder.AppendLine("You can't add zero products to cart.");

                string message = builder.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }

                _logger.LogInformation("Cart successfuly validated.");
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}