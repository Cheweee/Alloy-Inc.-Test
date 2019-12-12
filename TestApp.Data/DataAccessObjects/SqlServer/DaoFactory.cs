using Microsoft.Extensions.Logging;
using TestApp.Data.DataAccessObjects.SqlServer;
using TestApp.Shared.Enumerations;
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

        public ICartDao CartDao => new CartDao(_connectionString, _logger, DatabaseProvider.SqlServer);

        public ICartReportDao CartReportDao => new CartReportDao(_connectionString, _logger, DatabaseProvider.SqlServer);
        
        public IDeliveryDao DeliveryDao => new DeliveryDao(_connectionString, _logger, DatabaseProvider.SqlServer);

        public IOrderDao OrderDao => new OrderDao(_connectionString, _logger, DatabaseProvider.SqlServer);

        public IProductDao ProductDao => new ProductDao(_connectionString, _logger, DatabaseProvider.SqlServer);

        public IUserLocationDao UserLocationDao => new UserLocationDao(_connectionString, _logger, DatabaseProvider.SqlServer);
    }
}