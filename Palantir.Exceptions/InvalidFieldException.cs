namespace Palantir.Exceptions
{
    [System.Serializable]
    public class InvalidFieldException : System.Exception
    {
        public InvalidFieldException()
        {
        }

        public InvalidFieldException(string message)
        : base(message)
        {
        }

        public InvalidFieldException(string message, System.Exception inner)
        : base(message, inner)
        {
        }

        protected InvalidFieldException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}