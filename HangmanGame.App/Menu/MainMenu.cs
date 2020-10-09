using System;
using System.Linq;
using System.Threading.Tasks;
using HangmanGame.App.Services.Interfaces;
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
        private readonly IGameMediator _gameMediator;

        public MainMenu(IWordsProvider wordsProvider, 
            UserInput userInput, 
            UserOutput userOutput,
            IUserInputParser userInputParser,
            IGameRoundDrawer gameRoundDrawer,
            IGameMediator gameMediator
            )
        {
            _wordsProvider = wordsProvider;
            _userInput = userInput;
            _userOutput = userOutput;
            _userInputParser = userInputParser;
            _gameRoundDrawer = gameRoundDrawer;
            _gameMediator = gameMediator;
        }

        [MenuItem("Play game", "1")]
        // ReSharper disable once UnusedMember.Global - used by reflection to load actions dynamically
        public async Task<bool> PlayGame()
        {
            if (!TryGetCategory(out var category))
            {
                _userOutput("Invalid category");
                return await Failure();
            }

            var wordToGuess = await GetWordToGuess(category);

            _gameMediator.PlayGame(category, wordToGuess);

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
    }
}
