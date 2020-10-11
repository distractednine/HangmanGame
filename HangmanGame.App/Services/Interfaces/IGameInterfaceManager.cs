using System.Collections.Generic;
using HangmanGame.Common.Enums;

namespace HangmanGame.App.Services.Interfaces
{
    internal interface IGameInterfaceManager
    {
        void ShowGreeting();

        void ShowGameRound(string category, string word, IReadOnlyCollection<char> foundLetters, 
            int leftAttempts);

        void ShowGameResult(GameResult gameResult, string word);
    }
}