using System.Collections.Generic;

namespace HangmanGame.App.Services.Interfaces
{
    internal interface IGameRoundDrawer
    {
        void DrawGameRound(string category, string word, IReadOnlyCollection<char> foundLetters, 
            int leftAttempts);
    }
}