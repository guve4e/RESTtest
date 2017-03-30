using Newtonsoft.Json;
using RESTtest.Databse;
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
        /// <summary>
        /// History Instance 
        /// </summary>
        private static History historyForm = null;

        public List<RestRequest> requests { get; set; }

        // Create an instance of the ListBox.
        ListBox listBox;

        // reference to the MainForm
        private MainForm mform;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="m"></param>
        private History(List<RestRequest> requests, MainForm m)
        {
            InitializeComponent();
            // set DrawMode

            // get pointer to the MainForm
            this.mform = m;

            // set requests
            this.requests = requests;
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static History GetInstance(List<RestRequest> requests, MainForm m)
        {
            if (historyForm == null)
            {
                historyForm = new History(requests, m);
                historyForm.FormClosed += delegate { historyForm = null; };
            }
            return historyForm;
        }

        /// <summary>
        /// Wrapper over History_Load
        /// </summary>
        private void OnLoad()
        {
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

            // Add the ListBox to the makeObjetForm.
            this.Controls.Add(listBox);
            // Set the ListBox to display items in multiple columns.
            listBox.MultiColumn = false;
            // Set the selection mode to multiple and extended.
            listBox.SelectionMode = SelectionMode.MultiExtended;

            // Shutdown the painting of the ListBox as items are added.
            listBox.BeginUpdate();

            OnLoad();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // extract the request
            var request = listBox.SelectedItem as RestRequest;

            // make sure that the send makeObjetForm
            // dictionary is clear because 
            // you will add to it to construct
            // request's json data
            Make_Object.dictionary.Clear();

            // convert the json string to key:value pair
            // the reason for doing this is that Send.cs uses dictionary
            // and the user may want to change it
            // update static dictionary in MakeObject
            Make_Object.dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.json_data);

            // set baseUrl
            this.mform.textBox1.Text = Convert.ToString(request.url);
            // set controller
            this.mform.textBox2.Text = Convert.ToString(request.controller);
            
            
            // set method
            string method = Convert.ToString(request.method);
            this.mform.comboBox1.Text = method;

            if (method == "POST") this.mform.makeObjetForm.Show();
            // TODO rest of the Methods

            // set content type
            this.mform.comboBox2.Text = Convert.ToString(request.type);

            // convert the json string to key:value pair
            // the reason for doing this is that Send.cs uses dictionary
            // and the user may want to change it
            // update static dictionary in MainForm
            MainForm.data = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.json_data);

            // make MainForm.headers to point to the
            // current request from the chosen from
            // history 
            MainForm.headers = request.header;

            // click button programatically 
            this.mform.testButton.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateRequest db = new UpdateRequest();

            // extract the request
            var request = listBox.SelectedItem as RestRequest;

            if (request != null)
            {
                db.DeleteRequest(request.id);
                this.requests = db.GetRequests();
                listBox.Items.Clear();
                OnLoad();
            }
        }
    }
}
