﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HangmanGame.App.Options;
using HangmanGame.App.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace HangmanGame.App.Services
{
    internal class WordsProvider : IWordsProvider
    {
        private const string WordUrlSection = "{text}";
        private const string ApiKeyUrlSection = "{apiKey}";
        private const int MinWordLength = 3;

        private readonly GameOptions _gameOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public WordsProvider(IOptions<GameOptions> gameOptions, IHttpClientFactory httpClientFactory)
        {
            _gameOptions = gameOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        public IReadOnlyCollection<string> GetWordCategories() => _gameOptions.Categories;

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

            return GetWordsFromApiResponse(responseString);
        }

        private static IReadOnlyCollection<string> GetWordsFromApiResponse(string responseString)
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
                    .Select(x => x["item"].Value<string>())
                    .Where(x => x.Length >= MinWordLength)
                    .ToArray();

            return words;
        }
    }
}