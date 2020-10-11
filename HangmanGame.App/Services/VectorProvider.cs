using System;
using HangmanGame.App.Services.Interfaces;
using HangmanGame.Common;

namespace HangmanGame.App.Services
{
    internal class VectorProvider : IVectorProvider
    {
        public string GetFullHangmanVector()
        {
            return HangmanVectors[LastHangmanVectorIndex];
        }

        public string GetHangmanVectorByAttempt(int leftAttempts)
        {
            var vectorIndex = LastHangmanVectorIndex - leftAttempts;

            if (vectorIndex < Constants.ZeroAttempt || vectorIndex > LastHangmanVectorIndex)
            {
                throw new ArgumentException(nameof(leftAttempts));
            }

            return HangmanVectors[vectorIndex];
        }

        private int LastHangmanVectorIndex => HangmanVectors.Length - 1;

        private string[] HangmanVectors => new[]
        {
            @"
      ------|
      |     |
            |       
            |
            |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
            |
            |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
    /       |
            |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
    /||     |
            |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
    /||\    |
            |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
    /||\    |
    /       |
            |
            |                    
========================
",
            @"
      ------|
      |     |
     ()     |       
    /||\    |
    /  \    |
            |
            |                    
========================
"
        };

        
    }
}
