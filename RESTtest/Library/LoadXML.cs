using Newtonsoft.Json.Linq;
using RESTtest.Forms;
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
    /// <summary>
    /// Load Information from XML
    /// Makes Lists of requests to be
    /// send to Sent Form for Processing
    /// </summary>
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

        public XDocument xenv;

        public string url;

        internal JObject envariables;

        internal Dictionary<string, string> headers = new Dictionary<string, string>();

        public static List<RestRequest> requests = new List<RestRequest>();


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
        /// Updates requests list
        /// </summary>
        public void Load()
        {
            try
            {
                // get environmental variables 
                envariables = new JObject();
                // get Url
                url = Attribute(xenv.Root, "base");
                // get Environmental Variables
                foreach (XElement xvar in xenv.Root.Element("variables").Elements())
                {
                    envariables.Add(new JProperty(Attribute(xvar, "id"), Attribute(xvar, "value")));
                }
                // get headers
                foreach (XElement xhead in xenv.Root.Element("header-all").Elements())
                {
                    headers.Add(Attribute(xhead, "id"), Attribute(xhead, "value"));
                }
                // get Sequences
                foreach (XElement xsq in xenv.Root.Elements("sequence"))
                {
                    string id = Attribute(xsq, "id");

                    foreach (XElement xtest in xsq.Elements("test"))
                    {
                        string tid = xtest.Attribute("src").Value;
                        string f = xtest.Attribute("src").Value + ".xml";
                        string fname = "TestCases/" + f; 
                        if (!File.Exists(fname))
                        {
                            MessageBox.Show("File not found: " + fname);
                            return;
                        }
                        // make Test Case object
                        var tc = new TestCase(this, fname);
                        // execute
                        tc.Execute(fname);
                    }
                }

                // Send it to Send Form
                Send s = new Send(requests, "automatic");
                s.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }// end Load
       
    }// end class
}// end namespace 
