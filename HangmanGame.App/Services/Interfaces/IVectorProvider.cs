namespace HangmanGame.App.Services.Interfaces
{
    internal interface IVectorProvider
    {
        string GetFullHangmanVector();

        string GetHangmanVectorByAttempt(int leftAttempts);
    }
}