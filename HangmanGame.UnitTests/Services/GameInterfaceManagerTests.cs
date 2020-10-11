using FluentAssertions;
using HangmanGame.App.Services;
using Xunit;
using NSubstitute;
using HangmanGame.Common.Delegates;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;
using HangmanGame.Common.Console.Interfaces;
using HangmanGame.Common.Enums;
using HangmanGame.UnitTests.Extensions;

namespace HangmanGame.UnitTests.Services
{
    public class GameInterfaceManagerTests
    {
        private readonly IVectorProvider _vectorProvider;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string WordToGuess = "testing";

        public GameInterfaceManagerTests()
        {
            _vectorProvider = new VectorProvider();
            _consoleWrapper = Substitute.For<IConsoleWrapper>();
        }

        [Fact]
        public void ShowGameRound_DrawsCorrectly()
        {
            // Arrange
            var category = "automobiles";
            var foundLetters = new[] { 't', 'g' };
            var leftAttempts = 2;

            var userOutputMock = Substitute.For<UserOutput>();
            var systemUnderTest = 
                new GameInterfaceManager(_vectorProvider, userOutputMock, _consoleWrapper);

            // Act
            systemUnderTest.ShowGameRound(category, WordToGuess, foundLetters, leftAttempts);

            // Assert
            var userOutputText = userOutputMock.GetUserOutputText();

            userOutputText.Should()
                .Contain($"Category: {category}");

            userOutputText.Should()
                .Contain("Word to guess: t _ _ t _ _ g");

            userOutputText.Should()
                .Contain(_vectorProvider.GetHangmanVectorByAttempt(leftAttempts));
        }

        [Fact]
        public void ShowGameResult_DrawsVictoryCorrectly()
        {
            // Arrange 
            var userOutputMock = Substitute.For<UserOutput>();
            var systemUnderTest =
                new GameInterfaceManager(_vectorProvider, userOutputMock, _consoleWrapper);

            // Act
            systemUnderTest.ShowGameResult(GameResult.Victory, WordToGuess);

            // Assert
            var userOutput = userOutputMock.GetUserOutputText();

            userOutput.Should()
                .Contain("You win!");
            userOutput.Should()
                .Contain($"Word: {WordToGuess}");
            userOutput.Should()
                .Contain(_vectorProvider.GetHangmanVectorByAttempt(
                    Constants.MaxAttemptsCount));
        }


        [Fact]
        public void ShowGameResult_DrawsLossCorrectly()
        {
            // Arrange 
            var userOutputMock = Substitute.For<UserOutput>();
            var systemUnderTest =
                new GameInterfaceManager(_vectorProvider, userOutputMock, _consoleWrapper);

            // Act
            systemUnderTest.ShowGameResult(GameResult.Loss, WordToGuess);

            // Assert
            var userOutputText = userOutputMock.GetUserOutputText();

            userOutputText.Should()
                .Contain("You lose!");
            userOutputText.Should()
                .Contain($"Word: {WordToGuess}");
            userOutputText.Should()
                .Contain(_vectorProvider.GetHangmanVectorByAttempt(
                    Constants.ZeroAttempt));
        }
    }
}
