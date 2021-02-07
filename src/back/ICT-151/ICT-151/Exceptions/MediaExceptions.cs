using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Exceptions
{

    [Serializable]
    public class UnsupportedMediaTypeException : Exception
    {
        public UnsupportedMediaTypeException() { }
        public UnsupportedMediaTypeException(string message) : base(message) { }
        public UnsupportedMediaTypeException(string message, Exception inner) : base(message, inner) { }
        protected UnsupportedMediaTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
