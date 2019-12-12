using System;
using TestApp.Shared;
using TestApp.Utilities.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TestApp.Shared.Enumerations;
using Npgsql;
using System.Data.SqlClient;

namespace TestApp.Utilities.Actions
{
    public class MigrateUtilities
    {
        public static IDbCommand CreateCommand(DatabaseConnectionSettings settings, string command, IDbConnection connection)
        {
            switch (settings.Provider)
            {
                case DatabaseProvider.Postgres: return new NpgsqlCommand(command, connection as NpgsqlConnection);
                case DatabaseProvider.SqlServer: return new SqlCommand(command, connection as SqlConnection);
                default: return new SqlCommand(command, connection as SqlConnection);
            }
        }

        public static IDbConnection CreateServerConnection(DatabaseConnectionSettings settings)
        {
            switch (settings.Provider)
            {
                default:
                case DatabaseProvider.SqlServer: return new SqlConnection(settings.SqlServerServerConnectionString);
                case DatabaseProvider.Postgres: return new NpgsqlConnection(settings.PostgresServerConnectionString);
            }
        }

        public static IDbConnection CreateDatabaseConnection(DatabaseConnectionSettings settings)
        {
            switch (settings.Provider)
            {
                default:
                case DatabaseProvider.SqlServer: return new SqlConnection(settings.SqlServerServerDatabaseConnectionString);
                case DatabaseProvider.Postgres: return new NpgsqlConnection(settings.PostgresDatabaseConnectionString);
            }
        }

        public static IServiceProvider CreateServices(DatabaseConnectionSettings settings)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    switch (settings.Provider)
                    {
                        default:
                        case DatabaseProvider.SqlServer:
                            // Add SqlServer support to FluentMigrator
                            rb.AddSqlServer()
                            .WithGlobalConnectionString(settings.SqlServerServerDatabaseConnectionString);
                            break;
                        case DatabaseProvider.Postgres:
                            // Add Postgres support to FluentMigrator
                            rb.AddPostgres()
                            .WithGlobalConnectionString(settings.PostgresDatabaseConnectionString);
                            break;

                    }
                    // Define the assembly containing the migrations                    
                    rb.ScanIn(typeof(CreateProduct).Assembly).For.Migrations();
                })
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
    }
}