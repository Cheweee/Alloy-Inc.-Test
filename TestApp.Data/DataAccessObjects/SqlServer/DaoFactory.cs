using Microsoft.Extensions.Logging;
using TestApp.Data.DataAccessObjects.SqlServer;
using TestApp.Data.Interfaces;

namespace TestApp.Data.DataAccessObjects.SqlServer
{
    public class DaoFactory : IDaoFactory
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DaoFactory(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public ICartDao CartDao => new CartDao(_connectionString, _logger);

        public ICartReportDao CartReportDao => new CartReportDao(_connectionString, _logger);
        
        public IDeliveryDao DeliveryDao => new DeliveryDao(_connectionString, _logger);

        public IOrderDao OrderDao => new OrderDao(_connectionString, _logger);

        public IProductDao ProductDao => new ProductDao(_connectionString, _logger);
    }
}