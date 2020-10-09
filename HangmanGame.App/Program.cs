using System;
using System.Threading.Tasks;
using HangmanGame.App.Menu;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common.Console.Interfaces;
using HangmanGame.Common.Delegates;
using Microsoft.Extensions.DependencyInjection;

namespace HangmanGame.App
{
    public class Program
    {
        private static async Task Main()
        {
            // ReSharper disable once AssignNullToNotNullAttribute - used to set window title
            Console.Title = typeof(Program).Namespace;

            var serviceConfiguration = new ServiceConfiguration();
            var config = serviceConfiguration.BuildConfigurationProvider();
            var serviceProvider =
                serviceConfiguration.BuildServiceProvider(config);

            await ShowMainMenu(serviceProvider);
        }

        private static async Task ShowMainMenu(IServiceProvider serviceProvider)
        {
            var consoleCommandExecutor = serviceProvider.GetService<IConsoleCommandExecutor>();
            var wordProvider = serviceProvider.GetService<IWordsProvider>();
            var userOutput = serviceProvider.GetService<UserOutput>();
            var userInputParser = serviceProvider.GetService<IUserInputParser>();
            var gameDrawer = serviceProvider.GetService<IGameInterfaceManager>();
            var gameMediator = serviceProvider.GetService<IGameMediator>();

            var mainMenu =
                new MainMenu(wordProvider,
                    userOutput,
                    userInputParser,
                    gameDrawer,
                    gameMediator);

            gameDrawer.ShowGreeting();

            await consoleCommandExecutor.ShowMenuWithActions(mainMenu, true);
        }
    }
}
