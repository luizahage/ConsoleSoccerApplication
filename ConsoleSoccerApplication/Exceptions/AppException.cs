using System;

namespace ConsoleSoccerApplication.Exceptions
{
    public class AppException : ApplicationException
    {
        public AppException(string message) : base(message)
        {
        }
    }
}
