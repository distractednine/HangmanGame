namespace HangmanGame.App.Services.Interfaces
{
    internal interface IUserInputParser
    {
        bool TryGetNotEmptyString(string fieldName, out string parsedValue);
    }
}