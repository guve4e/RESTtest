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

namespace RESTtest
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> fields = new Dictionary<string, string>();

        public string jsonSend { get; set; }
        public string method { get; set; }


        public Form1()
        {
            InitializeComponent();
            // initialize comboBox1
            this.comboBox1.Items.Add("GET");
            this.comboBox1.Items.Add("POST");
            this.comboBox1.Items.Add("PUT");
            // initialize comboBox2
            this.comboBox2.Items.Add("application/json");
            
        }

        /// <summary>
        /// Handles TEST button 
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {



            

            string url = this.textBox1.Text.ToString();
            string controller = this.textBox2.Text.ToString();
            string fullUrl = url + "/" + controller;


            // CHECK IF combobox is NULL



            Library.Rest rest = new Library.Rest("*/*", this.method, fullUrl, "application/json");

            string s = rest.RestPost(this.jsonSend);

            MessageBox.Show(s);

            ////  string s = rest.RestRequest();
            //  try
            //  {
            //      string method = comboBox1.SelectedItem.ToString();
            //      string contentType = comboBox1.SelectedItem.ToString();

            //      MessageBox.Show("Method->" + method + "/n" + "ContentTypee-> " + contentType);
            //  } 
            //  catch(NullReferenceException ex)
            //  {
            //      MessageBox.Show("There is an empty field /n Please enter valid values");
            //      MessageBox.Show("Exception in Form 1:" + ex.Message);
            //  }
            //  catch(Exception ex)
            //  {
            //      MessageBox.Show("Exception in Form 1:" + ex.Message);
            //  }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = (string)comboBox1.SelectedItem;

            switch (selectedItem)
            {
                case "POST":
                    Make_Object form = new Make_Object();
                    form.Show();
                    // check if fields is null!!!
                    this.method = "POST";
                    jsonSend = Tools.makeObject(fields);
                    break;
                case "GET":
                    this.method = "GET";
                    break;
            }



        }
    }
}
