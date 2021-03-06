﻿using System;
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
    public partial class MainForm : Form
    {
        /// <summary>
        /// Holds JSON Data
        /// Equivalent to data dictionary
        /// but in different makeObjetForm.
        /// 
        /// </summary>
        public static string json_data { get; set; }

        /// <summary>
        /// List of requests
        /// Coming from Database
        /// 
        /// </summary>
        public static List<RestRequest> requests { get; set; }

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
        public static string method { get; set; }

        /// <summary>
        /// Content type
        /// 
        /// </summary>
        public static string type { get; set; }

        public static string baseUrl { get; set; }

        public static string controller { get; set; }
    
        public static string url { get; set; }


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


        public Make_Object makeObjetForm;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {

            // TODO is static now, take it from box
            type = "application/json";

            this.showMakeObject = true;

            // allocate memory for Make_Objects form
            makeObjetForm = new Make_Object("data");

            // initialize  
            InitializeComponent();

            // make the makeObjetForm not re-sizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // initialize comboBox1
            this.comboBox1.Items.Add("GET");
            this.comboBox1.Items.Add("POST");
            this.comboBox1.Items.Add("PUT");
            this.comboBox1.Items.Add("DELETE");
            // initialize comboBox2
            this.comboBox2.Items.Add("application/json");
            this.comboBox2.Items.Add("application/x-www-makeObjetForm-urlencoded");

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
                baseUrl = this.textBox1.Text.ToString();
                controller = this.textBox2.Text.ToString();
                // construct baseUrl
                url = baseUrl + "/" + controller;

                // send packed information to Send makeObjetForm
                Send s =  Send.GetInstance("manual");
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
                        makeObjetForm.Text = "Make JSON String";
                        makeObjetForm.Show();
                    }
                    // check if data is null!!!
                    method = "POST";
                    this.showMakeObject = true;

                    break;
                case "GET":
                    method = "GET";
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
                    type = "application/json";
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
            if (requests != null)
            {
                requests.Clear();
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
        /// It retrieves the last request form DB
        /// and populates the fields of the MainForm.
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
          
            try
            {

                // get the last updated row from the table
                UpdateRequest db = new UpdateRequest();
                List<RestRequest> requests = db.GetRequests(1);
                RestRequest request = requests.ElementAt(0);


                // fill in the fields 

                data.Clear();
                data = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.json_data);
                headers = request.header;
                this.textBox1.Text = request.url;
                this.textBox2.Text = request.controller;

                // if database is empty
                if (!(request.url == ""))
                {
                    this.showMakeObject = false;
                }

                this.comboBox1.Text = request.method;
                this.comboBox2.Text = request.type;
            }
            catch(NullReferenceException ex)
            {
                Debug.WriteLine("************** Exception In OnMainFormLoad ******************");
                Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** Exception In OnMainFormLoad ******************");
                Debug.WriteLine(ex);
            }
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
            List<RestRequest> r =  db.GetRequests();

            History h = History.GetInstance(r,this);
            h.Show();
        }

        private void jsonDataButton_Click(object sender, EventArgs e)
        {
            makeObjetForm.Text = "Make JSON String";
            makeObjetForm.Show();
        }
    }
}
