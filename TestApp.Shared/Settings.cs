using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using TestApp.Shared.Enumerations;

namespace TestApp.Shared
{
    public class Appsettings
    {
        public DatabaseConnectionSettings ConnectionSettings { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class DatabaseConnectionSettings
    {
        #region Private variables
        private string databaseHost;
        private string databaseName;
        private string databasePort;
        private string databaseUserName;
        private string databasePassword;
        private DatabaseProvider provider;
        #endregion

        #region Environment variables names
        public const string DatabaseHostVariableName = "database-host";

        public const string DatabaseNameVariableName = "database-name";

        public const string DatabasePortVariableName = "database-port";

        public const string DatabaseUserNameVariableName = "database-user-name";

        public const string DatabasePasswordVariableName = "database-password";
        #endregion

        public DatabaseProvider Provider { get => provider; set => provider = value; }

        public string PostgresServerConnectionString { get => $"host={databaseHost};port={databasePort};username={databaseUserName};password={databasePassword}"; }

        public string SqlServerServerConnectionString { get => $"data source={databaseHost};user id={databaseUserName};password={databasePassword};"; }

        public string SqlServerServerDatabaseConnectionString { get => $"{SqlServerServerConnectionString};initial catalog={databaseName};"; }

        public string PostgresDatabaseConnectionString { get => $"{PostgresServerConnectionString};database={databaseName};"; }

        public string DatabaseHost { get => databaseHost; set => databaseHost = value; }
        public string DatabaseName { get => databaseName; set => databaseName = value; }
        public string DatabasePort { get => databasePort; set => databasePort = value; }
        public string DatabaseUserName { get => databaseUserName; set => databaseUserName = value; }
        public string DatabasePassword { get => databasePassword; set => databasePassword = value; }

        public static DatabaseConnectionSettings InitializeSolutionSettings(
            string databaseHost,
            string databaseName,
            string databasePort,
            string databaseUserName,
            string databasePassword,
            DatabaseProvider provider)
        {
            return new DatabaseConnectionSettings
            {
                databaseHost = databaseHost,
                databaseName = databaseName,
                databasePassword = databasePassword,
                databasePort = databasePort,
                databaseUserName = databaseUserName,
                provider = provider
            };
        }
    }
}