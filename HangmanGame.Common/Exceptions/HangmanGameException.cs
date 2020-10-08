using System;
using System.Runtime.Serialization;

namespace HangmanGame.Common.Exceptions
{
    [Serializable]
    public class HangmanGameException : Exception
    {
        public HangmanGameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public HangmanGameException(string message)
            : base(message)
        {
        }

        protected HangmanGameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
