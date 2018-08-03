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
    public partial class FormExtCalLeak : Form
    {
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }

        public FormExtCalLeak()
        {
            InitializeComponent();
        }

        private void FormExtCalLeak_Load(object sender, EventArgs e)
        {
            try
            {
                this.AcceptButton = btnOk;
                this.CenterToParent();
                this.tbSerialNumber.Focus();
                this.tbSerialNumber.SelectAll();

                cbModelNumber.Items.Clear();

                //Combobox for MODEL NO.
                cbModelNumber.Items.Add("F8473321");
                cbModelNumber.SelectedIndex = 0;
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
                var selectedText = cbModelNumber.Text.Trim();
                if (selectedText != "")
                {
                    if (cbModelNumber.Items.Contains(selectedText))
                    {
                        // the value entered is one of the valid model numbers, return it
                        this.ModelNumber = selectedText;

                        if(tbSerialNumber.Text.Trim() != "")
                        {
                            this.SerialNumber = tbSerialNumber.Text;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid serial number!", "Invalid Serial Number", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Model Number entered is not in the valid model number list. Please reenter the model number!", "Model Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.cbModelNumber.Focus();
                        this.cbModelNumber.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid Model Number", "Invalid Model Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void FormExtCalLeak_Shown(object sender, EventArgs e)
        {
            this.tbSerialNumber.Focus();
            this.tbSerialNumber.SelectAll();
        }
    }
}
