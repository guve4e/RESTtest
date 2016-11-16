using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using RESTtest.Models;

namespace RESTtest.Library
{
    /// <summary>
    /// This class sends http requests
    /// </summary>
    class Rest
    {
        public string accept { get; set; }
        public string method { get; set; }
        public string url { get; set; }
        public string contentType { get; set; }

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
            string s = null;
            RestResponse res = new RestResponse();

            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)request.GetResponse();
                // get the response in a stream and contain it in a string
                using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                {

                    var start = DateTime.Now;
                    res.Duration = DateTime.Now - start;

                    if (objResponse is HttpWebResponse) res.UpdateFrom(objResponse as HttpWebResponse);
                    var hresponse = (HttpWebResponse)objResponse;

                  //  logger.Debug(hresponse.StatusDescription);
                    res.Success = true;

                    responseStream.Close(); // close the stream
                }
            }
            catch (WebException we) // handle some exceptions
            {
                //if (we.Response is HttpWebResponse) res.UpdateFrom(we.Response as HttpWebResponse);
                //res.Message = we.Message;
                //res.Success = false;

                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                MessageBox.Show(
                    "Exception in Rest Request ->" + we.Message + "Response -> " +
                    Convert.ToString(response));
            }

            return res;
        }// end

        /// <summary>
        /// Makes a Request to the API POST
        /// </summary>
        /// <param name="json"></param>
        public System.String RestPost(string json)
        {
            System.String result = null;
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                MessageBox.Show(
                   "Exception in RestPost ->" + we.Message + "Response -> " +
                   Convert.ToString(response));
            }

            return result;
        }

    

    }
}
