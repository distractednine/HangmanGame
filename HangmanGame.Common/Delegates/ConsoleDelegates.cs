using Microsoft.Extensions.Logging;

namespace HangmanGame.Common.Delegates
{
    public delegate string UserInput();

    public delegate void UserOutput(string message, LogLevel level = LogLevel.Debug);
}
