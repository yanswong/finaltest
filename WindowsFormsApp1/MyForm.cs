using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreheatMeasure;

namespace PreheatMeasure
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            //label5.Text = "STATUS";
            InitializeComponent();
            SerialTutorial mySerialPortClass = new SerialTutorial();
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            mySerialPortClass.PV();
            string Pvvalue = SerialTutorial.value;
              label6.Text = Pvvalue;
            mySerialPortClass.ComClose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // example
            //SerialTutorial mySerialPortClass = new SerialTutorial();
            //mySerialPortClass.Rough();

            try
            {
                double checkvolt = Convert.ToDouble(textBox1.Text);
                if (checkvolt <= 1.70 || checkvolt >= 1.79)
                {
                    label5.Text = "FAIL";
                    label5.ForeColor = Color.Red;
                    SerialTutorial serialTutorial = new SerialTutorial();
                    serialTutorial.SetPV();
                    serialTutorial.PV();
                    string Pvvalue = SerialTutorial.value;
                    label6.Text = Pvvalue;
                    serialTutorial.ComClose();
                }
                else
                {
                    label5.Text = "PASS";
                    label5.ForeColor = Color.Green;

                    DialogResult savefile = MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo);
                    if (savefile == DialogResult.Yes)
                    {
                        File.WriteAllText(@"C:\Users\yanswong\Desktop\preheat", textBox1.Text);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double checkvolt = Convert.ToDouble(textBox1.Text);
                    if (checkvolt <= 1.70 || checkvolt >= 1.79)
                    {
                        label5.Text = "FAIL";
                        label5.ForeColor = Color.Red;
                        SerialTutorial serialTutorial = new SerialTutorial();
                        serialTutorial.SetPV();
                        serialTutorial.PV();
                        string Pvvalue = SerialTutorial.value;
                        label6.Text = Pvvalue;
                        serialTutorial.ComClose();
                    }
                    else
                    {
                        label5.Text = "PASS";
                        label5.ForeColor = Color.Green;
                    }
                      
                }
            
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else { }
            SerialTutorial serialTutorial1 = new SerialTutorial();
            serialTutorial1.ComClose();

        }
         
    }
}
