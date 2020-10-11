using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HangmanGame.App.Options;
using HangmanGame.App.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace HangmanGame.App.Services
{
    internal class WordsProvider : IWordsProvider
    {
        private const string WordUrlSection = "{text}";
        private const string ApiKeyUrlSection = "{apiKey}";

        private const string RetrievingWordsByCategoryErrorMessage =
            "Error occurred while obtaining the words by category";

        private readonly GameOptions _gameOptions;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WordsProvider> _logger;

        public WordsProvider(IOptions<GameOptions> gameOptions, 
            IHttpClientFactory httpClientFactory,
            ILogger<WordsProvider> logger)
        {
            _gameOptions = gameOptions.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IReadOnlyCollection<string> GetWordCategories() => _gameOptions.Categories;

        public async Task<IReadOnlyCollection<string>> GetWordsByCategoryOrEmptyAsync(
            string category)
        {
            try
            {
                return await GetWordsByCategoryAsync(category);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, RetrievingWordsByCategoryErrorMessage);

                return new string[0];
            }
        }

        public async Task<IReadOnlyCollection<string>> GetWordsByCategoryAsync(string category)
        {
            var url =
                _gameOptions.WordAssociationsUrlTemplate
                    .Replace(WordUrlSection, category)
                    .Replace(ApiKeyUrlSection, _gameOptions.WordAssociationsApiKey);

            var httpClient = _httpClientFactory.CreateClient();

            var serverResponse = await httpClient.GetAsync(url);

            serverResponse.EnsureSuccessStatusCode();

            var responseString = await serverResponse.Content.ReadAsStringAsync();

            return GetWordsFromApiResponse(responseString, _gameOptions.MinimalWordLength);
        }

        private static IReadOnlyCollection<string> GetWordsFromApiResponse(
            string responseString, int minWordLength)
        {
            var responseChildren =
                JObject.Parse(responseString)
                    ["response"].Children<JObject>();

            var responseItems =
                responseChildren.Any() ?
                    responseChildren.First()["items"] : new JArray();

            var words =
                responseItems
                    .AsJEnumerable()
                    .Select(x => x["item"].Value<string>().ToLowerInvariant())
                    .Where(x => x.Length >= minWordLength)
                    .ToArray();

            return words;
        }
    }
}
