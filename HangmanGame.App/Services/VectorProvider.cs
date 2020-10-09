using HangmanGame.App.Services.Interfaces;

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
            return HangmanVectors[LastHangmanVectorIndex - leftAttempts];
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
