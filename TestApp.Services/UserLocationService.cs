using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;
using MaxMind.GeoIP2;
using System.IO;
using MaxMind.GeoIP2.Model;
using System.Net;
using MaxMind.GeoIP2.Responses;
using System.Linq;

namespace TestApp.Services
{
    public class UserLocationService
    {
        private readonly IUserLocationDao _dao;
        private readonly ILogger<UserLocationService> _logger;

        public UserLocationService(IUserLocationDao dao, ILogger<UserLocationService> logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<IEnumerable<UserLocation>> Get(UserLocationGetOptions options) => await _dao.Get(options);

        public async Task<IEnumerable<UserLocation>> Create(IEnumerable<UserLocation> models)
        {
            foreach (var model in models)
                model.Location = GetLocationFromIP(model.IPAddress);
            await _dao.Create(models.Where(o => !string.IsNullOrEmpty(o.Location)));
            return models;
        }

        public async Task<IEnumerable<UserLocation>> Update(IEnumerable<UserLocation> models)
        {
            foreach (var model in models)
                model.Location = GetLocationFromIP(model.IPAddress);
            await _dao.Update(models.Where(o => !string.IsNullOrEmpty(o.Location)));
            return models;
        }

        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        private string GetLocationFromIP(string address)
        {
            try
            {
                using (var reader = new DatabaseReader(Path.Combine(Environment.CurrentDirectory, "GeoIP", "GeoLite2-City.mmdb")))
                {  
                    IPAddress ipAddress = IPAddress.Parse(address);
                    CityResponse response = null;
                    if (reader.TryCity(ipAddress, out response))
                    {
                        string country = response.Country.Name;
                        string city = response.City.Name;
                        string postalCode = response.Postal.Code;
                        return $"{country} {city}({postalCode})";
                    }

                    return string.Empty;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}