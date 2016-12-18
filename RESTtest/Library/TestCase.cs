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
    /// <summary>
    /// This class represents one Test Case
    /// loaded from XML file in TestCases folder.
    /// 
    /// </summary>
    class TestCase
    {
        /// <summary>
        /// 
        /// </summary>
        private XDocument xdoc;

        /// <summary>
        /// 
        /// </summary>
        private XDocument envdoc;

        /// <summary>
        /// LoadXML Object
        /// 
        /// 
        /// </summary>
        private LoadXML book;

        /// <summary>
        /// 
        /// </summary>
        private int HttpCode;

        /// <summary>
        /// 
        /// </summary>
        private string ContentType;

        /// <summary>
        /// JSON Schema Objects
        /// </summary>
        JSchema schema, data;

        /// <summary>
        /// 
        /// </summary>
        private string url;

        /// <summary>
        /// 
        /// </summary>
        private string method;

        /// <summary>
        /// 
        /// </summary>
        private string controller;

    //    internal Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">LoadXML Object</param>
        /// <param name="fname">Name of Test Case file</param>
        public TestCase(LoadXML parent, string fname)
        {
            // Load the LoadXML object to book
            this.book = parent;

            try
            {
                // Load XML Object from parent 
                this.envdoc = parent.xenv;

                // Load url from parent
                this.url = parent.url;

                // Load the test case
                this.xdoc = XDocument.Load(fname);
                // Load controller
                this.controller = xdoc.Root.Attribute("controller").Value;
                // Load method
                this.method = xdoc.Root.Attribute("method").Value;

                // if POST, PUT, DELETE
                if (this.method == "POST" || this.method == "PUT" || this.method == "DELETE")
                {   // get data and load it to JSON Object
                    var d = xdoc.Root.Element("data");
                    data = JSchema.Parse(d.Value);
                }
                 
                // load the schema
                var schema = xdoc.Root.Element("result");
                // extract code
                string codes = Tools.Attr(schema, "code");
                // parse the http code to int
                HttpCode = Int32.Parse(codes);
                //  HttpCode = Convert.ToInt32(codes);
                // extract content type
                this.ContentType = Tools.Attr(schema, "type");
                // load json schema
                this.schema = JSchema.Parse(schema.Value);
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in TestCase: " + ex);
            }
        }

        /// <summary>
        /// Parse Tests XML
        /// 
        /// </summary>
        /// <param name="tid"></param>
        public void Execute(string tid)
        {

            ExpectedResponse eres;
     
            // collect information to encapsulate request
            RestRequest request = new RestRequest();
            request.url = this.url;
            request.controller = this.controller;
            request.method = this.method;
            request.header = book.headers;

            if (data != null)
            {
                request.json_data = data.ToString();
            }

            try // parse the JSON schema / the expected return format
            {
                // parse
                var xresult = xdoc.Root.Element("schema");
                string code = Tools.Attr(xresult, "code");
               // HttpCode = Convert.ToInt32(code);
                // parse the http code to int
                HttpCode = Int32.Parse(code);
                ContentType = Tools.Attr(xresult, "type");
                schema = JSchema.Parse(xresult.Value);
                eres = new ExpectedResponse(code, ContentType, schema);
                // update the expected response member in RestRequest class
                request.response = eres;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            

            // load the requests
            LoadXML.requests.Add(request);
         
        }

    }
}
