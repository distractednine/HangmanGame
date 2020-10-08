using System.Threading.Tasks;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common.Console;
using HangmanGame.Common.Delegates;

namespace HangmanGame.App.Menu
{
    internal class MainMenu : ConsoleMenuBase
    {
        private readonly IWordsProvider _wordsProvider;
        private readonly UserInput _userInput;
        private readonly UserOutput _userOutput;

        public MainMenu(IWordsProvider wordsProvider, 
            UserInput userInput, 
            UserOutput userOutput)
        {
            _wordsProvider = wordsProvider;
            _userInput = userInput;
            _userOutput = userOutput;
        }

        [MenuItem("Select word category", "1")]
        public Task<bool> SelectCategory()
        {
            return Success();
        }
    }
}
