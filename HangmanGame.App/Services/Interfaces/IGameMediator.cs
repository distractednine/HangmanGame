using HangmanGame.App.Enums;

namespace HangmanGame.App.Services.Interfaces
{
    internal interface IGameMediator
    {
        GameResult PlayGame(string category, string wordToGuess);
    }
}