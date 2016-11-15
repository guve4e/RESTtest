using Newtonsoft.Json;
using RESTtest.Library;
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
        /// <summary>
        /// Global
        /// holds JSON Data
        /// </summary>
        public Dictionary<string, string> json = new Dictionary<string, string>();

        /// <summary>
        /// Global,
        /// Headers
        /// </summary>
        public Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// Switch variable.
        /// The class is used differently with different value
        /// 
        /// </summary>
        public string sw { get; set; }

        public string  url { get; set; }
        public string  method { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public string response { get; set; }

        public Send(string url, string method, string type, Dictionary<string, string> json, Dictionary<string, string> headers, string sw)
        {
            InitializeComponent();
            this.json = json;
            this.headers = headers;
            this.url = url;
            this.method = method;
            this.type = type;
            this.sw = sw;
  
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        /// <summary>
        /// SEND button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Rest rest = null;

            if (sw == "manual")
            {
                switch (this.method)
                {
                    case "GET":
                        progressBar1.Value = 30;
                        rest = new Rest("*/*", this.method, this.url, this.type, this.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestGet();
                        progressBar1.Value = 100;
                        var g = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Clear();
                        this.textBox1.Text = g;
                        // clear the data dictionary
                        Form1.data.Clear();
                        break;
                    case "POST":
                        progressBar1.Value = 30;
                        rest = new Rest("*/*", this.method, this.url, this.type, this.headers);
                        progressBar1.Value = 70;
                        this.response = rest.RestPost(this.data);
                        progressBar1.Value = 100;
                        var p = JsonConvert.SerializeObject(this.response, Formatting.Indented);
                        this.textBox1.Clear();
                        this.textBox1.Text = p;
                        // clear the data dictionary
                        Form1.data.Clear();
                        break;
                    case "PUT":
                        break;
                    case "DELETE":
                        break;

                }
            }
            else if (sw == "automatic")
            {

            }     
        }

        /// <summary>
        /// On Load
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Load(object sender, EventArgs e)
        {
            if (sw == "manual")
            {
                this.textBox2.Text = this.url;
                this.textBox3.Text = this.method;
                this.textBox4.Text = this.type;


                // If headers
                if (Form1.headers.Count > 0)
                {
                    this.textBox1.Text += "Headers - " + "\r\n\r\n";
                    foreach (var v in Form1.headers)
                    {
                        this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                    }
                }

                // If post and object is created
                if (Form1.data.Count > 0)
                {
                    this.textBox1.Text += "Object to send - " + "\r\n\r\n";
                    foreach (var v in Form1.data)
                    {
                        this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                    }

                    this.textBox1.Text += "\r\n";
                    this.textBox1.Text += "JSON string: " + "\r\n";
                    this.data = Tools.makeObject(Form1.data);
                    this.textBox1.Text += data + "\r\n";
                }
            }
            else if (sw == "automatic")
            {

            }            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
