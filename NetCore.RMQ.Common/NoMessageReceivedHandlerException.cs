using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.RMQ.Common
{
    public class NoMessageReceivedHandlerException : Exception
    {
        public NoMessageReceivedHandlerException() : base("No callbacks registered for this queue consumer, please make sure there is at least one to prevent from messages being lost")
        {
        }
    }
}