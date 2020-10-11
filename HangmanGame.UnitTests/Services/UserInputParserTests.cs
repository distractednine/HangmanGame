using System;
using FluentAssertions;
using HangmanGame.App.Services;
using HangmanGame.Common;
using HangmanGame.Common.Delegates;
using HangmanGame.Common.Exceptions;
using NSubstitute;
using Xunit;

namespace HangmanGame.UnitTests.Services
{
    public class UserInputParserTests
    {
        private readonly UserOutput _userOutputMock;

        private const string FieldName = "testFieldName";

        public UserInputParserTests()
        {
            _userOutputMock = Substitute.For<UserOutput>();
        }

        [Theory]
        [InlineData("testUserInput1", "testUserInput1")]
        [InlineData(" testUserInput2 ", "testUserInput2")]
        public void ParseString_CanParseCorrectString(string inputString, string expectedString)
        {
            // Arrange
            var userInputMock = GetSetupUserInput(inputString);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            var isParsed = 
                systemUnderTest.TryGetNotEmptyString(FieldName, out var parsedValue);

            // Assert
            isParsed.Should()
                .BeTrue();

            parsedValue.Should()
                .Be(expectedString);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ParseString_CanHandleEmptyString(string inputString)
        {
            // Arrange
            var userInputMock = GetSetupUserInput(inputString);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            var isParsed =
                systemUnderTest.TryGetNotEmptyString(FieldName, out var parsedValue);

            // Assert
            isParsed.Should()
                .BeFalse();

            parsedValue.Should()
                .Be(string.Empty);
        }

        [Fact]
        public void ParseString_CanThrowOnAbortInput()
        {
            // Arrange
            var userInputMock = GetSetupUserInput(Constants.AbortGamePhrase);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            Action action = () =>
                systemUnderTest.TryGetNotEmptyString(
                    FieldName, out _, Constants.AbortGamePhrase);

            // Assert
            action.Should().Throw<GameAbortException>();
        }

        [Theory]
        [InlineData("a", 'a')]
        [InlineData(" b ", 'b')]
        public void ParseChar_CanParseCorrectChar(string inputString, char expectedChar)
        {
            // Arrange
            var userInputMock = GetSetupUserInput(inputString);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            var isParsed =
                systemUnderTest.TryGetCharLetter(FieldName, out var parsedValue);

            // Assert
            isParsed.Should()
                .BeTrue();

            parsedValue.Should()
                .Be(expectedChar);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("*")]
        public void ParseChar_CanHandleInvalidInput(string inputString)
        {
            // Arrange
            var userInputMock = GetSetupUserInput(inputString);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            var isParsed =
                systemUnderTest.TryGetCharLetter(FieldName, out _);

            // Assert
            isParsed.Should()
                .BeFalse();
        }

        [Fact]
        public void ParseChar_CanThrowOnAbortInput()
        {
            // Arrange
            var userInputMock = GetSetupUserInput(Constants.AbortGamePhrase);
            var systemUnderTest = new UserInputParser(userInputMock, _userOutputMock);

            // Act
            Action action = () =>
                systemUnderTest.TryGetCharLetter(
                    FieldName, out _, Constants.AbortGamePhrase);

            // Assert
            action.Should().Throw<GameAbortException>();
        }

        private static UserInput GetSetupUserInput(string inputString)
        {
            var userInputMock = Substitute.For<UserInput>();

            userInputMock.Invoke()
                .Returns(x => inputString);

            return userInputMock;
        }
    }
}
