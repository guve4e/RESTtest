using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTtest.Library
{
    class Rest
    {
        public string accept { get; set; }
        public string method { get; set; }
        public string url { get; set; }
        public string contentType { get; set; }

        WebRequest request;

        public Rest(string accept, string method, string url, string contentType)
        {
            this.accept = accept;
            this.method = method;
            this.url = url;
            this.contentType = contentType;

            try
            {
                request = WebRequest.Create(this.url);
                request.Method = this.method;
                request.ContentType =contentType;
                request.Headers.Add("Authorization", "Basic reallylongstring");
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
        public string RestRequest()
        {
            string s = null;
            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)request.GetResponse();
                // get the response in a stream and contain it in a string
                using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                {
                    s = responseStream.ReadToEnd();
                    responseStream.Close(); // close the stream
                }
            }
            catch (WebException we) // handle some exceptions
            {
                var response = we.Response as HttpWebResponse;
                if (response == null)
                    throw;
                MessageBox.Show(
                    "Exception in Rest Request ->" + we.Message + "Response -> " +
                    Convert.ToString(response));
            }

            return s;
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
