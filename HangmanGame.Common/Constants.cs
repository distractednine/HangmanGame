using System;

namespace HangmanGame.Common
{
    public class Constants
    {
        public const string QuitButton = "q";

        public const string ReturnButton = "r";

        public const string AbortGamePhrase = "break";

        public static string PressEnterCont = $"Press `Enter` to continue{Environment.NewLine}";

        public static readonly string Nl = Environment.NewLine;

        public const int MaxAttemptsCount = 6;
    }
}
