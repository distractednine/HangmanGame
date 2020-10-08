using System.Threading.Tasks;

namespace HangmanGame.Common.Console
{
    public abstract class ConsoleMenuBase
    {
        protected Task<bool> Success()
        {
            return Result(true);
        }

        protected Task<bool> Failure()
        {
            return Result(false);
        }

        protected Task<bool> Result(bool isSuccessful)
        {
            return Task.FromResult(isSuccessful);
        }
    }
}
