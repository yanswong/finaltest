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
    public partial class FormSerialNumber : Form
    {
        public string SerialNumber { get; set; }

        public FormSerialNumber()
        {
            InitializeComponent();
        }

        private void FormSerialNumber_Load(object sender, EventArgs e)
        {
            try
            {
                this.AcceptButton = btnOk;
                this.CenterToParent();
                this.tbSerialNumber.Focus();
                this.tbSerialNumber.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedText = this.tbSerialNumber.Text.Trim();
                if (selectedText != "")
                {
                    // todo: in future maybe need to check the serial number formatting such as start with MY and etc.
                    this.SerialNumber = selectedText;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid Serial Number", "Invalid Serial Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void FormSerialNumber_Shown(object sender, EventArgs e)
        {
            this.tbSerialNumber.Focus();
            this.tbSerialNumber.SelectAll();
        }
    }
}
