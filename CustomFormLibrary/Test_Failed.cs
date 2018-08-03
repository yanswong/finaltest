using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomFormLibrary
{
    public partial class Test_Failed : Form
    {
        private string testpoint;
        private string message_exception;
        private string testgroup;

        public string Testpoint
        {
            get { return testpoint; }
            set { testpoint = value; }
        }

        public string Message_Exception
        {
            get { return message_exception; }
            set { message_exception = value; }
        }

        public string Testgroup
        {
            get { return testgroup; }
            set { testgroup = value; }
        }

        public Test_Failed()
        {
            InitializeComponent();
        }

        private void Test_Failed_Load(object sender, EventArgs e)
        {
            button_abort.DialogResult = DialogResult.Abort;

            test_label.Text = Testgroup;
            label_testpoint.Text = "Test Point: " + Testpoint;
            label_error.Text = "Error :  " + Message_Exception; 
        }

        private void button_abort_Click(object sender, EventArgs e)
        {
            DialogResult confirmation =  MessageBox.Show("Are you sure?", "WARNING", MessageBoxButtons.YesNo);

            if (confirmation == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Abort;
            }

            else if(confirmation == DialogResult.No)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
