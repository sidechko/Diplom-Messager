using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessagerClient.utils
{
    public class MessagerBaseException : Exception
    {
        public MessagerBaseException(string message) : base(message) { }
        public MessagerBaseException(string message, Exception inner) : base(message, inner) { }
    }
    public class MessagerHardClientException : MessagerBaseException
    {
        public MessagerHardClientException(string message) : base(message) { }
        public MessagerHardClientException(string message, Exception inner): base(message, inner) { }
    }

    public class MessagerSoftClientException : MessagerBaseException
    {
        public MessagerSoftClientException(string message) : base(message) { }
        public MessagerSoftClientException(string message, Exception inner): base(message, inner) { }
    }

    public class MessagerDeserializeException : MessagerBaseException
    {
        public MessagerDeserializeException(string? message) : base(message) { }
        public MessagerDeserializeException(string message, Exception inner) : base(message, inner) { }
    }

    public class MessagerConnectionException : MessagerBaseException
    { 
        public MessagerConnectionException(string? message) : base(message) {}
        public MessagerConnectionException(string message, Exception inner) : base(message, inner) { }
    }

    public class MessagerModelException : MessagerBaseException
    { 
        public MessagerModelException(string? message) : base(message) {}
        public MessagerModelException(string message, Exception inner) : base(message, inner) { }
    }


}
