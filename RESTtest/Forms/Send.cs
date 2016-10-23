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
        public string  url { get; set; }

        public string  method { get; set; }

        public string type { get; set; }
        public string data { get; set; }
        public string response { get; set; }

        public Send(string url, string method, string type)
        {
            InitializeComponent();
            this.url = url;
            this.method = method;
            this.type = type;
  
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

            switch (this.method)
            {
                case "GET":
                    progressBar1.Value = 30;
                    rest = new Rest("*/*", this.method, this.url, this.type);
                    progressBar1.Value = 70;
                    this.response = rest.RestGet();
                    progressBar1.Value = 100;
                    this.textBox1.Clear();
                    this.textBox1.Text = this.response;
                    // clear the fields dictionary
                    Form1.fields.Clear();
                    break;
                case "POST":
                    progressBar1.Value = 30;
                    rest = new Rest("*/*", this.method, this.url,this.type);
                    progressBar1.Value = 70;
                    this.response = rest.RestPost(this.data);
                    progressBar1.Value = 100;
                    this.textBox1.Clear();
                    this.textBox1.Text = this.response;
                    // clear the fields dictionary
                    Form1.fields.Clear();
                    break;
                case "PUT":
                    break;
                case "DELETE":
                    break;

            }
        }

        private void Send_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "URL - " + this.url + "\r\n\r\n";
            this.textBox1.Text += "METHOD - " + this.method + "\r\n\r\n";
            this.textBox1.Text += "CONTENT TYPE - " + this.method + "\r\n\r\n";

            // If post and object is created
            if (Form1.fields.Count > 0)
            {
                this.textBox1.Text += "Object to send - " + "\r\n\r\n";
                foreach(var v in Form1.fields)
                {
                    this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                }

                this.textBox1.Text += "\r\n";
                this.textBox1.Text += "JSON string: " + "\r\n";
                this.data = Tools.makeObject(Form1.fields);
                this.textBox1.Text += data + "\r\n";
            }
        }
    }
}
