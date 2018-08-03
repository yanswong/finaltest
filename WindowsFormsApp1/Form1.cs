using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreheatMeasure
{
    public partial class Form1 : Form
    {
        public static decimal newpv;
        public Form1()
        {
            //textBox1.Text = "71";
            InitializeComponent();           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                newpv = Convert.ToDecimal(textBox1.Text);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
