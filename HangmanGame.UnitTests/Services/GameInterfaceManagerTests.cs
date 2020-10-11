using Xunit;
using NSubstitute;
using HangmanGame.Common.Delegates;
using HangmanGame.App.Services.Interfaces;

namespace HangmanGame.UnitTests.Services
{
    public class GameInterfaceManagerTests
    {
        public GameInterfaceManagerTests()
        {
            var a = Substitute.For<IVectorProvider>();
        }

        [Fact]
        public void ShowGameRound_DrawsCorrectly()
        {
            // Arrange
            var userOutputMock = Substitute.For<UserOutput>();

            // Act


            // Assert
        }
    }
}
