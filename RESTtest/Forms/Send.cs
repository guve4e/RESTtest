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
                    rest = new Rest("*/*", this.method, this.url, this.type,Form1.headers);
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
                    rest = new Rest("*/*", this.method, this.url,this.type,Form1.headers);
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

        private void Send_Load(object sender, EventArgs e)
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
                foreach(var v in Form1.data)
                {
                    this.textBox1.Text += v.Key + " => " + v.Value + "\r\n";
                }

                this.textBox1.Text += "\r\n";
                this.textBox1.Text += "JSON string: " + "\r\n";
                this.data = Tools.makeObject(Form1.data);
                this.textBox1.Text += data + "\r\n";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
