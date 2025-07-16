using System.Runtime.Serialization;

namespace ReverseProxyManager.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
