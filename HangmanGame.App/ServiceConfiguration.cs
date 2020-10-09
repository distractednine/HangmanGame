using System;
using System.Linq;
using HangmanGame.App.Options;
using HangmanGame.App.Services;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common.Console;
using HangmanGame.Common.Console.Interfaces;
using HangmanGame.Common.Delegates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace HangmanGame.App
{
    internal class ServiceConfiguration
    {
        public IServiceProvider BuildServiceProvider(IConfiguration configuration)
        {
            var serviceCollection = GetServiceCollection(configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private IServiceCollection GetServiceCollection(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection()
                // standard extension methods
                .AddHttpClient()
                .AddLogging(builder =>
                {
                    builder.AddProvider(GetLoggerProvider());
                    builder.SetMinimumLevel(LogLevel.Debug);

                    // remove default logging output for HttpClientFactory
                    builder.AddFilter("Microsoft.Extensions.Http.DefaultHttpClientFactory", LogLevel.Warning);
                    builder.AddFilter("System.Net.Http.HttpClient.Default", LogLevel.Warning);
                })

                // services registration
                .AddTransient<IWordsProvider, WordsProvider>()
                .AddTransient<IUserInputParser, UserInputParser>()

                // utilities
                .AddTransient<IConsoleCommandExecutor, ConsoleCommandExecutor>()
                .AddSingleton<UserInput>(Console.ReadLine)
                .AddSingleton<UserOutput>(sp =>
                {
                    var logger = sp.GetService<ILoggerFactory>()
                        .CreateLogger("output");

                    return (text, level) => logger.Log(level, text);
                })

                // options
                .AddOptions()
                .Configure<GameOptions>(configuration.GetSection("GameOptions"));

            return serviceCollection;
        }

        public IConfiguration BuildConfigurationProvider()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("config.json", false, false)
                .Build();
        }

        private ILoggerProvider GetLoggerProvider()
        {
            var configureNamedOptions = 
                new ConfigureNamedOptions<ConsoleLoggerOptions>("", opts => { });

            var optionsFactory = new OptionsFactory<ConsoleLoggerOptions>(
                new[] { configureNamedOptions },
                Enumerable.Empty<IPostConfigureOptions<ConsoleLoggerOptions>>());

            var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(
                optionsFactory,
                Enumerable.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(),
                new OptionsCache<ConsoleLoggerOptions>());

            return new ConsoleLoggerProvider(optionsMonitor);
        }
    }
}
