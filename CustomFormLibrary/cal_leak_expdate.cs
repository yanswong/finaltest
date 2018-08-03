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
    public partial class cal_leak_expdate : Form
    {
        private static string month;
        private static string day;
        private static string year;
        private static string expdate;

        public string Month                                     
        {
            get { return month; }
            set { month = value; }
        }

        public string Day                                       
        {
            get { return day; }
            set { day = value; }
        }
        
        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Expdate
        {
            get { return expdate; }
            set { expdate = value; }
        }

        public cal_leak_expdate()
        {
            InitializeComponent();
        }

        private void cal_leak_expdate_Load(object sender, EventArgs e)
        {
            btn_confirm.Enabled = false;
            btn_abort.DialogResult = DialogResult.Abort;
        }
        
        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(month_input.Text) > 0 && Convert.ToInt32(month_input.Text) <= 12 && Convert.ToInt32(day_input.Text) > 0 && Convert.ToInt32(day_input.Text) <= 31 && Convert.ToInt32(year_input.Text) > DateTime.Now.Year)
            {
                if (month_input.Text.Length == 1)
                {
                    Month = "0"+ month_input.Text;
                }
                else
                    Month = month_input.Text;

                if(day_input.Text.Length == 1)
                {
                    Day = "0" + day_input.Text;
                }
                else
                    Day = day_input.Text;

                Year = year_input.Text;
                Expdate = Month + " " + Day + " " + Year;
                this.DialogResult = DialogResult.OK;
            }

            else
                MessageBox.Show("Please enter the correct expiration date.");
        }

        private void month_input_TextChanged(object sender, EventArgs e)
        {
            if(month_input.Text.Length == 2)
            {
                day_input.Focus();
            }

            if(month_input.Text == string.Empty)
            {
                month_input.Text = "mm";
                month_input.Focus();
            }
        }

        private void day_input_TextChanged(object sender, EventArgs e)
        {
            if (day_input.Text.Length == 2)
            {
                year_input.Focus();
            }

            if (day_input.Text == string.Empty)
            {
                day_input.Text = "dd";
                day_input.Focus();
            }
        }

        private void year_input_TextChanged(object sender, EventArgs e)
        {
            btn_confirm.Enabled = true;

            if(year_input.Text == string.Empty)
            {
                year_input.Text = "yyyy";
                year_input.Focus();
            }
        }
    }
}
