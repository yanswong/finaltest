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
    public partial class FormLD_Options : Form
    {
        public bool isWirelessChecked { get; set; }
        public bool isIoChecked { get; set; }

        public FormLD_Options()
        {
            InitializeComponent();
        }

        private void FormLD_Options_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            this.AcceptButton = btnOk;
            this.isWirelessChecked = false;
            this.isIoChecked = false;
        }

        private void cbWireless_CheckedChanged(object sender, EventArgs e)
        {
            this.isWirelessChecked = cbWireless.Checked;
        }

        private void cbIO_CheckedChanged(object sender, EventArgs e)
        {
            this.isIoChecked = cbIO.Checked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
