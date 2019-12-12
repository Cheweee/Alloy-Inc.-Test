using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestApp.ConsoleClientApp.WebClients;
using TestApp.Data.Models;

namespace TestApp.ConsoleClientApp.Services
{
    public class UserLocationService
    {
        private readonly string apiUrl = "api/userlocation";
        private readonly ApiClient _apiClient;
        private readonly ILogger _logger;

        public UserLocationService(ApiClient apiClient, ILogger logger)
        {
            _apiClient = apiClient ?? throw new ArgumentException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<UserLocation>> Get(UserLocationGetOptions options)
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder(apiUrl);
                int conditionIndex = 0;
                if (!string.IsNullOrEmpty(options.Search))
                {
                    queryBuilder.Append($"{(conditionIndex++ == 0 ? "?" : "&")}search={options.Search}");
                }

                return await _apiClient.Get<UserLocation, UserLocationGetOptions>(options, queryBuilder.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Create(List<UserLocation> models)
        {
            try
            {
                await _apiClient.Post(models, apiUrl);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(List<UserLocation> models)
        {
            try
            {
                await _apiClient.Patch(models, apiUrl);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Delete(IReadOnlyList<int> ids)
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder(apiUrl);
                int conditionIndex = 0;
                foreach (int id in ids)
                {
                    queryBuilder.Append($"{(conditionIndex++ == 0 ? "?" : "&")}ids={id}");
                }

                await _apiClient.Delete(queryBuilder.ToString());
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}