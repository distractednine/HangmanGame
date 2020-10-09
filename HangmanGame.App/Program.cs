using System;
using System.Threading.Tasks;
using HangmanGame.App.Menu;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
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
            var vectorProvider = serviceProvider.GetService<IVectorProvider>();

            var wordProvider = serviceProvider.GetService<IWordsProvider>();
            var userOutput = serviceProvider.GetService<UserOutput>();
            var userInput = serviceProvider.GetService<UserInput>();
            var userInputParser = serviceProvider.GetService<IUserInputParser>();
            var gameDrawer = serviceProvider.GetService<IGameRoundDrawer>();
            var gameMediator = serviceProvider.GetService<IGameMediator>();

            ShowGreeting(userOutput, vectorProvider);

            var mainMenu =
                new MainMenu(wordProvider,
                    userInput,
                    userOutput,
                    userInputParser,
                    gameDrawer,
                    gameMediator);

            await consoleCommandExecutor.ShowMenuWithActions(mainMenu, true);
        }

        private static void ShowGreeting(UserOutput userOutput, IVectorProvider vectorProvider)
        {
            var vector = vectorProvider.GetFullHangmanVector();
            userOutput($"Welcome to Hangman Game!{Constants.Nl}{vector}");
        }
    }
}
