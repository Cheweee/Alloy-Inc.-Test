using System;
using TestApp.Shared;
using CommandLine;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;
using TestApp.Shared.Enumerations;

namespace TestApp.Utilities.Actions
{
    [Verb("app-settings", HelpText = "Set application settings by it names")]
    public class SolutionSettingsOptions
    {
        [Option(DatabaseConnectionSettings.DatabaseHostVariableName, HelpText = "Allow to set database host")]
        public string DatabaseHost { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseNameVariableName, HelpText = "Allow to set database name")]
        public string DatabaseName { get; set; }

        [Option(DatabaseConnectionSettings.DatabasePortVariableName, HelpText = "Allow to set database port", Required = false)]
        public string DatabasePort { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseUserNameVariableName, HelpText = "Allow to set database user name")]
        public string DatabaseUserName { get; set; }

        [Option(DatabaseConnectionSettings.DatabasePasswordVariableName, HelpText = "Allow to set database password")]
        public string DatabasePassword { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseProviderVariableName, HelpText = "Allow to set database provider")]
        public DatabaseProvider DatabaseProvider { get; set; }

        public void InitialiazeSettings()
        {
            DatabaseHost = @"localhost";
            DatabaseName = "testapp";
            DatabasePort = "5432";
            DatabaseUserName = "postgres";
            DatabasePassword = "admin";
            DatabaseProvider = DatabaseProvider.Postgres;
        }
    }

    public class SettingsUpdate
    {
        public static int Run(
            ILogger logger,
            SolutionSettingsOptions options
        )
        {
            try
            {
                if (string.IsNullOrEmpty(options.DatabaseHost)
                && string.IsNullOrEmpty(options.DatabaseName)
                && string.IsNullOrEmpty(options.DatabaseUserName)
                && string.IsNullOrEmpty(options.DatabasePassword))
                {
                    options.InitialiazeSettings();
                }
                logger.LogInformation("Try to update solution settings");

                Appsettings appsettings = JsonConvert.DeserializeObject<Appsettings>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")));
                appsettings.ConnectionSettings = DatabaseConnectionSettings.InitializeSolutionSettings(
                    options.DatabaseHost,
                    options.DatabaseName,
                    options.DatabasePort,
                    options.DatabaseUserName,
                    options.DatabasePassword,
                    options.DatabaseProvider
                );

                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), JsonConvert.SerializeObject(appsettings));

                logger.LogInformation("Solution settings updated successfully");
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }
            return 0;
        }
    }
}