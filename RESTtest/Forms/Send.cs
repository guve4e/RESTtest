using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTtest.Databse;
using RESTtest.Library;
using RESTtest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTtest.Forms
{
    public partial class Send : Form
    {
        ///// <summary>
        ///// Global
        ///// holds JSON Data
        ///// </summary>
        //public Dictionary<string, string> json = new Dictionary<string, string>();

        ///// <summary>
        ///// Global,
        ///// Headers
        ///// </summary>
        //public Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// Switch variable.
        /// The class is used differently with different value
        /// 
        /// </summary>
        public string sw { get; set; }

        ///// <summary>
        ///// URL
        ///// </summary>
        //public string  baseUrl { get; set; }

        ///// <summary>
        ///// Method 
        ///// Ex: GET,POST,DELETE,PUT
        ///// 
        ///// </summary>
        //public string  method { get; set; }

        ///// <summary>
        ///// Type
        ///// </summary>
        //public string type { get; set; }

        ///// <summary>
        ///// Data to be sent
        ///// </summary>
        //public string data { get; set; }

        ///// <summary>
        ///// URL the base of the API URL
        ///// </summary>
        //public string baseUrl { get; set; }

        ///// <summary>
        ///// Controller
        ///// </summary>
        //public string controller { get; set; }

        /// <summary>
        ///  Rest Response
        /// </summary>
        private RestResponse response;

        /// <summary>
        /// List of all requests
        /// </summary>
        public List<RestRequest> requests = new List<RestRequest>();

        /// <summary>
        /// Send Form Instance
        /// </summary>
        public static Send sendForm = null; 

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <param name="headers"></param>
        /// <param name="sw"></param>
        private Send(string sw = "manual")
        {
            // Initialize Components
            InitializeComponent();
            // make the makeObjetForm not re-sizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.sw = sw;
            
            // Set Progress bar
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="requests">List of requests</param>
        /// <param name="sw">switch variable</param>
        private Send(List<RestRequest> requests, string sw = "automatic")
        {
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Continuous;
            this.requests = requests;
            this.sw = sw;
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static Send GetInstanceForLoad(List<RestRequest> requests, string sw = "automatic")
        {
            if (sendForm == null)
            {
                sendForm = new Send(requests, sw);
                sendForm.FormClosed += delegate { sendForm = null; };
            }
            return sendForm;
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static Send GetInstance(string sw = "manual")
        {
            if (sendForm == null)
            {
                sendForm = new Send(sw);
                sendForm.FormClosed += delegate { sendForm = null; };
            }
            return sendForm;
        }

        /// <summary>
        /// SEND button
        /// Switches sw variable
        /// It does different things
        /// corresponding to the sw variable
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Rest rest = null;
            UpdateRequest db = new UpdateRequest(); // Update Database

            if (sw == "manual") // manually filed fields
            {
                this.textBox1.Clear();

                switch (MainForm.method) // switch method
                {
                    case "GET":
                        progressBar1.Value = 30;
                        // RestCall
                        rest = new Rest("*/*", MainForm.method, MainForm.baseUrl, MainForm.type, MainForm.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestGet();
                        progressBar1.Value = 100;
                        var g = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Text += g;
                        // clear the data dictionary
                        //    MainForm.data.Clear();
                        break;
                    case "POST":
                        progressBar1.Value = 30;
                        // RestCall
                        rest = new Rest("*/*", MainForm.method, MainForm.baseUrl, MainForm.type, MainForm.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(JsonConvert.SerializeObject(MainForm.data));
                        progressBar1.Value = 100;
                        var p = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Text = p;
                        // clear the data dictionary
                    //    MainForm.data.Clear();
                        break;
                    case "PUT":
                        progressBar1.Value = 30;
                        // RestCall
                        rest = new Rest("*/*", MainForm.method, MainForm.baseUrl, MainForm.type, MainForm.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(JsonConvert.SerializeObject(MainForm.data));
                        progressBar1.Value = 100;
                        var put = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Text = put;
                        // clear the data dictionary
                      //  MainForm.data.Clear();
                        break;
                    case "DELETE":
                        progressBar1.Value = 30;
                        // RestCall
                        rest = new Rest("*/*", MainForm.method, MainForm.baseUrl, MainForm.type, MainForm.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(JsonConvert.SerializeObject(MainForm.data));
                        progressBar1.Value = 100;
                        var del = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Text = del;
                        // clear the data dictionary
                      //  MainForm.data.Clear();
                        //to do
                        break;

                }

                // update database with this request
                // update database works only if fields are field manually
                RestRequest r = new RestRequest();
                r.url = MainForm.url;
                r.method = MainForm.method;
                r.header = MainForm.headers;
                r.controller = MainForm.controller;
                string data = JsonConvert.SerializeObject(MainForm.data);
                r.json_data = data;
                r.type = MainForm.type;
                db.CreateRequest(r);
                requests.Clear(); // clear the requests
            }
            else if (sw == "automatic") // if loaded from XML
            {
                // TODO POST PUT DELETE use same format, combine them to reuse code

                // go trough each request and send it
                // then check and validate the JSON Schema 
                foreach(var l in requests)
                {
                    // combine URL + controller
                    string uri = l.url + "/" + l.controller; // combine 

                    // switch methods
                    if (l.method == "GET")
                    {
                        progressBar1.Value = 30;
                        // RestCall
                        rest = new Rest("*/*", l.method, uri, l.type, l.header);
                        progressBar1.Value = 70;
                        // make GET request
                        this.response = rest.RestGet();
                        progressBar1.Value = 100;
                        // make JSON from response
                        var g = JsonConvert.SerializeObject(this.response, Formatting.Indented);

                        try
                        {
                            // extract the schema from the requests list
                            string sc = l.response.schema.ToString();
                            // convert to JObject
                            JObject obj = JObject.Parse(g);
                            // List to collect the response messages
                            IList<string> message;
                            // validate JSON and update message
                            bool b = Tools.validateJson(sc, obj, out message);
                            // go over the messages and print them on the screen
                            foreach (var m in message)
                            {
                                MessageBox.Show("Valid -> " + b + " " + m);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        this.textBox1.Text += g;
                    }
                    else if (l.method == "POST")
                    {
                        progressBar1.Value = 30;
                        rest = new Rest("*/*", l.method, uri, MainForm.type, l.header);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(l.json_data);
                        progressBar1.Value = 100;
                        // make JSON from response
                        var p = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        try
                        {
                            // extract the schema from the requests list
                            string sc = l.response.schema.ToString();
                            // convert to JObject
                            JObject obj = JObject.Parse(p);
                            // List to collect the response messages
                            IList<string> message;
                            // validate JSON and update message
                            bool b = Tools.validateJson(sc, obj, out message);
                            // go over the messages and print them on the screen
                            foreach (var m in message)
                            {
                                MessageBox.Show("Valid -> " + b + " " + m);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.textBox1.Text += p;
                    }
                    else if (l.method == "PUT")
                    {
                        progressBar1.Value = 30;
                        rest = new Rest("*/*", l.method, uri, MainForm.type, l.header);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(l.json_data);
                        progressBar1.Value = 100;
                        // make JSON from response
                        var p = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        try
                        {
                            // extract the schema from the requests list
                            string sc = l.response.schema.ToString();
                            // convert to JObject
                            JObject obj = JObject.Parse(p);
                            // List to collect the response messages
                            IList<string> message;
                            // validate JSON and update message
                            bool b = Tools.validateJson(sc, obj, out message);
                            // go over the messages and print them on the screen
                            foreach (var m in message)
                            {
                                MessageBox.Show("Valid -> " + b + " " + m);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.textBox1.Text += p;
                    }
                    else if (l.method == "DELETE")
                    {
                        progressBar1.Value = 30;
                        rest = new Rest("*/*", l.method, uri, MainForm.type, l.header);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(l.json_data);
                        progressBar1.Value = 100;
                        // make JSON from response
                        var p = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        try
                        {
                            // extract the schema from the requests list
                            string sc = l.response.schema.ToString();
                            // convert to JObject
                            JObject obj = JObject.Parse(p);
                            // List to collect the response messages
                            IList<string> message;
                            // validate JSON and update message
                            bool b = Tools.validateJson(sc, obj, out message);
                            // go over the messages and print them on the screen
                            foreach (var m in message)
                            {
                                MessageBox.Show("Valid -> " + b + " " + m);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        this.textBox1.Text += p;
                    }

                    // update fields
                    this.textBox2.Text = uri;
                    this.textBox3.Text = l.method;
                    this.textBox4.Text = l.type;
                }
              //  requests.Clear(); // clear the requests
            }     
        }


        /// <summary>
        /// This Method is called 
        /// when the Form is first loaded and 
        /// when the Refresh button is pressed.
        /// It reads its properties and two static
        /// dictionaries in MainForm containing 
        /// json data and headers of the request.
        /// </summary>
        private void OnLoad()
        {
            // If Manually entered data in the fields
            if (sw == "manual")
            {
                this.textBox2.Text = MainForm.url;
                this.textBox3.Text = MainForm.method;
                this.textBox4.Text = MainForm.type;

                // If headers
                if (MainForm.headers.Count > 0)
                {
                    // print info in the text box
                    this.textBox1.Text += "Headers - " + "\r\n\r\n";
                    foreach (var v in MainForm.headers)
                    {
                        this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                    }
                }

                // If post and object is created
                if (MainForm.data != null && MainForm.data.Count > 0)
                {
                    this.textBox1.Text += "Object to send - " + "\r\n\r\n";
                    foreach (var v in MainForm.data)
                    {
                        this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                    }

                    this.textBox1.Text += "\r\n";
                    this.textBox1.Text += "JSON string: " + "\r\n";
                    string data = Tools.makeObject(MainForm.data);
                    this.textBox1.Text += data + "\r\n";
                }
            }
            else if (sw == "automatic") // If loaded from XML
            {
                try
                {
                    // Load the first request 
                    string url = requests[0].url + "/" + requests[0].controller;
                    this.textBox2.Text = url;
                    this.textBox3.Text = requests[0].method;
                    this.textBox4.Text = requests[0].type;
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show("The format of XML is wrong!" + ex.Message);
                }
            }
        }

        /// <summary>
        /// On Load Wrapper
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Load(object sender, EventArgs e)
        {
            this.OnLoad();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Simulates reload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.OnLoad();
        }
    }
}
