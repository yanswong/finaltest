using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VSLDtest.SubGroupTest;

namespace VSLDtest.TestForms
{
    
    public partial class CalleakinfoDB : Form
    {
        public static string iteSlot { get; internal set; }
        public CalleakinfoDB()
        {
            InitializeComponent();
            if (iteSlot.Contains("P1"))
            {
                label7.Text = UUT_Parameters.Stdleak_SN1;
                label8.Text = UUT_Parameters.Stdleak1;
                label9.Text = UUT_Parameters.Stdleak_Exp_date1;
                label10.Text = UUT_Parameters.Stdleak_temp1;
                label11.Text = UUT_Parameters.Stdleak_factor1;
            }
            else if (iteSlot.Contains("P2"))
            {
                label7.Text = UUT_Parameters.Stdleak_SN2;
                label8.Text = UUT_Parameters.Stdleak2;
                label9.Text = UUT_Parameters.Stdleak_Exp_date2;
                label10.Text = UUT_Parameters.Stdleak_temp2;
                label11.Text = UUT_Parameters.Stdleak_factor2;
            }
            else
            {
               MessageBox.Show("ERRORR with cal leak info", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Logger.WriteLine("ERRORR with cal leak info");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ERRORR with cal leak info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
        }
        
    }
}
