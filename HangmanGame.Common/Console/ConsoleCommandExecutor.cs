using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HangmanGame.Common.Console.Interfaces;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace HangmanGame.Common.Console
{
    public class ConsoleCommandExecutor : IConsoleCommandExecutor
    {
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;
        private readonly ILogger<ConsoleCommandExecutor> _logger;
        private readonly IConsoleWrapper _consoleWrapper;

        public ConsoleCommandExecutor(UserInput userInput, 
            UserOutput userOutput, 
            ILogger<ConsoleCommandExecutor> logger,
            IConsoleWrapper consoleWrapper)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _logger = logger;
            _consoleWrapper = consoleWrapper;
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
                catch (GameAbortException)
                {
                    _consoleWrapper.Clear();
                    _userOutput("User aborted the game.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred on action execution");
                }

                var result = isSuccessful ? "success" : "failure";

                _userOutput(
                    $"Action finished with {result}{Constants.Nl}",
                    LogLevel.Trace);
                _userOutput(Constants.PressEnterCont);

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
                    ShowTerminatingOutput(true);
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
