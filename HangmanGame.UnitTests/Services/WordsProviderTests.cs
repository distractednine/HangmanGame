using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HangmanGame.App.Options;
using HangmanGame.App.Services;
using HangmanGame.UnitTests.Stubs;
using HangmanGame.UnitTests.TestData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace HangmanGame.UnitTests.Services
{
    public class WordsProviderTests
    {
        private const string Category = "testCategory";
        private const string InvalidResponseText = "invalidResponseText";

        private readonly IOptions<GameOptions> _gameOptionsMock;
        private readonly ILogger<WordsProvider> _loggerMock;
        
        public WordsProviderTests()
        {
            _gameOptionsMock = GetGameOptionsMock();
            _loggerMock = Substitute.For<ILogger<WordsProvider>>();
        }

        [Fact]
        public async Task GetWordsByCategoryAsync_CanReturnWordsByCategory()
        {
            // Arrange
            var clientFactoryMock = 
                GetHttpClientFactoryMock(
                    HttpStatusCode.OK, ServerResponses.WordAssociationsJsonResponse);
            var systemUnderTest = 
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            var wordsByCategory = await systemUnderTest.GetWordsByCategoryAsync(Category);

            // Assert
            wordsByCategory.Should()
                .Contain("gardening");

            wordsByCategory.Should()
                .Contain("collecting");

            wordsByCategory.Should()
                .Contain("cooking");
        }

        [Fact]
        public async Task GetWordsByCategoryAsync_ThrowsIfResponseStatusCodeIsNotSuccessful()
        {
            // Arrange
            var clientFactoryMock =
                GetHttpClientFactoryMock(
                    HttpStatusCode.BadRequest, InvalidResponseText);
            var systemUnderTest =
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            Func<Task<IReadOnlyCollection<string>>> action = 
                () => systemUnderTest.GetWordsByCategoryAsync(Category);

            // Assert
            await action.Should()
                .ThrowAsync<HttpRequestException>();
        }

        [Theory]
        [InlineData(InvalidResponseText)]
        [InlineData("")]
        public async Task GetWordsByCategoryAsync_ThrowsIfResponseCannotBeParsed(
            string serverResponseContent)
        {
            // Arrange
            var clientFactoryMock =
                GetHttpClientFactoryMock(
                    HttpStatusCode.OK, serverResponseContent);
            var systemUnderTest =
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            Func<Task<IReadOnlyCollection<string>>> action =
                () => systemUnderTest.GetWordsByCategoryAsync(Category);

            // Assert
            await action.Should()
                .ThrowAsync<JsonReaderException>();
        }

        [Fact]
        public async Task GetWordsByCategoryOrEmptyAsync_CanReturnWordsByCategory()
        {
            // Arrange
            var clientFactoryMock =
                GetHttpClientFactoryMock(
                    HttpStatusCode.OK, ServerResponses.WordAssociationsJsonResponse);
            var systemUnderTest =
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            var wordsByCategory = await systemUnderTest.GetWordsByCategoryOrEmptyAsync(Category);

            // Assert
            wordsByCategory.Should()
                .Contain("gardening");

            wordsByCategory.Should()
                .Contain("collecting");

            wordsByCategory.Should()
                .Contain("cooking");
        }

        [Fact]
        public async Task GetWordsByCategoryOrEmptyAsync_ReturnsEmptyIfResponseStatusCodeIsNotSuccessful()
        {
            // Arrange
            var clientFactoryMock =
                GetHttpClientFactoryMock(
                    HttpStatusCode.BadRequest, InvalidResponseText);
            var systemUnderTest =
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            var wordsByCategory = await systemUnderTest.GetWordsByCategoryOrEmptyAsync(Category);

            // Assert
            wordsByCategory.Should()
                .BeEmpty();
        }

        [Theory]
        [InlineData(InvalidResponseText)]
        [InlineData("")]
        public async Task GetWordsByCategoryOrEmptyAsync_ReturnsEmptyIfResponseCannotBeParsed(
            string serverResponseContent)
        {
            // Arrange
            var clientFactoryMock =
                GetHttpClientFactoryMock(
                    HttpStatusCode.OK, serverResponseContent);
            var systemUnderTest =
                new WordsProvider(_gameOptionsMock, clientFactoryMock, _loggerMock);

            // Act
            var wordsByCategory = await systemUnderTest.GetWordsByCategoryOrEmptyAsync(Category);

            // Assert
            wordsByCategory.Should()
                .BeEmpty();
        }

        private static IOptions<GameOptions> GetGameOptionsMock()
        {
            var options = new GameOptions
            {
                WordAssociationsUrlTemplate = "https://api.com/search?apikey={apiKey}&text={text}",
                Categories = new[] { "cat1", "cat2" }
            };

            var gameOptions = Substitute.For<IOptions<GameOptions>>();
            gameOptions.Value.Returns(options);

            return gameOptions;
        }

        private static IHttpClientFactory GetHttpClientFactoryMock(HttpStatusCode statusCode,
            string stringContent)
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var clientHandlerStub = new DelegatingHandlerStub(statusCode, stringContent);
            var client = new HttpClient(clientHandlerStub);

            httpClientFactoryMock
                .CreateClient(Arg.Any<string>())
                .Returns(client);

            return httpClientFactoryMock;
        }
    }
}
