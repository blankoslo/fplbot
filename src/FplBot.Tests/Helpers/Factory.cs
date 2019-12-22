using System.Linq;
using Fpl.Client;
using Fpl.Client.Abstractions;
using Fpl.Client.Clients;
using Fpl.Client.Infra;
using FplBot.ConsoleApps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Workers.Connections;
using Slackbot.Net.Workers.Handlers;
using Slackbot.Net.Workers.Publishers;
using Xunit.Abstractions;

namespace FplBot.Tests.Helpers
{
    public static class Factory
    {
        public static T Create<T>(ITestOutputHelper logger = null)
        {
            return BuildServiceProvider(logger).GetService<T>();
        }

        public static IHandleMessages GetHandler<T>(ITestOutputHelper logger)
        {
            var allHandlers = BuildServiceProvider(logger).GetServices<IHandleMessages>();
            return allHandlers.First(h => h is T);
        }

        private static ServiceProvider BuildServiceProvider(ITestOutputHelper logger)
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.Local.json", optional: true);
            config.AddEnvironmentVariables();
            var configuration = config.Build();

            var services = new ServiceCollection();
            services.AddFplApiClient(configuration.GetSection("fpl"));
            services.AddFplBot(configuration);
            
            services.ReplacePublishersWithDebugPublisher(logger);
            services.Replace<BotDetails>(new BotDetails { Id = "UREFQD887", Name = "fplbot"});

            services.AddSingleton<ILogger<CookieFetcher>, XUnitTestOutputLogger<CookieFetcher>>(s => new XUnitTestOutputLogger<CookieFetcher>(logger));
            var provider = services.BuildServiceProvider();
            return provider;
        }
        
        // remove live slack integration and replace with debugging publishers
        private static void ReplacePublishersWithDebugPublisher(this ServiceCollection services, ITestOutputHelper logger)
        {
            var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == typeof(IPublisher)).ToList();
            foreach (var service in serviceDescriptors)
            {
                var t = services.Remove(service);
            }

            services.AddSingleton<IPublisher, XUnitTestoutPublisher>(s => new XUnitTestoutPublisher(logger));
        }
        
        private static void Replace<T>(this ServiceCollection services, T replacement) where T : class
        {
            var serviceDescriptors = services.Where(descriptor => descriptor.ServiceType == typeof(T)).ToList();
            foreach (var service in serviceDescriptors)
            {
                var t = services.Remove(service);
            }

            services.AddSingleton<T>(s => replacement);
        }
    }
}