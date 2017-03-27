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
    /// It contains TestCase object
    /// 
    /// </summary>
    class LoadXML
    {
        /// <summary>
        /// File Path
        /// 
        /// </summary>
        public string  file { get; set; }

        /// <summary>
        /// XML object loaded from XML file
        /// 
        /// </summary>
        public XDocument xenv;

        /// <summary>
        /// The base of the URL
        /// 
        /// </summary>
        public string url;

        /// <summary>
        /// Environmental Variables
        /// 
        /// </summary>
        internal JObject envariables;

        /// <summary>
        /// Headers 
        /// 
        /// </summary>
        internal Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// List to encapsulate all requests collected makeObjetForm the XML files
        /// 
        /// This list will be populated from TestCase class
        /// after the class reads the test cases from XML file
        /// and encapsulate them into requests objects
        /// 
        /// </summary>
        public static List<RestRequest> requests;


        /// <summary>
        /// Constructor
        /// Loads the XML Object 
        ///
        /// </summary>
        /// <param name="file"></param>
        public LoadXML(string file)
        {
            // 
            requests = new List<RestRequest>();

            this.file = file;
            xenv = XDocument.Load(file);

        }

        /// <summary>
        /// Loads and Parses XML file
        /// Updates requests list
        /// 
        /// </summary>
        public void Load()
        {
            try
            {
                // get environmental variables 
                envariables = new JObject();
                // get URL
                url = Tools.Attr(xenv.Root, "base");

                // get Environmental Variables
                foreach (XElement xvar in xenv.Root.Element("variables").Elements())
                {
                    envariables.Add(new JProperty(Tools.Attr(xvar, "id"), Tools.Attr(xvar, "value")));
                }
                
                // get headers
                foreach (XElement xhead in xenv.Root.Element("headers").Elements())
                {
                    headers.Add(Tools.Attr(xhead, "id"), Tools.Attr(xhead, "value"));
                }

                // get test cases
                foreach (XElement xsq in xenv.Root.Elements("test-cases"))
                {
                    string id = Tools.Attr(xsq, "id"); // get id

                    foreach (XElement xtest in xsq.Elements("test"))
                    {
                        string tid = xtest.Attribute("src").Value;
                        string f = xtest.Attribute("src").Value + ".xml";
                        // TestCases is the name of the folder where the Test Cases suppose to be
                        string fname = "TestCases/" + f; 
                        if (!File.Exists(fname))
                        {
                            MessageBox.Show("File not found: " + fname);
                            return;
                        }
                        // make Test Case object
                        var tc = new TestCase(this, fname);
                        // call Execute in TestCase class to parse the cases XML
                        tc.Execute(fname);
                    }
                }

                // Send it to Send Form
                Send s = new Send(requests, "automatic");
                s.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong XML format! " + ex.Message);
            }

        }// end Load
       
    }// end class
}// end namespace 
