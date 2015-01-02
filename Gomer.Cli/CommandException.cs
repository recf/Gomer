using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Cli
{
    public class CommandException : Exception
    {
        public int StatusCode { get; private set; }

        public CommandException(string message) : this(1, message) { }

        public CommandException(int statusCode, string message) : this(statusCode, message, null) { }

        public CommandException(int statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
