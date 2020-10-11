using FluentAssertions;
using HangmanGame.App.Services;
using HangmanGame.App.Services.Interfaces;
using Xunit;

namespace HangmanGame.UnitTests.Services
{
    public class VectorProviderTests
    {
        private readonly IVectorProvider _vectorProvider;

        private const string Vector1 = @"
      ------|
      |     |
     ()     |       
    /||\    |
    /  \    |
            |
            |                    
========================
";

        private const string Vector2 = @"
      ------|
      |     |
     ()     |       
    /       |
            |
            |
            |                    
========================
";

        private const string Vector3 = @"
      ------|
      |     |
     ()     |       
    /||\    |
            |
            |
            |                    
========================
";

        public VectorProviderTests()
        {
            _vectorProvider = new VectorProvider();
        }

        [Fact]
        public void CanGetFullHangmanVector()
        {
            // Arrange
            // Act
            var vector = _vectorProvider.GetFullHangmanVector();

            // Assert
            vector.Should()
                .Be(Vector1);
        }

        [Theory]
        [InlineData(2, Vector3)]
        [InlineData(4, Vector2)]
        public void GetHangmanVectorByAttempt(int attemptNumber, string expectedText)
        {
            // Arrange
            // Act
            var vector = 
                _vectorProvider.GetHangmanVectorByAttempt(attemptNumber);

            // Assert
            vector.Should()
                .Be(expectedText);
        }
    }
}
