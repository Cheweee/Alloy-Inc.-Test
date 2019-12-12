using TestApp.Data.Interfaces;
using TestApp.Shared;
using Microsoft.Extensions.Logging;
using TestApp.Shared.Enumerations;

namespace TestApp.Data
{
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DatabaseConnectionSettings settings, ILogger logger)
        {
            switch (settings.Provider)
            {
                case DatabaseProvider.SqlServer:
                    return new DataAccessObjects.SqlServer.DaoFactory(settings.SqlServerServerDatabaseConnectionString, logger);
                case DatabaseProvider.Postgres:
                    return new DataAccessObjects.Postgres.DaoFactory(settings.PostgresDatabaseConnectionString, logger);
                default:
                    return new DataAccessObjects.SqlServer.DaoFactory(settings.SqlServerServerDatabaseConnectionString, logger);
            }
        }
    }
}