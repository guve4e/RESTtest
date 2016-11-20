using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RESTtest.Library
{
    class Tools
    {
        /// <summary>
        /// Creates dynamically object
        /// and returns a string representation of it
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string makeObject(Dictionary<string,string> fields) 
        {
            if (fields == null) throw new NullReferenceException("makeObjects");
            // make dynamic object
            dynamic exo = new System.Dynamic.ExpandoObject();
            // initialize it
            foreach (var field in fields)
            {
                ((IDictionary<String, Object>)exo).Add(field.Key, field.Value);
            }

            // output - from Json.Net NuGet package
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            // return string representation
            return s;
        }


        public static string Attr(XElement parent, XName name)
        {
            if (parent == null) return "";
            var atr = parent.Attribute(name);
            if (atr == null) return "";
            return atr.Value;
        }

        internal static string Attr(JObject jresponse, string v)
        {
            throw new NotImplementedException();
        }


        public static bool validateJson(string schema, JObject json, out IList<string> message)
        {
            // locals
            IList<string> m;
            bool valid = false;
     
            // parse
            JSchema s = JSchema.Parse(schema);
            // validate
            valid = json.IsValid(s, out m);
            // update messages
            message = m;
            // return
            return valid;                
        }

    }
}
