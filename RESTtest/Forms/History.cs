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
        
        public History(List<RestRequest> requests)
        {
            InitializeComponent();
            this.requests = requests;
        }

        private void History_Load(object sender, EventArgs e)
        {

        }
    }
}
