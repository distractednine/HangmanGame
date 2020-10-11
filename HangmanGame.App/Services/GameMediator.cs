using System;
using System.Collections.Generic;
using System.Linq;
using HangmanGame.App.Enums;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace HangmanGame.App.Services
{
    internal class GameMediator : IGameMediator
    {
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;
        private readonly IUserInputParser _userInputParser;
        private readonly IGameInterfaceManager _gameInterfaceManager;

        public GameMediator(UserInput userInput, UserOutput userOutput, 
            IUserInputParser userInputParser, 
            IGameInterfaceManager gameInterfaceManager)
        {
            _userInput = userInput;
            _userOutput = userOutput;
            _userInputParser = userInputParser;
            _gameInterfaceManager = gameInterfaceManager;
        }

        public GameResult PlayGame(string category, string wordToGuess)
        {
            var leftAttempts = Constants.MaxAttemptsCount;
            var foundLetters = new HashSet<char>();
            var foundLettersGoal = wordToGuess.Distinct().Count();
            var gameResult = GameResult.InProcess;

            do
            {
                _gameInterfaceManager.ShowGameRound(
                    category, wordToGuess, foundLetters, leftAttempts);

                if (!_userInputParser.TryGetCharLetter("letter",
                    out var enteredChar, Constants.AbortGamePhrase))
                {
                    PromptToContinue();
                    continue;
                }

                ProcessEnteredGuessedLetter(
                    wordToGuess, enteredChar, foundLetters, ref leftAttempts);

                PromptToContinue();

                gameResult = GetGameResult(
                    foundLetters, leftAttempts, foundLettersGoal);

            } while (gameResult == GameResult.InProcess);

            return gameResult;
        }

        private void ProcessEnteredGuessedLetter(string wordToGuess, char enteredChar,
            ICollection<char> foundLetters, ref int leftAttempts)
        {
            var containsChar =
                wordToGuess.ToCharArray()
                    .ContainsChar(enteredChar, StringComparison.OrdinalIgnoreCase);

            if (containsChar)
            {
                foundLetters.Add(enteredChar);
                _userOutput("This letter is present in a guessed word!", LogLevel.Information);
            }
            else
            {
                leftAttempts--;
                _userOutput("This letter is NOT present in a guessed word!", LogLevel.Error);
            }
        }

        private static GameResult GetGameResult(IReadOnlyCollection<char> foundLetters, int leftAttempts,
            int foundLettersGoal)
        {
            if (leftAttempts == 0)
            {
                return GameResult.Loss;
            }
            return foundLettersGoal == foundLetters.Count ? 
                GameResult.Victory : GameResult.InProcess;
        }

        private void PromptToContinue()
        {
            _userOutput(Constants.PressEnterCont);
            _userInput();
        }
    }
}
