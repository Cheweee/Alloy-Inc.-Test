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
    public class OrderService
    {
        private readonly IOrderDao _dao;
        private readonly CartService _cartService;
        private readonly ILogger _logger;

        public OrderService(IOrderDao dao, CartService cartService, ILogger logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _cartService = cartService ?? throw new ArgumentException(nameof(cartService));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<Order>> Get(OrderGetOptions options) => await _dao.Get(options);
        public async Task<Order> Create(Order model)
        {
            await _dao.Create(model);
            foreach (Cart product in model.Products)
            {
                product.OrderId = model.Id;
                await _cartService.PlaceToOrder(product);
            }
            return model;
        }
        public async Task<Order> Update(Order model)
        {
            await _dao.Update(model);
            return model;
        }
        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        public string Validate(Order model)
        {
            try
            {
                _logger.LogInformation("Start order validating.");
                StringBuilder builder = new StringBuilder();

                if (!ValidationUtilities.NotEmptyRule(model.Addres))
                    builder.AppendLine("Your address should not be empty.");
                if (!ValidationUtilities.NotEmptyRule(model.FullName))
                    builder.AppendLine("Your full name should not be empty.");
                if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(model.FullName))
                    builder.AppendLine("Your full name should contains only letters, dots, commas and number.");
                if (!ValidationUtilities.NotNullRule(model.DeliveryId))
                    builder.AppendLine("You must choose a delivery service.");

                string message = builder.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }

                _logger.LogInformation("Order successfuly validated.");
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