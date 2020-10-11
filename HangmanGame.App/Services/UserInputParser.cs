using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Exceptions;

namespace HangmanGame.App.Services
{
    internal class UserInputParser : IUserInputParser
    {
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;

        public UserInputParser(UserInput userInput, UserOutput userOutput)
        {
            _userInput = userInput;
            _userOutput = userOutput;
        }

        public bool TryGetNotEmptyString(string fieldName, out string parsedValue, string abortString = null)
        {
            _userOutput($"Enter the `{fieldName}`:");

            parsedValue = _userInput().Trim();

            var isNotEmpty = !string.IsNullOrWhiteSpace(parsedValue);

            if (!isNotEmpty)
            {
                _userOutput(
                    $"Invalid input. The `{fieldName}` should not be empty. Terminating action");
            }
            if (string.Equals(parsedValue, abortString))
            {
                throw new GameAbortException();
            }

            return isNotEmpty;
        }

        public bool TryGetCharLetter(string fieldName, out char parsedValue, string abortString = null)
        {
            parsedValue = default;
            if (!TryGetNotEmptyString(fieldName, out var parsedString, abortString))
            {
                return false;
            }

            var isValidChar = 
                char.TryParse(parsedString, out var parsedChar) && 
                char.IsLetter(parsedChar);

            parsedValue = parsedChar;

            if (!isValidChar)
            {
                _userOutput("Invalid input. Only one letter should be entered");
            }

            return isValidChar;
        }
    }
}
