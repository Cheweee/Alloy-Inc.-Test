using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestApp.ConsoleClientApp.Services;
using TestApp.ConsoleClientApp.WebClients;
using TestApp.Data.Models;

namespace TestApp.ConsoleClientApp
{
    [Verb("get", HelpText = "Provide options for get users locations")]
    class GetOptions
    {
        [Option('s', "search")]
        public string Search { get; set; }
    }

    [Verb("create", HelpText = "Provide options for create user location")]
    class CreateOptions { }

    [Verb("delete", HelpText = "Provide options for delete users locations")]
    class DeleteOptions
    {
        [Option("ids", Separator = ',')]
        public IEnumerable<int> Ids { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            Configure(serviceCollection);

            var services = serviceCollection.BuildServiceProvider();
            var apiClient = services.GetService<ApiClient>();
            var logger = services.GetService<ILogger<UserLocationService>>();
            var locationService = new UserLocationService(apiClient, logger);

            return CommandLine.Parser.Default.ParseArguments<GetOptions, CreateOptions, DeleteOptions>(args)
            .MapResult(
                (GetOptions options) => RunGet(options, locationService, logger).GetAwaiter().GetResult(),
                (CreateOptions options) => RunCreate(options, locationService, logger).GetAwaiter().GetResult(),
                (DeleteOptions options) => RunDelete(options, locationService, logger).GetAwaiter().GetResult(),
                errors => 1
            );
        }

        static async Task<int> RunGet(GetOptions options, UserLocationService locationService, ILogger logger)
        {
            try
            {
                logger.LogInformation("Try to load users locations.");

                var locations = await locationService.Get(new UserLocationGetOptions { Search = options.Search });

                foreach (var location in locations)
                {
                    logger.LogInformation($"#{location.Id}. IP address: {location.IPAddress} Location: {location.Location};");
                }

                logger.LogInformation("Users locations successfully loaded.");

                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }

        static async Task<int> RunCreate(CreateOptions options, UserLocationService locationService, ILogger logger)
        {
            try
            {
                logger.LogInformation("Try to create user location.");

                string strHostName = string.Empty;
                strHostName = Dns.GetHostName();
                logger.LogInformation($"Local Machine's Host Name: {strHostName}.");
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;

                List<UserLocation> locations = new List<UserLocation>();
                foreach (IPAddress address in addr)
                {
                    var newAddress = address.MapToIPv4();
                    locations.Add(new UserLocation { IPAddress = newAddress.ToString() });
                }

                await locationService.Create(locations);

                logger.LogInformation("User location successfully created.");

                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }

        static async Task<int> RunDelete(DeleteOptions options, UserLocationService locationService, ILogger logger)
        {
            try
            {
                logger.LogInformation("Try to delete users locations.");
                await locationService.Delete(options.Ids.ToList());
                logger.LogInformation("Try to delete users locations.");
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }

        static void Configure(IServiceCollection services)
        {
            services.AddLogging(logging => { logging.AddConsole(); })
            .AddHttpClient("api", c =>
            {
                c.BaseAddress = new Uri("http://localhost:5000/");
            })
            .AddTypedClient<ApiClient>();
        }
    }
}
