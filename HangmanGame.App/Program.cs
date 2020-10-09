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
        private static async Task Main(string[] args)
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
            var wordProvider = serviceProvider.GetService<IWordsProvider>();
            var userOutput = serviceProvider.GetService<UserOutput>();
            var userInput = serviceProvider.GetService<UserInput>();
            var userInputParser = serviceProvider.GetService<IUserInputParser>();

            userOutput("Hello from Hangman Game!");
            userOutput("Please select the action");

            var mainMenu =
                new MainMenu(wordProvider,
                    userInput,
                    userOutput,
                    userInputParser
                    );

            await consoleCommandExecutor.ShowMenuWithActions(mainMenu, true);
        }
    }
}
