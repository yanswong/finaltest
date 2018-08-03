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
    public partial class cal_leak_prompt : Form
    {
        private static string user_input;
        private static string exp_input;
        private static string stdleak_value;

        public string User_input                                //Real number
        {
            get { return user_input; }

            set { user_input = value; }
        }

        public string Exp_input                                 //Exponent value
        {
            get { return exp_input; }

            set { exp_input = value; }
        }        

        public string Stdleak_value
        {
            get { return stdleak_value; }
            set { stdleak_value = value; }
        }

        public cal_leak_prompt()
        {
            InitializeComponent();
        }

        private void cal_leak_prompt_Load(object sender, EventArgs e)
        {
            button_confirm.Enabled = false;
            button_abort.DialogResult = DialogResult.Abort;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (Convert.ToSingle(num_input.Text) > 1.1 && Convert.ToSingle(num_input.Text) < 1.9)
            {
                User_input = num_input.Text;

                if (exp_value.Text.Length < 2)
                {
                    Exp_input = "0" + exp_value.Text;
                }
                else
                    Exp_input = exp_value.Text;

                Stdleak_value = User_input + "E-" + Exp_input;
                this.DialogResult = DialogResult.OK;        
            }
            else
                MessageBox.Show("Please enter the correct stdleak rate value.", "WARNING");
        }

        private void num_input_TextChanged(object sender, EventArgs e)
        {
            if(num_input.Text.Length == 3)
            {
                exp_value.Focus();
            }

            if(num_input.Text == string.Empty)
            {
                num_input.Text = "X.X";
                num_input.Focus();
            }
        }

        private void exp_value_TextChanged(object sender, EventArgs e)
        {
            button_confirm.Enabled = true;

            if(exp_value.Text == string.Empty)
            {
                exp_value.Text = "XX";
                exp_value.Focus();
            }
        }
    }
}
