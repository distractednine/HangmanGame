using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HangmanGame.Common.Console.Interfaces;
using HangmanGame.Common.Delegates;
using Microsoft.Extensions.Logging;

namespace HangmanGame.Common.Console
{
    public class ConsoleCommandExecutor : IConsoleCommandExecutor
    {
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;
        private readonly ILogger<ConsoleCommandExecutor> _logger;

        public ConsoleCommandExecutor(UserInput userInput, 
            UserOutput userOutput, 
            ILogger<ConsoleCommandExecutor> logger)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _logger = logger;
        }

        public async Task ShowMenuWithActions<T>(T actionsHolder, bool isMainMenu)
        {
            var menuActions = GetActions(actionsHolder);

            while (true)
            {
                var actionToExecute = ShowMenuAndGetAction(menuActions, isMainMenu);

                if (actionToExecute == null)
                {
                    return;
                }

                _userOutput($"{Constants.Nl}Launching `{actionToExecute.Name}` command", LogLevel.Trace);

                bool isSuccessful = false;

                try
                {
                    isSuccessful = await actionToExecute.Action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred on action execution");
                }

                var result = isSuccessful ? "success" : "failure";
                var level = isSuccessful ? LogLevel.Information : LogLevel.Error;

                _userOutput(
                    $"`{actionToExecute.Name}` command finished execution with {result}{Constants.Nl}Press `Enter` to continue{Constants.Nl}",
                    level);
                _userOutput("");
                _userInput();
            }
        }

        private ICollection<MenuAction> GetActions<T>(T targetObject)
        {
            var actions =
                typeof(T)
                    .GetMethods()
                    .Where(x => x.GetCustomAttributes().OfType<MenuItemAttribute>().Any());

            var builtActions = actions.Select(action =>
            {
                var attr = action.GetCustomAttributes().OfType<MenuItemAttribute>().First();

                var menuAction = (AsyncMenuAction)action.CreateDelegate(typeof(AsyncMenuAction), targetObject);

                return new MenuAction
                {
                    Key = attr.Key,
                    Name = attr.ActionName,
                    Action = menuAction
                };
            });

            return builtActions.ToList();
        }

        private MenuAction ShowMenuAndGetAction(ICollection<MenuAction> menuActions, bool isMainMenu)
        {
            var outputText = new StringBuilder($"Options:{Constants.Nl}");

            menuActions.ToList().ForEach(action =>
            {
                outputText.Append($"{action.Key} - {action.Name} {Constants.Nl}");
            });

            var exitCommand = isMainMenu ?
                $"{Constants.QuitButton} - Quit{Constants.Nl}" :
                $"{Constants.ReturnButton} - Return{Constants.Nl}";

            outputText.Append($"{exitCommand}Enter command:");

            _userOutput(outputText.ToString());

            return ParseInputAction(menuActions, isMainMenu);
        }

        private MenuAction ParseInputAction(ICollection<MenuAction> menuActions, bool isMainMenu)
        {
            while (true)
            {
                var userInput = _userInput()?.ToLowerInvariant();

                if (string.IsNullOrEmpty(userInput))
                {
                    continue;
                }

                var foundAction = menuActions.FirstOrDefault(x => x.Key == userInput);

                if (foundAction != null)
                {
                    return foundAction;
                }
                if (userInput == Constants.QuitButton && isMainMenu)
                {
                    ShowTerminatingOutput(isMainMenu);
                    return null;
                }
                if (userInput == Constants.ReturnButton && !isMainMenu)
                {
                    return null;
                }

                _userOutput("Invalid input");
            }
        }

        public void ShowTerminatingOutput(bool isMainMenu)
        {
            var text = isMainMenu ? "Exiting" : "Returning";
            _userOutput($"{Constants.Nl}{text}...");
            Thread.Sleep(2000);
        }
    }
}
