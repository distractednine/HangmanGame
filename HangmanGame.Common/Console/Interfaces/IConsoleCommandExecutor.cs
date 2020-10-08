using System.Threading.Tasks;

namespace HangmanGame.Common.Console.Interfaces
{
    public interface IConsoleCommandExecutor
    {
        Task ShowMenuWithActions<T>(T actionsHolder, bool isMainMenu);

        void ShowTerminatingOutput(bool isMainMenu);
    }
}