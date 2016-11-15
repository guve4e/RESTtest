using Newtonsoft.Json.Linq;
using RESTtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RESTtest.Library
{
    class LoadXML
    {
        /// <summary>
        /// File Path
        /// </summary>
        public string  file { get; set; }

        /// <summary>
        /// XmlDoc
        /// </summary>
        public XmlDocument doc { get; set; }

        /// <summary>
        /// XML String
        /// </summary>
        public string xml { get; set; }
        private XDocument xbook;
        public XDocument xenv;
        public XElement xreport;
        public string Environment;

        public string url;

        internal JObject envariables;

        internal Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file"></param>
        public LoadXML(string file)
        {
            this.file = file;
            xenv = XDocument.Load(file);

        }

        /// <summary>
        /// Gets the attribute of an XML element
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Attribute(XElement parent, XName name)
        {
            if (parent == null) return "";
            var atr = parent.Attribute(name);
            if (atr == null) return "";
            return atr.Value;
        }

        /// <summary>
        /// Loads and Parses XML file
        /// </summary>
        public void Load()
        {
            try
            {    
                // get environmental variables 
                envariables = new JObject();



                url =  Attribute(xenv.Root, "base");


                //     Debug.WriteLine("URL->" + x);

                foreach (XElement xvar in xenv.Root.Element("variables").Elements())
                {
                    envariables.Add(new JProperty(Attribute(xvar, "id"),Attribute(xvar, "value")));
                }
                // get headers
                foreach (XElement xhead in xenv.Root.Element("header-all").Elements())
                {
                    headers.Add(Attribute(xhead, "id"), Attribute(xhead, "value"));
                }

                foreach (XElement xsq in xenv.Root.Elements("sequence"))
                {
                    string id = Attribute(xsq, "id");
     
                    foreach (XElement xtest in xsq.Elements("test"))
                    {
                        string tid = xtest.Attribute("src").Value;
                        string fname = xtest.Attribute("src").Value + ".xml";
                        if (!File.Exists(fname))
                        {
                            MessageBox.Show("File not found: " + fname);
                            return;
                        }

                        var tc = new TestCase(this,fname);
                   
                        tc.Execute(fname);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
    }// end class
}// end namespace 
