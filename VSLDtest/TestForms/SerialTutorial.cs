using SerialPortIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreheatMeasure //using same namespace you dunt need to add using namespace.
{
    public class SerialTutorial
    {
        
        public static string value;
        //SerialPort mySerialPort;
        public int locY { get; set; }
        public int locX { get; set; }

        public SerialTutorial(VSLeakDetector myLD) //instantenous
        {
            //turn on instantenous then only to method
            //mySerialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
            //mySerialPort.Open();        // open connection        
        }

        public void PV(VSLeakDetector myLD)
        {
            try
            {
                myLD.Write("?PV\r");
                Thread.Sleep(500);
                value = myLD.Read();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetPV(VSLeakDetector myLD)
        {
            try
            {
                Form1 setpvform = new Form1();
                setpvform.StartPosition = FormStartPosition.Manual;
                setpvform.Location = new System.Drawing.Point(locX, locY);
                setpvform.ShowDialog();
                if (Form1.newpv == 0)
                {
                    Form1.newpv = 71;
                }
                else
                { }
                myLD.Write(Form1.newpv + " set_pv\r");
                Thread.Sleep(500);
                value = myLD.Read();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Reload(VSLeakDetector myLD)
        {
            try
            {
                myLD.Write("reload\r");
                Thread.Sleep(500);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public void ComClose(VSLeakDetector myLD)
        //{
        //    myLD.Close();       // close connection
        //    myLD = null;

        //}
    }
}
