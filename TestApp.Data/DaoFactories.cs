using TestApp.Data.Interfaces;
using TestApp.Data.Enumerations;
using Microsoft.Extensions.Logging;

namespace TestApp.Data
{
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DataProvider provider, string connectionString, ILogger logger)
        {
            switch (provider)
            {
                case DataProvider.SqlServer:
                    return new DataAccessObjects.SqlServer.DaoFactory(connectionString, logger);
                default:
                    return new DataAccessObjects.SqlServer.DaoFactory(connectionString, logger);
            }
        }
    }
}