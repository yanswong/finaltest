using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialPortIO;
using PluginSequence;
using System.Threading;
using System.IO.Ports;


namespace CustomFormLibrary
{
    public partial class Leak_Reading : Form
    {
        private static string leakrate;
        private static int num = 1;
        private static int store_num = 1;
        private static string stdleak_status;
        private VSLeakDetector myLD;


        public static string Stdleak_status
        {
            get { return stdleak_status; }

            set { stdleak_status = value; }
        }

        public static string Leakrate
        {
            get { return leakrate; }

            set { leakrate = value; }
        }

        public Leak_Reading(VSLeakDetector myLD)
        {
            InitializeComponent();
            this.myLD = myLD;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void Leak_Reading_Load(object sender, EventArgs e)
        {
            label1.Text = "Helium Spray Check with STDLEAK " + Stdleak_status +"\n---------------------------------------------------------\nSpray helium into the exhaust port of the calibrated leak\nvalve and watch for a response. Press APPROVE if the\nresponse is acceptable, else press REJECT.\nThe leak rate can be observed below: ";
            list_leakrate.Items.Clear();
            text_leakrate.Clear();

            num = 1;
            store_num = 1;

            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_approve.Enabled = false;
            button_reject.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (num > 0)
            {
                backgroundWorker1.ReportProgress(num);

                Thread.Sleep(1000);
                num++;

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    num = 0;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int counter = e.ProgressPercentage;

            string retval;
            //VSLeakDetector myLD = new VSLeakDetector("COM4");
          
            //myLD.Open();

            myLD.Write("?LEAK");
            retval = myLD.Read();

            if (retval.Contains("ok"))
            {
                string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < 1; j++)
                {
                    Leakrate = response[j];
                }
            }

            text_leakrate.Text = Leakrate + "Std .cc/s";

            if (text_leakrate.Text != null)
            {
                list_leakrate.Items.Add(counter + ": " + Leakrate + "Std .cc/s");
            }

            list_leakrate.SelectedIndex = list_leakrate.Items.Count - 1;

            //myLD.Close();
        }

        private void button_approve_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();             
            this.DialogResult = DialogResult.Yes;          
        }

        private void button_reject_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();            
            this.DialogResult = DialogResult.No;          
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            num = store_num;

            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();

                button_start.Enabled = false;
                button_pause.Enabled = true;
                button_approve.Enabled = true;
                button_reject.Enabled = true;
            }       
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            store_num = num + 1;

            backgroundWorker1.CancelAsync();

            Thread.Sleep(1200);
            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_approve.Enabled = true;
            button_reject.Enabled = true;
        }
    }
}
