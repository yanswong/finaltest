using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginSequence;

namespace Demo2
{
    public partial class uut_form : Form
    {
        public uut_form()
        {
            InitializeComponent();
        }
       
        private void uut_form_Load(object sender, EventArgs e)                          //ComboBox customization
        {
            button_apply.DialogResult = DialogResult.OK;
            button_cancel.DialogResult = DialogResult.Cancel;

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            //Combobox for MODEL NO.
            comboBox1.Items.Add("VSPR021");
            comboBox1.Items.Add("VSPR022");
            comboBox1.Items.Add("G8600-60000");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSPD030");
            comboBox1.Items.Add("VSPD031");
            comboBox1.Items.Add("VSPD032");
            comboBox1.Items.Add("G8600-60001");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSMR151");
            comboBox1.Items.Add("VSMR152");
            comboBox1.Items.Add("G8601-60000");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSMD301");
            comboBox1.Items.Add("VSMD302");
            comboBox1.Items.Add("G8601-60001");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("G8601-64004");
            comboBox1.Items.Add("G8601-64005");
            comboBox1.Items.Add("G8601-60002");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSBR152");
            comboBox1.Items.Add("VSBR151");
            comboBox1.Items.Add("G8602-60000");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("VSBD301");
            comboBox1.Items.Add("VSBD302");
            comboBox1.Items.Add("G8602-60001");

            comboBox1.Items.Add("-------------------------");

            comboBox1.Items.Add("G8602-64004");
            comboBox1.Items.Add("G8602-64005");
            comboBox1.Items.Add("G8602-60002");

            //Combobox for OPTION
            comboBox2.Items.Add("0001");
            comboBox2.Items.Add("0002");
            comboBox2.Items.Add("0003");
            comboBox2.Items.Add("Not Aplicable");
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "-------------------------")
            {
                MessageBox.Show("Please select the actual model number.");
                button_apply.Hide();
            }

            else
                button_apply.Show();
        }
       
        private void button_apply_Click(object sender, EventArgs e)
        {
            PerformanceTestManager begin = new PerformanceTestManager();

            string model = Convert.ToString(comboBox1.SelectedItem);
            string serial = text_serial.Text;
            string option = Convert.ToString(comboBox2.SelectedItem);
            string spec = text_spec.Text;

            begin.Model = model;
            begin.Serial = serial;
            begin.Option = option;
            begin.Spec = spec;
        }        
    }
}
