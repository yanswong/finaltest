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
    public partial class StartupForm : Form
    {
        string model, serial, spectroserial, slot, customer_opt;
        

        public string Model
        {
            get { return model; }

            set { model = value; }
        }

        public string Serial
        {
            get { return serial; }

            set { serial = value; }
        }

        public string SpectroSerial
        {
            get { return spectroserial; }

            set { spectroserial = value; }
        }

        public string Slot
        {
            get { return slot; }

            set { slot = value; }
        }

        public string Customer_opt
        {
            get { return customer_opt; }

            set { customer_opt = value; }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

        }

        public StartupForm()
        {
            InitializeComponent();
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            button_apply.DialogResult = DialogResult.OK;
            button_cancel.DialogResult = DialogResult.Cancel;

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            //Combobox for MODEL NO.
            comboBox1.Items.Add("VSPR021");
            comboBox1.Items.Add("VSPR022");
           
            //comboBox1.Items.Add("-------------------------");
            
            comboBox1.Items.Add("VSPD030");
            comboBox1.Items.Add("VSPD031");
            comboBox1.Items.Add("VSPD032");

            //comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSMR151");
            comboBox1.Items.Add("VSMR152");

            //comboBox1.Items.Add("-------------------------");
            
            comboBox1.Items.Add("VSMD301");
            comboBox1.Items.Add("VSMD302");

            //comboBox1.Items.Add("-------------------------");
            
            comboBox1.Items.Add("G8601-64004");
            comboBox1.Items.Add("G8601-64005");

            //comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSBR152");
            comboBox1.Items.Add("VSBR151");

            //comboBox1.Items.Add("-------------------------");
            
            comboBox1.Items.Add("VSBD301");
            comboBox1.Items.Add("VSBD302");

            //comboBox1.Items.Add("-------------------------");
            
            comboBox1.Items.Add("G8602-64004");
            comboBox1.Items.Add("G8602-64005");

            
            //Combobox for slot no.
            comboBox2.Items.Add("1");
            comboBox2.Items.Add("2");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "-------------------------")
            {
                MessageBox.Show("Please select the actual model number.");
                button_apply.Hide();
            }
            else
                button_apply.Show();
        }

        private void button_apply_Click(object sender, EventArgs e)
        {

            Model = Convert.ToString(comboBox1.SelectedItem);
            Serial = text_serial.Text;
            SpectroSerial = text_spec.Text;
            Slot = Convert.ToString(comboBox2.SelectedItem);

            if (checkbox_IOboard.Checked == true)
            {
                Customer_opt = "IO Board";
            }

            else if (checkbox_Wireless.Checked == true)
            {
                Customer_opt = "Wireless Remote";
            }

            else if(checkbox_IOboard.Checked == true && checkbox_Wireless.Checked == true)
            {
                Customer_opt = "Both";
            }

            else
                Customer_opt = "None";
        }
    }
}
