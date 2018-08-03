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
    public partial class userprompt : Form
    {
        private string user_input;
        private string deslabel;

        public string User_Input
        {
            get { return user_input; }
            
            set { user_input = value; }
        }

        public string DesLabel
        {
            get { return deslabel; }

            set { deslabel = value; }
        }

        public userprompt()
        {
            InitializeComponent();
        }

        private void userprompt_Load(object sender, EventArgs e)
        {
            //button_confirm.Enabled = false;
            label_prompt.Text = DesLabel;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            User_Input = "ok";
            this.DialogResult = DialogResult.OK;
            /* if (text_input.Text == "ok" || text_input.Text == "no")
             {
                 User_Input = text_input.Text;
                 this.DialogResult = DialogResult.OK;
             }

             else
                 MessageBox.Show("Please enter the correct value according to the description stated above.");*/
        }

        private void button_abort_Click(object sender, EventArgs e)
        {
            User_Input = "NOK";
            button_abort.DialogResult = DialogResult.Abort; 
        }

        /*  private void text_input_TextChanged(object sender, EventArgs e)
          {
              button_confirm.Enabled = true;
          }*/
    }
}
