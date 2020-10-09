using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
using HangmanGame.Common.Console;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Extensions;

namespace HangmanGame.App.Menu
{
    internal class MainMenu : ConsoleMenuBase
    {
        private static readonly Random Random = new Random();
        private readonly IWordsProvider _wordsProvider;
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;
        private readonly IUserInputParser _userInputParser;
        private readonly IGameRoundDrawer _gameRoundDrawer;

        public MainMenu(IWordsProvider wordsProvider, 
            UserInput userInput, 
            UserOutput userOutput,
            IUserInputParser userInputParser,
            IGameRoundDrawer gameRoundDrawer
            )
        {
            _wordsProvider = wordsProvider;
            _userInput = userInput;
            _userOutput = userOutput;
            _userInputParser = userInputParser;
            _gameRoundDrawer = gameRoundDrawer;
        }

        [MenuItem("Play game", "1")]
        public async Task<bool> PlayGame()
        {
            _userOutput("Please select an action");

            if (!TryGetCategory(out var category))
            {
                _userOutput("Invalid category");
                return await Failure();
            }

            var wordToGuess = await GetWordToGuess(category);

            PlayGame(category, wordToGuess);

            return await Success();
        }

        private bool TryGetCategory(out string category)
        {
            var eligibleCategories = _wordsProvider.GetWordCategories();
            var categoriesString = string.Join(", ", eligibleCategories);

            _userOutput($"Available categories: {categoriesString}.");
            _userOutput("Please select a category from options above from which the word to guess will be chosen");

            _userInputParser.TryGetNotEmptyString("category", out var parsedCategory);

            category = parsedCategory;

            return eligibleCategories.ContainsString(category, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<string> GetWordToGuess(string category)
        {
            var wordsByCategory = await _wordsProvider.GetWordsByCategoryAsync(category);

            if (!wordsByCategory.Any())
            {
                _userOutput($"Error occurred: no words found by category: {category}");
                return null;
            }

            var wordIndex = Random.Next(0, wordsByCategory.Count - 1);

            return wordsByCategory.ElementAt(wordIndex);
        }

        private void PlayGame(string category, string wordToGuess)
        {
            var leftAttempts = Constants.MaxAttemptsCount;
            var foundLetters = new HashSet<char>();
            var foundLettersGoal = wordToGuess.Distinct().Count();
            var isVictory = false;

            do
            {
                _gameRoundDrawer.DrawGameRound(category, wordToGuess, foundLetters, leftAttempts);

                if (!_userInputParser.TryGetCharLetter("letter",
                    out var enteredChar, Constants.AbortGamePhrase))
                {
                    _userOutput($"Press `Enter` to continue{Constants.Nl}");
                    _userInput();
                    continue;
                }

                ProcessEnteredGuessedLetter(wordToGuess, enteredChar, foundLetters, ref leftAttempts);

            } while (!IsGameFinished(foundLetters, leftAttempts, foundLettersGoal, out isVictory));

            // todo add victory/lose screen with correct word written and press enter, returning to main menu
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
            }
            else
            {
                leftAttempts--;
            }
        }

        private bool IsGameFinished(IReadOnlyCollection<char> foundLetters, int leftAttempts, 
            int foundLettersGoal,  out bool isVictory)
        {
            isVictory = false;
            if (leftAttempts == 0)
            {
                return true;
            }
            if (foundLettersGoal == foundLetters.Count)
            {
                isVictory = true;
                return true;
            }

            return false;
        }
    }
}
