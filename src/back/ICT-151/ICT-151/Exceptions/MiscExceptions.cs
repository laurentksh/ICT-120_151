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
}
