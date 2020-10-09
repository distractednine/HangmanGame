using System;
using System.Runtime.Serialization;

namespace HangmanGame.Common.Exceptions
{
    [Serializable]
    public class GameAbortException : HangmanGameException
    {
        public GameAbortException()
            : base("")
        {
        }

        public GameAbortException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public GameAbortException(string message)
            : base(message)
        {
        }

        protected GameAbortException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
