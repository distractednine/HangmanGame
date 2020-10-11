using HangmanGame.Common.Console.Interfaces;

namespace HangmanGame.Common.Console
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void Clear()
        {
            System.Console.Clear();
        }
    }
}
