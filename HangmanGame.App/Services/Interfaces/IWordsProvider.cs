using System.Collections.Generic;
using System.Threading.Tasks;

namespace HangmanGame.App.Services.Interfaces
{
    internal interface IWordsProvider
    {
        IReadOnlyCollection<string> GetWordCategories();

        Task<IReadOnlyCollection<string>> GetWordsByCategoryOrEmptyAsync(string category);

        Task<IReadOnlyCollection<string>> GetWordsByCategoryAsync(string category);
    }
}