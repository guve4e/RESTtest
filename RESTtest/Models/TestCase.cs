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
        JSchema schema;


        internal Dictionary<string, string> headers = new Dictionary<string, string>();

        public IList<ValidationError> schemaErrors;
        private XElement xrequest;
        private XElement xresponse;
       // private RestResponse theResponse = null;

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
                xdoc = XDocument.Load(fname);



                string controller = xdoc.Root.Attribute("url").Value;
                string method = xdoc.Root.Attribute("type").Value;

                Debug.WriteLine("Controller ---> " + controller);
                Debug.WriteLine("Method ---> " + method);




                var da = xdoc.Root.Element("data");
                JSchema s = JSchema.Parse(da.Value);
                Debug.WriteLine("S--->" + s.ToString());

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
            Debug.WriteLine("METHOD " + rtype);
        //    Debug.WriteLine("DATA " + data.ToString());
            Debug.WriteLine("XDOC " + xdoc.Root);


            switch (rtype)
            {
                //case "GET": success = executeRestGet(tid, rtype, xdoc.Root); break;
              
            }

            if (!success)
            {
               
            }
        }

        private bool executeRestGet(string tid, string rtype, XElement root)
        {
            Console.Write("Running test {0} >", tid);
            try
            {
               
               

            }
            catch (Exception ex)
            {
               
            }
            return true;
        }





    }
}
