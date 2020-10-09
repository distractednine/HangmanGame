using System;
using System.Collections.Generic;
using System.Linq;

namespace HangmanGame.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainsString(this IEnumerable<string> enumerable, string targetString,
            StringComparison comparison)
        {
            return Array.FindIndex(
                       enumerable.ToArray(), 
                       x => x.Equals(targetString, comparison))
                   > -1;
        }

        public static bool ContainsChar(this IEnumerable<char> enumerable, char targetChar,
            StringComparison comparison)
        {
            return Array.FindIndex(
                       enumerable.ToArray(),
                       x => x.ToString().Equals(targetChar.ToString(), comparison))
                   > -1;
        }
    }
}
