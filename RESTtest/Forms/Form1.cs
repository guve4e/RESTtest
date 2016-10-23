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
        
        public string method { get; set; }
        public string contentType { get; set; }


        public Form1()
        {
            this.contentType = "application/json";
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

            if (this.textBox1 != null && this.textBox2 != null && 
                this.comboBox1.SelectedItem != null && this.comboBox2.SelectedItem != null)

            {

                string url = this.textBox1.Text.ToString();
                string controller = this.textBox2.Text.ToString();
                string fullUrl = url + "/" + controller;


                Send s = new Forms.Send(fullUrl, method, this.contentType);
                s.Show();
            }
            

            
            // CHECK IF combobox is NULL


            ////  string s = rest.RestGet();
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
                    break;
                case "GET":
                    this.method = "GET";
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
    }
}
