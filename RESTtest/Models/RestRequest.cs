using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTtest.Models
{
    public class RestRequest
    {
        public string url { get; set; }
        public string controller { get; set; }
        public string method { get; set; }
        public string type { get; set; }

        public Dictionary<string, string> header = new Dictionary<string, string>();

        public string json_data { get; set; }
    }
}
