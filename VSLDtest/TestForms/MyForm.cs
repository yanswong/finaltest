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
using SerialPortIO;
using System.Data.SqlClient;
using PluginSequence;

namespace PreheatMeasure
{
    public partial class MyForm : Form 
    {
       
        public string comPort = VSLDtest.SubGroupTest.Seq5_2.comPort;
        private SerialTutorial mySerialPortClass;
        private VSLeakDetector myLD;
        public MyForm(VSLeakDetector myLD)
        {
            //label5.Text = "STATUS";
            InitializeComponent();
            this.myLD = myLD;
            mySerialPortClass = new SerialTutorial(myLD);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
            mySerialPortClass.PV(myLD);
            Pvvalue = SerialTutorial.value;
            label6.Text = Pvvalue;           
            //mySerialPortClass.ComClose(myLD);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // example
            //SerialTutorial mySerialPortClass = new SerialTutorial();
            //mySerialPortClass.Rough();

            try
            {
                checkvolt = Convert.ToDouble(textBox1.Text);
                if (checkvolt <= 1.70 || checkvolt >= 1.79)
                {
                    label5.Text = "FAIL";
                    label5.ForeColor = Color.Red;
                    //SerialTutorial serialTutorial = new SerialTutorial(comPort);
                    mySerialPortClass.SetPV(myLD);
                    mySerialPortClass.PV(myLD);
                    Pvvalue = SerialTutorial.value;
                    label6.Text = Pvvalue;
                    mySerialPortClass.Reload(myLD);
                    //mySerialPortClass.ComClose(myLD);
                    MessageBox.Show("Please wait for unit restart to stabilization wait and remeasure the preheat voltage","wait...");
                }
                else
                {
                    label5.Text = "PASS";
                    label5.ForeColor = Color.Green;

                    DialogResult savefile = MessageBox.Show("Do you want to save?", "Save", MessageBoxButtons.YesNo);
                    if (savefile == DialogResult.Yes)
                    {
                        //File.WriteAllText(@"C:\Users\qqvpdtstr\Desktop\preheat.txt", textBox1.Text);

                        /* insert into PV(LOT, MODEL, PREHEAT, RESULT, DAC, FIRMWARE, ION_SOURCE)
                             values('B110', 'G8600-64000', '1.73', 'PASS', '71', '1.03', '1799-00900')
                             go*/
                        //_uutData.SerNum = formSerial.SerialNumber;
                        FormISsn IS = new FormISsn();
                        IS.ShowDialog();
                        ISsn = IS.save;
                        Close();
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
            Pvvalue = "Abort";
            Close();
        }

        public void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double checkvolt = Convert.ToDouble(textBox1.Text);
                    if (checkvolt <= 1.71 || checkvolt >= 1.78)
                    {
                        label5.Text = "FAIL";
                        label5.ForeColor = Color.Red;
                        //SerialTutorial serialTutorial = new SerialTutorial(comPort);
                        mySerialPortClass.SetPV(myLD);
                        mySerialPortClass.PV(myLD);
                        Pvvalue = SerialTutorial.value;
                        label6.Text = Pvvalue;
                        mySerialPortClass.Reload(myLD);
                        //mySerialPortClass.ComClose(myLD);
                        MessageBox.Show("Please wait for unit restart to stabilization wait and remeasure the preheat voltage", "wait...");
                    }
                    else
                    {
                        label5.Text = "PASS";
                        label5.ForeColor = Color.Green;//ADD IN DATABASE 
                    }
                      
                }
            
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else { }
            //SerialTutorial serialTutorial1 = new SerialTutorial(comPort);
            //serialTutorial1.ComClose();
        }   

        public double checkvolt { get; set; }
        public string Pvvalue { get; set; }
        public string ISsn { get; set; }
    }
}
