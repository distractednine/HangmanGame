using System.Linq;
using HangmanGame.Common.Delegates;
using NSubstitute;

namespace HangmanGame.UnitTests.Extensions
{
    internal static class MockExtensions
    {
        public static string GetUserOutputText(this UserOutput userOutputMock)
        {
            return 
                userOutputMock.ReceivedCalls()
                    .Select(x => x.GetArguments().First())
                    .Aggregate((x, y) => $"{x}{y}")
                    .ToString();
        }
    }
}
