using System;
using System.Collections.Generic;
using System.Linq;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Enums;
using HangmanGame.Common.Exceptions;

namespace HangmanGame.App.Services
{
    internal class GameInterfaceManager : IGameInterfaceManager
    {
        private readonly IVectorProvider _vectorProvider;
        private readonly UserOutput _userOutput;

        public GameInterfaceManager(IVectorProvider vectorProvider, UserOutput userOutput)
        {
            _vectorProvider = vectorProvider;
            _userOutput = userOutput;
        }

        public void ShowGreeting()
        {
            var vector = _vectorProvider.GetFullHangmanVector();
            _userOutput($"Welcome to Hangman Game!{Constants.Nl}{vector}");
        }

        public void ShowGameRound(string category, string word, IReadOnlyCollection<char> foundLetters, 
            int leftAttempts)
        {
            Console.Clear();

            ShowCategory(category);
            ShowWord(word, foundLetters);
            ShowHangman(leftAttempts, "Your progress:");
            ShowInputPrompt();
        }

        public void ShowGameResult(GameResult gameResult, string word)
        {
            if (gameResult != GameResult.Victory && gameResult != GameResult.Loss)
            {
                throw new HangmanGameException($"Invalid {nameof(gameResult)} provided!");
            }

            Console.Clear();

            string gameResultOutput;
            int hangmanState;

            if (gameResult == GameResult.Victory)
            {
                gameResultOutput = "You win!";
                hangmanState = Constants.MaxAttemptsCount;
            }
            else
            {
                gameResultOutput = "You lose!";
                hangmanState = Constants.ZeroAttempt;
            }

            var caption = $"{gameResultOutput}{Constants.Nl}Word: {word}";

            ShowHangman(hangmanState, caption);
        }

        private void ShowCategory(string category)
        {
            _userOutput($"Category: {category}{Constants.Nl}");
        }

        private void ShowWord(string word, IReadOnlyCollection<char> foundLetters)
        {
            var charsToDraw = 
                word.Select(ch => (foundLetters.Contains(ch) ? ch : '_') + " ");

            var stringToDraw = string.Join("", charsToDraw);

            _userOutput($"{Constants.Nl}Word to guess: {stringToDraw}{Constants.Nl}");
        }

        private void ShowHangman(int leftAttempts, string caption)
        {
            var vector = _vectorProvider.GetHangmanVectorByAttempt(leftAttempts);

            _userOutput($"{caption}{Constants.Nl}{vector}{Constants.Nl}");
        }

        private void ShowInputPrompt()
        {
            _userOutput($"[enter `{Constants.AbortGamePhrase}` to abort the game]{Constants.Nl}");
        }
    }
}
