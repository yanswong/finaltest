using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSLDtest.TestForms
{
    public partial class FormDcPowerSupply : Form
    {
        public double DcVolt { get; set; }
        public string SlotNum { get; set; }

        public FormDcPowerSupply()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tbDcv.Text.Trim() == "")
                {
                    MessageBox.Show("Please key in the measured 24V dc voltage!");
                    return;
                }
                else
                {
                    double dcv = Convert.ToDouble(this.tbDcv.Text);
                    this.DcVolt = dcv;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DcVolt = -999;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormDcPowerSupply_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnOk;
            //this.CenterToParent();
            this.tbDcv.Focus();
            this.tbDcv.SelectAll();
        }
    }
}
