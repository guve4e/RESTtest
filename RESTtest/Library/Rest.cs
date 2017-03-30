using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using RESTtest.Models;
using System.Text;

namespace RESTtest.Library
{
    /// <summary>
    /// This class sends http requests
    /// </summary>
    class Rest
    {

        public string accept { get; set; }

        /// <summary>
        /// Method
        /// Ex: GET,POST,PUT,DELETE
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// Url
        /// Ex: http://example.com
        /// </summary>
        public string url { get; set; }
        
        /// <summary>
        /// Content type of the request
        /// Ex: application/json
        /// </summary>
        public string contentType { get; set; }

        /// <summary>
        /// Headers of the request
        /// </summary>
        public Dictionary<string, string> header = new Dictionary<string, string>();

        /// <summary>
        /// Request Object
        /// 
        /// </summary>
        WebRequest request;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        public Rest(string accept, string method, string url, string contentType, Dictionary<string,string> header)
        {
            this.accept = accept;
            this.method = method;
            this.url = url;
            this.contentType = contentType;
            this.header = header;

            try
            {
                request = WebRequest.Create(this.url);
                request.Method = this.method;
                request.ContentType =contentType;

                // go trough headers dictionary
                foreach (var v in header)
                {
                    string key = v.Key.ToString();
                    string value = v.Value.ToString();
                    // Add to Headers
                    if (key != "" && value != "")
                        request.Headers.Add(key,value);
                }
            }
            catch (WebException we)
            {
                MessageBox.Show("Exception in Rest Constructor " + we.Message);
            }
       

        }

        /// <summary>
        /// Makes a Request to the API GET
        /// </summary>
        /// <returns></returns>
        public RestResponse RestGet()
        {
            // encapsulate the response
            RestResponse res = new RestResponse();
            // start timer
            DateTime start = DateTime.Now; 

            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)request.GetResponse();
                res.Duration = DateTime.Now - start; // end timer

                // get the response in a stream and contain it in a string
                using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                {
                    res.Success = true; // if here no exception was thrown
                    // get the response
                    // update RestResposne object
                    if (objResponse is HttpWebResponse) res.UpdateFrom(objResponse as HttpWebResponse);
                    var hresponse = (HttpWebResponse)objResponse;

                    
                    res.Method = this.method;
                    responseStream.Close(); // close the stream
                }
            }
            catch (WebException we) // handle exceptions but no mater what update the RestResposne object
            {
               
                // update the RestResponse object
                if (we.Response is HttpWebResponse) res.UpdateFrom(we.Response as HttpWebResponse);
                res.Message = we.Message;
                res.Success = false;

                // Show to user
                var response = we.Response as HttpWebResponse;
                // update time
                res.Duration = DateTime.Now - start;
                // if no response from server message the user
                if (response == null) MessageBox.Show("No Response from server :( waited " + res.Duration + " seconds");
                else MessageBox.Show( "Exception in Rest Request ->" + we.Message + "Response -> " +
                    Convert.ToString(response));
            }
     
            return res;
        }// end

        /// <summary>
        /// Makes a Request to the API POST
        /// </summary>
        /// <param name="json"></param>
        public RestResponse RestPost(string json)
        {
            // encapsulate the response
            RestResponse res = new RestResponse();
            // set the content type

            // request.ContentType = "application/x-www-makeObjetForm-urlencoded";
            request.ContentType = "application/json";

            // start timer
            DateTime start = DateTime.Now;

            try
            {
                // get the request stream 
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                res.Success = true; // if here no exception was thrown

                var httpResponse = (HttpWebResponse)request.GetResponse();
                res.Duration = DateTime.Now - start; // end timer
                // get the response
                // update RestResposne object
                if (httpResponse is HttpWebResponse) res.UpdateFrom(httpResponse as HttpWebResponse);
                var hresponse = (HttpWebResponse)httpResponse;
                res.Method = this.method;
            }
            catch (WebException we)// handle exceptions but no mater what update the RestResposne object
            {
                // update the RestResponse object    
                if (we.Response is HttpWebResponse) res.UpdateFrom(we.Response as HttpWebResponse);
                res.Message = we.Message;
                res.Success = false;
                // Show to user
                var response = we.Response as HttpWebResponse;
                // update time
                res.Duration = DateTime.Now - start;
                // if no response from server message the user
                if (response == null) MessageBox.Show("No Response from server :( waited " + res.Duration + " seconds");
                else MessageBox.Show("Exception in Rest Request ->" + we.Message + "Response -> " +
                    Convert.ToString(response));
            }
           
            return res;
        }

    

    }
}
