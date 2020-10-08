using System.Threading.Tasks;

namespace HangmanGame.Common.Console
{
    public delegate Task<bool> AsyncMenuAction();

    public class MenuAction
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public AsyncMenuAction Action { get; set; }
    }
}
