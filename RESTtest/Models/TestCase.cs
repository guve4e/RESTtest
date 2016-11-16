using RESTtest.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Schema;



namespace RESTtest.Models
{
    class TestCase
    {
       
        private XDocument xdoc;
        private XDocument envdoc;
        private LoadXML book;

        private int HttpCode;
        private string ContentType;
        JSchema schema, data;

        private string url;
        private string method;
        private string controller;

        internal Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">LoadXML Object</param>
        /// <param name="fname">Name of Test Case file</param>
        public TestCase(LoadXML parent, string fname)
        {
            book = parent;
            try
            {

                envdoc = parent.xenv;
                url = parent.url;
                xdoc = XDocument.Load(fname);
                this.controller = xdoc.Root.Attribute("url").Value;
                this.method = xdoc.Root.Attribute("type").Value;
                // if POST, PUT, DELETE
                if (method == "POST")
                {
                    var d = xdoc.Root.Element("data");
                    data = JSchema.Parse(d.Value);
                }
                var xresult = xdoc.Root.Element("result");
 
                string codes = Tools.Attr(xresult, "code");
                HttpCode = Convert.ToInt32(codes);

                ContentType = Tools.Attr(xresult, "type");
                schema = JSchema.Parse(xresult.Value);
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
               
            }
        }

        public void Execute(string tid)
        {
           
            string rtype = xdoc.Root.Attribute("type").Value;
     
            RestRequest request = new RestRequest();
            request.url = this.url;
            request.controller = this.controller;
            request.method = this.method;
            request.header = book.headers;
            if (data != null)
            {
                request.json_data = data.ToString();
            }   

            LoadXML.requests.Add(request);
         
        }

    }
}
