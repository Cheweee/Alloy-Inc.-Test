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
    public class ProductService
    {
        private readonly IProductDao _dao;
        private readonly ILogger _logger;

        public ProductService(IProductDao dao, ILogger logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<Product>> Get(ProductGetOptions options) => await _dao.Get(options);
        public async Task<Product> Create(Product model)
        {
            await _dao.Create(model);
            return model;
        }
        public async Task<Product> Update(Product model)
        {
            await _dao.Update(model);
            return model;
        }
        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        public async Task<string> Validate(Product model)
        {
            try
            {
                _logger.LogInformation("Start product validating.");
                StringBuilder builder = new StringBuilder();

                if (!ValidationUtilities.NotEmptyRule(model.Name))
                    builder.AppendLine("Product name should not be empty.");
                if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(model.Name))
                    builder.AppendLine("Product name should contains only letters, dots, commas and number.");
                if (!ValidationUtilities.MoreThanValueLengthRule(model.Name, 5))
                    builder.AppendLine("Product name should be longer than 5 symbols.");
                if (!ValidationUtilities.MoreThanValueRule(model.Price, 0))
                    builder.AppendLine("Product price should be greater than zero.");


                var models = await _dao.Get(new ProductGetOptions { Name = model.Name });
                if (models.Count() > 0)
                {
                    builder.AppendLine("Product with same name have been already created. Please try another name.");
                }

                string message = builder.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }

                _logger.LogInformation("Product successfuly validated.");
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