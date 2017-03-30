using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTtest.Models
{
    public class ExpectedResponse
    {
        /// <summary>
        /// Expected code
        /// </summary>
        public string http_code { get; set; }

        /// <summary>
        /// Expected Content Type
        /// </summary>
        public string content_type { get; set; }

        /// <summary>
        /// Expected JSON schema
        /// </summary>
        public JSchema schema { get; set; }
         
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="http_code"></param>
        /// <param name="content_type"></param>
        /// <param name="schema"></param>
        public ExpectedResponse (string http_code, string content_type, JSchema schema)
        {
            this.http_code = http_code;
            this.content_type = content_type;
            this.schema = schema;
        }
    }
}
