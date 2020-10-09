using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common.Delegates;

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

        public bool TryGetNotEmptyString(string fieldName, out string parsedValue)
        {
            _userOutput($"Enter the `{fieldName}`:");

            parsedValue = _userInput();

            var isValid = !string.IsNullOrWhiteSpace(parsedValue);

            if (!isValid)
            {
                _userOutput(
                    $"Invalid input. The `{fieldName}` should not be empty. Terminating action");
            }

            return isValid;
        }
    }
}
