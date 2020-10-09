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
            // ReSharper disable once AssignNullToNotNullAttribute
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
            var vectorProvider = serviceProvider.GetService<IVectorProvider>();

            var wordProvider = serviceProvider.GetService<IWordsProvider>();
            var userOutput = serviceProvider.GetService<UserOutput>();
            var userInput = serviceProvider.GetService<UserInput>();
            var userInputParser = serviceProvider.GetService<IUserInputParser>();
            var gameDrawer = serviceProvider.GetService<IGameRoundDrawer>();

            ShowGreeting(userOutput, vectorProvider);

            var mainMenu =
                new MainMenu(wordProvider,
                    userInput,
                    userOutput,
                    userInputParser,
                    gameDrawer);

            await consoleCommandExecutor.ShowMenuWithActions(mainMenu, true);
        }

        private static void ShowGreeting(UserOutput userOutput, IVectorProvider vectorProvider)
        {
            userOutput("Hello from Hangman Game!");
            userOutput(vectorProvider.GetFullHangmanVector());
        }
    }
}
