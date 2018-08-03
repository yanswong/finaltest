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
    public partial class FormIntCalLeak : Form
    {
        public string SerialNumber { get; set; }

        public FormIntCalLeak()
        {
            InitializeComponent();
        }

        private void FormIntCalLeak_Load(object sender, EventArgs e)
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
                var selectedText = tbSerialNumber.Text.Trim();
                if (selectedText != "")
                {
                    this.DialogResult = DialogResult.OK;
                    this.SerialNumber = selectedText;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid Serial Number", "Invalid Serial Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.tbSerialNumber.Focus();
                    this.tbSerialNumber.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void FormIntCalLeak_Shown(object sender, EventArgs e)
        {
            this.tbSerialNumber.Focus();
            this.tbSerialNumber.SelectAll();
        }
    }
}
