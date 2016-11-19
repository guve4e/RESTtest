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
        public int id { get; set; }

        public Dictionary<string, string> header = new Dictionary<string, string>();

        public string json_data { get; set; }

        public RestRequest() { }
        public RestRequest(int u_id, string url, string method, string date, string body, string controller,string parameters)
        {
            this.id = u_id;
            this.url = url;
            this.controller = controller;
            this.method = method;
            this.type = type;
        }
    }
}
