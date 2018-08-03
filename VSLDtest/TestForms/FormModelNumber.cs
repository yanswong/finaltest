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
    public partial class FormModelNumber : Form
    {
        public string ModelNumber { get; set; }

        public FormModelNumber()
        {
            InitializeComponent();
        }

        private void FormModelNumber_Load(object sender, EventArgs e)
        {
            try
            {
                this.AcceptButton = btnOk;
                this.CenterToParent();



                cbModelNumber.Items.Clear();

                //Combobox for MODEL NO.
                cbModelNumber.Items.Add("VSPR021");
                cbModelNumber.Items.Add("VSPR022");

                cbModelNumber.Items.Add("VSPD030");
                cbModelNumber.Items.Add("VSPD031");
                cbModelNumber.Items.Add("VSPD032");

                cbModelNumber.Items.Add("VSMR151");
                cbModelNumber.Items.Add("VSMR152");

                cbModelNumber.Items.Add("VSMD301");
                cbModelNumber.Items.Add("VSMD302");

                cbModelNumber.Items.Add("G8601-64004");
                cbModelNumber.Items.Add("G8601-64005");

                cbModelNumber.Items.Add("VSBR152");
                cbModelNumber.Items.Add("VSBR151");

                cbModelNumber.Items.Add("VSBD301");
                cbModelNumber.Items.Add("VSBD302");

                cbModelNumber.Items.Add("G8602-64004");
                cbModelNumber.Items.Add("G8602-64005");
                cbModelNumber.Items.Add("MSPLL10779");
                // London Part Numbers
                cbModelNumber.Items.Add("G8610-64000");
                cbModelNumber.Items.Add("G8610-64001");
                cbModelNumber.Items.Add("G8610-64002");
                cbModelNumber.Items.Add("G8610-64003");
                cbModelNumber.Items.Add("G8610-64004");

                cbModelNumber.Items.Add("G8611-64000");
                cbModelNumber.Items.Add("G8611-64001");
                cbModelNumber.Items.Add("G8611-64002");
                cbModelNumber.Items.Add("G8611-64003");
                cbModelNumber.Items.Add("G8611-64004");
                cbModelNumber.Items.Add("G8611-64005");
                cbModelNumber.Items.Add("G8611-64006");

                cbModelNumber.Items.Add("G8612-64000");
                cbModelNumber.Items.Add("G8612-64001");
                cbModelNumber.Items.Add("G8612-64002");
                cbModelNumber.Items.Add("G8612-64003");
                cbModelNumber.Items.Add("G8612-64004");
                cbModelNumber.Items.Add("G8612-64005");
                cbModelNumber.Items.Add("G8611-64006");

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
                        this.DialogResult = DialogResult.OK;
                        this.Close();
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

        private void FormModelNumber_Shown(object sender, EventArgs e)
        {
            this.cbModelNumber.Focus();
            this.cbModelNumber.SelectAll();
        }
    }
}
