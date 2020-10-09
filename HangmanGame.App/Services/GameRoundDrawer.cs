using System;
using System.Collections.Generic;
using System.Linq;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
using HangmanGame.Common.Delegates;

namespace HangmanGame.App.Services
{
    internal class GameRoundDrawer : IGameRoundDrawer
    {
        private readonly IVectorProvider _vectorProvider;
        private readonly UserOutput _userOutput;

        public GameRoundDrawer(IVectorProvider vectorProvider, UserOutput userOutput)
        {
            _vectorProvider = vectorProvider;
            _userOutput = userOutput;
        }

        public void DrawGameRound(string category, string word, IReadOnlyCollection<char> foundLetters, 
            int leftAttempts)
        {
            Console.Clear();

            DrawCategory(category);
            DrawWord(word, foundLetters);
            DrawHangman(leftAttempts);
            DrawInputPrompt();
        }

        private void DrawCategory(string category)
        {
            _userOutput($"Category: {category}{Constants.Nl}");
        }

        private void DrawWord(string word, IReadOnlyCollection<char> foundLetters)
        {
            var charsToDraw = 
                word.Select(ch => (foundLetters.Contains(ch) ? ch : '_') + " ");

            var stringToDraw = string.Join("", charsToDraw);

            _userOutput($"{Constants.Nl}Word to guess: {stringToDraw}{Constants.Nl}");
        }

        private void DrawHangman(int leftAttempts)
        {
            var vector = _vectorProvider.GetHangmanVectorByAttempt(leftAttempts);

            _userOutput($"Your progress:{Constants.Nl}{vector}{Constants.Nl}");
        }

        private void DrawInputPrompt()
        {
            _userOutput($"[enter `{Constants.AbortGamePhrase}` to abort the game]{Constants.Nl}");
        }
    }
}
