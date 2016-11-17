#region Source code license
/* RESTfull API Automated Testing tool
 * Source:    https://github.com/skch/RESTA
 * Author:    skch@usa.net
This is a free software (MIT license) */
#endregion
using Newtonsoft.Json.Linq;
using RESTtest.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RESTtest.Models
{
    /// <summary>
    /// Describes Web Response from the API
    /// </summary>
	public class RestResponse
	{

        // members
        public string Method = "";
		public bool Success = false;
		public string ContentType = "";
		public string RawData = "";
		public string CleanData = "";
		public int HttpCode = 0;
		public string Message = "";
		public string RedirectUrl = "";

		public JObject jdata;
		public JArray jlist;
		public string Code;
		public TimeSpan Duration;

        /// <summary>
        /// Rest class sends an HttpWebRespose to this method.
        /// The method updates the fields of the class
        /// </summary>
        /// <param name="response"></param>
		public void UpdateFrom(HttpWebResponse response)
		{

			HttpCode = (int)response.StatusCode;
			ContentType = response.ContentType;

			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			RawData = reader.ReadToEnd();
			reader.Close();
			dataStream.Close();
			response.Close();
		}


		public void ParseResponse()
		{
			Message = "";
			jdata = null;
			jlist = null;
			Code = "";

			if (RawData.Trim().StartsWith("<"))
			{
                MessageBox.Show("Expecting JSON, but getting XML or HTML instead");
				if (!parseAsXml()) parseAsHtml();
				return;
			}

			if (Success) parseSuccess(); else parseError();


		}

		private void parseSuccess()
		{
			if (String.IsNullOrWhiteSpace(RawData))
			{
                MessageBox.Show("A successfull call returns empty result");
				Success = false;
				return;
			}
			if (RawData == "null")
			{
                MessageBox.Show("A successfull call returns JSON null result");
				Success = false;
				return;
			}
            MessageBox.Show(RawData);

			var jresponse = JObject.Parse(RawData);
			JToken jd = jresponse;
			Code = Tools.Attr(jresponse, "Code");
			if (!String.IsNullOrEmpty(Code))
			{
				Message = Tools.Attr(jresponse, "Message");
				RedirectUrl = Tools.Attr(jresponse, "RedirectUrl");
				jd = jresponse.GetValue("Data");
				if (!jresponse.TryGetValue("Data", out jd))
				{
                    MessageBox.Show("The expected element 'Data' was not found in the response");
					Success = false;
					return;
				}
			}
			else
			{
                MessageBox.Show("The JSON response is not in standard form");
			}
			if (jd is JArray) jlist = jd as JArray;
			if (jd is JObject) jdata = jd as JObject;
			Success = (Code == "200") || (Code == "1000");
		}


		private void parseError()
		{
            MessageBox.Show("Parsing the following error:\n{0}", RawData);
		}

		private bool parseAsXml()
		{
			try
			{
				var res = XElement.Parse(RawData);
                MessageBox.Show("Parsed result as XML: {0}", res.Name.LocalName);
                MessageBox.Show(RawData);
				return true;

			}
			catch
			{
				return false;
			}
		}

		private void parseAsHtml()
		{
			if (RawData.Contains("<i>Runtime Error</i>"))
			{
                MessageBox.Show("Run-time error on REST server (HTTP)");
				return;
			}

			if (RawData.Contains("<i>The resource cannot be found.</i>"))
			{
                MessageBox.Show("Invalid resource URL (HTML)");
				return;
			}

            MessageBox.Show("Unknown reponse in HTML format:\n{0}", RawData);
		}

		public XElement AsXml()
		{
			var xres = new XElement("response");
			xres.Add(new XAttribute("code", HttpCode));
			xres.Add(new XAttribute("type", ContentType));
			if (!String.IsNullOrEmpty(Message)) xres.Add(new XAttribute("message", Message));
			xres.Add(new XAttribute("duration", String.Format("{0}:{1}.{2}", Duration.Minutes, Duration.Seconds, Duration.Milliseconds)));
			if (!String.IsNullOrEmpty(RawData)) xres.Add(new XElement("data", RawData));
			return xres;
		}
	}
}
