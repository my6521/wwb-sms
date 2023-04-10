using System;

namespace WWB.SMS.Exceptions
{
    public class SMSException : Exception
    {
        public SMSException()
        {
        }

        public SMSException(string message) : base(message)
        {
        }

        public SMSException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}