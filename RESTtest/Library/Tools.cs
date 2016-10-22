using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
