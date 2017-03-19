using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RESTtest.Library;
using RESTtest.Forms;
using System.Diagnostics;
using System.Xml.Linq;
using RESTtest.Databse;
using RESTtest.Models;

namespace RESTtest
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Holds JSON Data
        /// Equivalent to data dictionary
        /// but in different form.
        /// 
        /// </summary>
        public string json_data { get; set; }

        /// <summary>
        /// List of requests
        /// Coming from Database
        /// 
        /// </summary>
        public List<RestRequest> requests { get; set; }

        /// <summary>
        /// Global Static
        /// holds JSON Data
        /// This dictionary will be send to
        /// Send Form
        /// 
        /// </summary>
        public static Dictionary<string, string> data = new Dictionary<string, string>();

        /// <summary>
        /// Global, Static
        /// Headers
        /// 
        /// </summary>
        public static Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// Method
        /// 
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// Content type
        /// 
        /// </summary>
        public string contentType { get; set; }
        
        /// <summary>
        /// RestRequest Object
        /// 
        /// </summary>
        public RestRequest rest { get; set; }

        /// <summary>
        /// Switch Variable
        /// If true it shows MakeObject Form
        /// 
        /// </summary>
        public bool showMakeObject { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            this.contentType = "application/json";
            this.showMakeObject = true;

            // initialize  
            InitializeComponent();

            // initialize comboBox1
            this.comboBox1.Items.Add("GET");
            this.comboBox1.Items.Add("POST");
            this.comboBox1.Items.Add("PUT");
            this.comboBox1.Items.Add("DELETE");
            // initialize comboBox2
            this.comboBox2.Items.Add("application/json");
            this.comboBox2.Items.Add("application/x-www-form-urlencoded");

        }

        /// <summary>
        /// Handles TEST button 
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // check if all fields are field
            if (this.textBox1 != null && this.textBox2 != null && 
                this.comboBox1.SelectedItem != null && this.comboBox2.SelectedItem != null)
            {
                // extract info from textBoxes
                string url = this.textBox1.Text.ToString();
                string controller = this.textBox2.Text.ToString();
                // construct url
                string fullUrl = url + "/" + controller;

                // send packed information to Send form
                Send s = new Forms.Send(fullUrl, this.method, this.contentType,data,headers,"manual");
                s.baseUrl = url; // update URL
                s.controller = controller; // update controller 
                s.Show();
            }
            else
            {
                MessageBox.Show("Enter Valid information in the data");
            }
        }

        /// <summary>
        /// Combo Box 1
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // holds the selected item
            string selectedItem = (string)comboBox1.SelectedItem;
            // switch the selectedItem
            switch (selectedItem)
            {
                case "POST":
                    if (this.showMakeObject)
                    {
                        Make_Object form = new Make_Object("data");
                        form.Text = "Make JSON String";
                        form.Show();
                    }
                    // check if data is null!!!
                    this.method = "POST";
                    this.showMakeObject = true;

                    break;
                case "GET":
                    this.method = "GET";
                    this.showMakeObject = true;
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = (string)comboBox2.SelectedItem;

            switch (selectedItem)
            {
                case "application/json":
                    this.contentType = "application/json";
                break;
                    // add more
            }

        }

        /// <summary>
        /// Load Environment XML 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // clear the request, if any
            if (this.requests != null)
            {
                this.requests.Clear();
            }
             
            // Open and Load file
            OpenFileDialog fd = new OpenFileDialog();
            if ( fd.ShowDialog() == DialogResult.OK)
            {
                LoadXML l = new LoadXML(fd.FileName);
                l.Load();
            }
        }

        /// <summary>
        /// Make Headers Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Make_Object form = new Make_Object("headers");
            form.Text = "Make Headers";
            form.Show();
        }

        /// <summary>
        /// On Load
        /// It executes first
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateRequest db = new UpdateRequest();
            rest = db.GetLastUpdatedRowIdRequest();
            //json_data = JsonConvert.SerializeObject(rest.json_data, Formatting.Indented);
            data.Clear();
            data = JsonConvert.DeserializeObject<Dictionary<string, string>>(rest.json_data);
            headers = rest.header;
            this.textBox1.Text = rest.url;
            this.textBox2.Text = rest.controller;

            // if database is empty
            if (!(rest.url == ""))
            {
                this.showMakeObject = false;
            }
            
            this.comboBox1.Text = rest.method;
        }

        /// <summary>
        /// History click
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateRequest db = new UpdateRequest();

            History h = new History(requests);
            h.Show();
        }
    }
}
