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

        public string  code { get; set; }
    
        public int http_code { get; set; }
        public string content_type { get; set; }

        public JSchema schema { get; set; }
         

        public ExpectedResponse (string code, int http_code, string content_type, JSchema schema)
        {
            this.code = code;
            this.http_code = http_code;
            this.content_type = content_type;
            this.schema = schema;
        }
    
    


    }
}
