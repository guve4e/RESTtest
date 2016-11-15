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
        private LoadXML book;

        private int HttpCode;
        private string ContentType;
        JSchema schema;


        public IList<ValidationError> schemaErrors;
        private XElement xrequest;
        private XElement xresponse;
       // private RestResponse theResponse = null;

        public TestCase(LoadXML parent, string fname)
        {
            book = parent;
            try
            {
                xdoc = XDocument.Load(fname);
                var xresult = xdoc.Root.Element("result");
                string codes = parent.Attribute(xresult, "code");
                HttpCode = Convert.ToInt32(codes);
                ContentType = parent.Attribute(xresult, "type");
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
            bool success = false;

            Debug.WriteLine("ID " + tid);
            Debug.WriteLine("TYPE " + rtype);
            Debug.WriteLine("XDOC " + xdoc.Root);

            switch (rtype)
            {
                //case "GET": success = executeRestGet(tid, rtype, xdoc.Root); break;
                //case "POST": success = executeRestPost(tid, rtype, xdoc.Root); break;
                //case "PUT": success = executeRestPost(tid, rtype, xdoc.Root); break;
                //case "DELETE": success = executeRestGet(tid, rtype, xdoc.Root); break;
                //default: Terminal.WriteError("Unsupported test type: " + rtype); break;
            }

            if (!success)
            {
               
            }
        }
    }
}
