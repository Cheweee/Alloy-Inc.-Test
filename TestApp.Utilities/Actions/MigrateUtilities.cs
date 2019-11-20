using System;
using TestApp.Shared;
using TestApp.Utilities.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace TestApp.Utilities.Actions
{
    public class MigrateUtilities
    {
        public static IServiceProvider CreateServices(DatabaseConnectionSettings settings)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add Postgres support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString(settings.SqlServerServerDatabaseConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateProduct).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
    }
}