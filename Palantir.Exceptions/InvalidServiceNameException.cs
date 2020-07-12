namespace Palantir.Exceptions
{
    [System.Serializable]
    public class InvalidServiceNameException : System.Exception
    {
        public InvalidServiceNameException()
        {
        }

        public InvalidServiceNameException(string message)
        : base(message)
        {
        }

        public InvalidServiceNameException(string message, System.Exception inner)
        : base(message, inner)
        {
        }

        protected InvalidServiceNameException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}