using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Exceptions
{

    [Serializable]
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() { }
        public DataNotFoundException(string message) : base(message) { }
        public DataNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected DataNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class DataAlreadyExistsException : Exception
    {
        public DataAlreadyExistsException() { }
        public DataAlreadyExistsException(string message) : base(message) { }
        public DataAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected DataAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
