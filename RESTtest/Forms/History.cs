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
    public partial class History : Form
    {
        public List<RestRequest> requests  { get; set; }

        // Create an instance of the ListBox.
        ListBox listBox;

        // reference to the MainForm
        private MainForm mform;

        public History(List<RestRequest> requests, MainForm m)
        {
            InitializeComponent();
            // set DrawMode

            // get pointer to the MainForm
            mform = m;

            // set requests
            this.requests = requests;
        }

        private void History_Load(object sender, EventArgs e)
        {

            // Create an instance of the ListBox.
            listBox = new ListBox();
            // Set the size and location of the ListBox.
            listBox.Size = new System.Drawing.Size(385, 450);
            listBox.Location = new System.Drawing.Point(0, 0);

            // Turn off the scrollbar.
            listBox.ScrollAlwaysVisible = false;

            // Set the border style to a single, flat border.
            listBox.BorderStyle = BorderStyle.FixedSingle;

            // Set the size and location of the ListBox.

            // Add the ListBox to the form.
            this.Controls.Add(listBox);
            // Set the ListBox to display items in multiple columns.
            listBox.MultiColumn = false;
            // Set the selection mode to multiple and extended.
            listBox.SelectionMode = SelectionMode.MultiExtended;

            // Shutdown the painting of the ListBox as items are added.
            listBox.BeginUpdate();
            // Loop through and add 50 items to the ListBox.

            

            foreach (var r in requests)
            {
                listBox.Items.Add(r);
            }


            // set the display and the value
            listBox.DisplayMember = "url";
           // listBox.ValueMember = "UserId";

            // Allow the ListBox to repaint and display the new items.
            listBox.EndUpdate();



            //TODO
            //// Display the second selected item in the ListBox to the console.
            //System.Diagnostics.Debug.WriteLine(listBox.SelectedItems[1].ToString());
            //// Display the index of the first selected item in the ListBox.
            //System.Diagnostics.Debug.WriteLine(listBox.SelectedIndices[0].ToString());

            listBox.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // extract the request
            var request = listBox.SelectedItem as RestRequest;
            // set url
            this.mform.textBox1.Text = Convert.ToString(request.url);
            // set controller
            this.mform.textBox2.Text = Convert.ToString(request.controller);
            // set method
            this.mform.comboBox1.Text = Convert.ToString(request.method);
            // set content type
            this.mform.comboBox2.Text = Convert.ToString(request.type);
        }
    
    }
}
