using System;

namespace Blazored.LocalStorage.Exceptions
{
    public class BrowserStorageDisabledException : Exception
    {
        public BrowserStorageDisabledException()
        {
        }

        public BrowserStorageDisabledException(string message) : base(message)
        {
        }

        public BrowserStorageDisabledException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
