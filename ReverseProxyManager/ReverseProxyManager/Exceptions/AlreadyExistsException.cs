using System.Runtime.Serialization;

namespace ReverseProxyManager.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {
                
        }

        public AlreadyExistsException(string message): base(message)
        {
            
        }

        public AlreadyExistsException(string message, Exception innerException): base(message, innerException)
        {

        }

        public AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
