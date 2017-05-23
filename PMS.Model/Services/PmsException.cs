using System;

namespace PMS.Model.Services
{
    public class PmsException : Exception
    {
        public PmsException()
        {
            
        }

        public PmsException(string message) : base(message)
        {
            
        }

        public PmsException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
