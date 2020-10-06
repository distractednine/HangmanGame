using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            {

            };

            return serviceCollection;
        }

        public IConfiguration BuildConfigurationProvider()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("config.json", false, false)
                .Build();
        }
    }
}
