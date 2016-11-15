using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RESTtest.Library
{
    class LoadXML
    {
        public string  file { get; set; }
        public XmlDocument doc { get; set; }
        public string xml { get; set; }

        public LoadXML(string file)
        {
            this.file = file;
        }

        public Dictionary<string,string> Load()
        {
            doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList nodeList = doc.SelectNodes("//*");

            var result = new Dictionary<string, string>();

            foreach (XmlNode node in nodeList )
            {
                result.Add(node.InnerText, node.InnerText);
            }
            return result;
        }

    }
}
