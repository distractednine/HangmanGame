namespace HangmanGame.App.Services.Interfaces
{
    internal interface IUserInputParser
    {
        bool TryGetNotEmptyString(string fieldName, out string parsedValue,
            string abortString = null);

        bool TryGetCharLetter(string fieldName, out char parsedValue,
            string abortString = null);
    }
}