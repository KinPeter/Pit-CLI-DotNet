using System;

namespace Pit.Process
{
    public class CustomProcessException : Exception
    {
        public CustomProcessException() { }
        public CustomProcessException(string message) : base(message) { }
        public CustomProcessException(string message, Exception innerException) : base(message, innerException) { }
    }
}