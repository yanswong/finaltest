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
        //turn on instantenous then only to method
        SerialPort mySerialPort = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
        public static string value;

        public SerialTutorial() //instantenous
        {            
            mySerialPort.Open();        // open connection        
        }

        public void PV()
        {
            try
            {
                mySerialPort.Write("?PV\r");
                Thread.Sleep(500);
                value = mySerialPort.ReadExisting();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetPV()
        {
            try
            {
                Form1 setpvform = new Form1();
                setpvform.ShowDialog();
                if (Form1.newpv == 0)
                {
                    Form1.newpv = 71;
                }
                else
                { }
                mySerialPort.Write(Form1.newpv + " set_pv\r");
                Thread.Sleep(500);
                value = mySerialPort.ReadExisting();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Rough() //method
        {
            try
            {
                SerialPort mySerialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
                mySerialPort.Open();        // open connection
                mySerialPort.Write("?ROUGH");
                Thread.Sleep(5000);
                string values = mySerialPort.ReadExisting();
                mySerialPort.Close();       // close connection
                mySerialPort = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }

        public void ComClose()
        {
            mySerialPort.Close();       // close connection
            mySerialPort = null;

        }
    }
}
