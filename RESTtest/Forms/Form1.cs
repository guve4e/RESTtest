using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTtest
{
    public partial class Form1 : Form
    {
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


            try
            {
                string method = comboBox1.SelectedItem.ToString();
                string contentType = comboBox1.SelectedItem.ToString();

                MessageBox.Show("Method->" + method + "/n" + "ContentTypee-> " + contentType);
            } 
            catch(NullReferenceException ex)
            {
                MessageBox.Show("There is an empty field /n Please enter valid values");
                MessageBox.Show("Exception in Form 1:" + ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception in Form 1:" + ex.Message);
            }
           
        }
    }
}
