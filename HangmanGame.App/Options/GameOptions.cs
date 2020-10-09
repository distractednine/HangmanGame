using System.Collections.Generic;

namespace HangmanGame.App.Options
{
    public class GameOptions
    {
        public string WordAssociationsUrlTemplate { get; set; }

        public string WordAssociationsApiKey { get; set; }

        public IReadOnlyCollection<string> Categories { get; set; }

        public int MinimalWordLength { get; set; }
    }
}
