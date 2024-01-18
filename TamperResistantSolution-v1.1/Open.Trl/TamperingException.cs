using System;
using System.Collections.Generic;
using System.Text;

namespace Open.Trl
{
    public class TamperingException : ApplicationException
    {
        public TamperingException() : base() { }

        public TamperingException(string message) : base(message) { }

        public TamperingException(string message, Exception innerException) : base(message, innerException) { }

        public TamperingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
