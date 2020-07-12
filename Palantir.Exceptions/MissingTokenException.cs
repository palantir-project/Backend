namespace Palantir.Exceptions
{
    [System.Serializable]
    public class MissingTokenException : System.Exception
    {
        public MissingTokenException()
        {
        }

        public MissingTokenException(string message)
        : base(message)
        {
        }

        public MissingTokenException(string message, System.Exception inner)
        : base(message, inner)
        {
        }

        protected MissingTokenException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}