using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTtest.Library
{
    /// <summary>
    /// Custom Exception
    /// 
    /// TO_DO
    /// </summary>
    class RestTestException : Exception
    {
        public RestTestException() { }
        public RestTestException(string message) { }
        public RestTestException(string message, int code, Exception previous)
            : base(message, previous)
        { }
    }
}
