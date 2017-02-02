using System;

namespace Common.DI
{
    public class DependencyInjectionException : Exception
    {
        public DependencyInjectionException()
        {
        }
        public DependencyInjectionException(string message) : base(message)
        {
        }

        public DependencyInjectionException(string message, Exception innerException):base(message, innerException)
        {
        }
    }
}