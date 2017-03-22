using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTtest.Forms
{
    public partial class Make_Object : Form
    {   
        /// <summary>
        /// Switch Variable.
        /// MakeObjects can be called to make
        /// Headers and Data.
        /// sw = "data" or sw = "headers"
        /// </summary>       
        private string sw { set; get; }

        /// <summary>
        /// Initial value: -1
        /// </summary>
        private int key_count { set; get; }

        /// <summary>
        /// Initial value: -1
        /// </summary>
        private int value_count { set; get; }

        /// <summary>
        /// Dictionary of Text-boxes
        /// To hold Key-Value pair
        /// 
        /// </summary>
        private Dictionary<TextBox,TextBox> dictionary = new Dictionary<TextBox,TextBox>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Make_Object(string data)
        {
            this.sw = data;
            key_count = -1;
            value_count = -1;

            // Initialize Components
            InitializeComponent();
            // make the form not re-sizable
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
   
        }

        private void showTextBoxes()
        {
            // Text-boxes to hold the key and the value
            TextBox textBoxKey = new System.Windows.Forms.TextBox();
            TextBox textBoxValue = new System.Windows.Forms.TextBox();
            // Labels
            Label label1 = new System.Windows.Forms.Label();
            Label label2 = new System.Windows.Forms.Label();

            // ???
            key_count++;
            value_count++;

            // 
            // textBox1
            // 
            textBoxKey.Location = new System.Drawing.Point(111, 35);
            textBoxKey.Name = "textBoxKey" + key_count;
            textBoxKey.Size = new System.Drawing.Size(112, 20);
            textBoxKey.TabIndex = 1;
            // 
            // textBox2
            // 
            textBoxValue.Location = new System.Drawing.Point(287, 35);
            textBoxValue.Name = "textBoxKey" + value_count;
            textBoxValue.Size = new System.Drawing.Size(112, 20);
            textBoxValue.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(77, 38);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(28, 13);
            label1.TabIndex = 3;
            label1.Text = "KEY";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(233, 39);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(48, 13);
            label2.TabIndex = 4;
            label2.Text = ": VALUE";

            // add to controls
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(textBoxKey);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(textBoxValue);
            this.Controls.Add(flowLayoutPanel1);

            // add text data to dictionary
            this.dictionary.Add(textBoxKey, textBoxValue);
        }

        /// <summary>
        /// Add Object Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Text-Boxes are first shown when the 
            // Form is Loaded
            // then OnClick of this button, showTextBoxes is 
            // invoked again
            showTextBoxes();
        }

        /// <summary>
        /// DONE button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();


            // CAREFULL HERE value_count may be -1;
            if (value_count > -1 && (dictionary.Count > 0))
            {
                // collect the values from the text fields and
                // insert them in the dictionary 
                 foreach (var v in dictionary)
                { 
                     string key = v.Key.Text.ToString();
                     string value = v.Value.Text.ToString();

                    try
                    {   // Make sure that the keys are unique
                        dict.Add(key, value);
                    }
                    catch (Exception ex) { MessageBox.Show("Exception: " + ex.Message); }
                 }
             
            }
            else
            {
                MessageBox.Show("Add Members First");
            }
        
            
            switch (this.sw) 
            {
                case "data": // if called to make JSON object
                    Form1.data = dict;
                    break;
                case "headers": // if called to make Headers
                    Form1.headers = dict;
                    break;
                default: // it shouldn't be here 
                    throw new Exception("Wrong Initialization of sw variable");

            }
            // close the form
            Close();          
        }

        /// <summary>
        /// OnLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Make_Object_Load(object sender, EventArgs e)
        {
            showTextBoxes();
        }
    }
}
