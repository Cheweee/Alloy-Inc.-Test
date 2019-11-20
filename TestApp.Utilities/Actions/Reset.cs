using System;
using TestApp.Shared;
using CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace TestApp.Utilities.Actions
{
    [Verb("reset", HelpText = "Reset the DB (drop, create, migrate, seed)")]
    public class ResetOptions : SolutionSettingsOptions { }

    public class Reset
    {
        public static int Run(ILogger logger, ResetOptions options)
        {
            try
            {
                Appsettings appsettings = JsonConvert.DeserializeObject<Appsettings>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")));
                bool databaseInitialized = appsettings.ConnectionSettings != null;
                if (databaseInitialized)
                {
                    logger.LogInformation($"Try to reset \"{appsettings.ConnectionSettings.DatabaseName}\" database");
                }
                else
                {
                    logger.LogInformation($"Try to initialize project database");
                }

                SettingsUpdate.Run(logger, options);

                appsettings = JsonConvert.DeserializeObject<Appsettings>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")));

                if (databaseInitialized)
                    if (Drop.Run(logger, appsettings.ConnectionSettings) < 0) throw new Exception("There was some errors with dropping database");
                if (Create.Run(logger, appsettings.ConnectionSettings) < 0) throw new Exception("There was some errors with creating database");
                if (MigrateUp.Run(logger, appsettings.ConnectionSettings) < 0) throw new Exception("There was some errors with migrating database");

                logger.LogInformation($"{appsettings.ConnectionSettings.DatabaseName} database successfully reseted");
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }
    }
}