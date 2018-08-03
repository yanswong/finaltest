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
    public partial class cal_leak_tempfactor : Form
    {
        private static string user_input;
        private static string decimal_value;
        private static string tempfactor;
        private static string sign;

        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }

        public string User_input
        {
            get { return user_input; }
            set { user_input = value; }
        }

        public string Decimal_value
        {
            get { return decimal_value; }
            set { decimal_value = value; }
        }

        public string Tempfactor
        {
            get { return tempfactor; }
            set { tempfactor = value; }
        }

        public cal_leak_tempfactor()
        {
            InitializeComponent();
        }

        private void cal_leak_tempfactor_Load(object sender, EventArgs e)
        {
            button_confirm.Enabled = false;
            button_abort.DialogResult = DialogResult.Abort;

            combo_sign.Items.Add("+");
            combo_sign.Items.Add("-");
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(num_input.Text) >= 0 && Convert.ToInt32(num_input.Text) <= 4)
            {
                Sign = Convert.ToString(combo_sign.SelectedItem);
                User_input = num_input.Text;
                Decimal_value = decimal_input.Text;
                Tempfactor = Sign + User_input + "." + Decimal_value;
                this.DialogResult = DialogResult.OK;
            }

            else
            {
                MessageBox.Show("Please enter the correct temperature factor.");
            }
        }

        private void num_input_TextChanged(object sender, EventArgs e)
        {
            if(num_input.Text.Length == 1)
            {
                decimal_input.Focus();
            }

            if(num_input.Text == string.Empty)
            {
                num_input.Text = "X";
                num_input.Focus();
            }
        }

        private void decimal_input_TextChanged(object sender, EventArgs e)
        {
            button_confirm.Enabled = true;

            if (decimal_input.Text == string.Empty)
            {
                decimal_input.Text = "X";
                decimal_input.Focus();
            }
        }
    }
}
