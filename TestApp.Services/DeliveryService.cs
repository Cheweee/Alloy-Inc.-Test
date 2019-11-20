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
    public class DeliveryService
    {
        private readonly IDeliveryDao _dao;
        private readonly ILogger _logger;

        public DeliveryService(IDeliveryDao dao, ILogger logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<Delivery>> Get(DeliveryGetOptions options) => await _dao.Get(options);
        public async Task<Delivery> Create(Delivery model)
        {
            await _dao.Create(model);
            return model;
        }
        public async Task<Delivery> Update(Delivery model)
        {
            await _dao.Update(model);
            return model;
        }
        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        public async Task<string> Validate(Delivery model)
        {
            try
            {
                _logger.LogInformation("Start delivery validating.");
                StringBuilder builder = new StringBuilder();

                if (!ValidationUtilities.NotEmptyRule(model.Name))
                    builder.AppendLine("Delivery name should not be empty.");
                if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(model.Name))
                    builder.AppendLine("Delivery name should contains only letters, dots, commas and number.");
                if (!ValidationUtilities.MoreThanValueLengthRule(model.Name, 5))
                    builder.AppendLine("Delivery name should be longer than 5 symbols.");
                if (!ValidationUtilities.MoreThanValueRule(model.DeliveryPrice, 0))
                    builder.AppendLine("Delivery price should be greater than zero.");

                var models = await _dao.Get(new DeliveryGetOptions
                {
                    Name = model.Name,
                    ExcludeIds = new List<int> { model.Id }
                });
                if (models.Count() > 0)
                {
                    builder.AppendLine("Delivery with same name have been already created. Please try another name.");
                }

                string message = builder.ToString();
                if (!string.IsNullOrEmpty(message))
                {
                    return message;
                }

                _logger.LogInformation("Delivery successfuly validated.");
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