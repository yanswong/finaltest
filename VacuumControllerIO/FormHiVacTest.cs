// Type: VacuumControllerIO.FormHiVacTest
// Assembly: VacuumControllerIO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C1A7622-3733-44B0-841A-E16286973DCF
// Assembly location: Z:\VPD LDA\Z_General_Hairus\Projects\Intern Yan Han\IMG100\VacuumControllerIO.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VacuumControllerIO
{
    public class FormHiVacTest : Form
    {
        private IContainer components = (IContainer)null;
        private const int Timeout = 45;
        private Label label1;
        private Label label2;
        private Button btnQuit;
        private Label label3;
        private Label labelPressureReading;
        private Label label5;
        private Label lblHour;
        private Label lblMinutes;
        private Label lblSeconds;
        private Label label8;
        private Label label9;
        private Thread myThread;
        private Thread timingThread;

        public string ComPortStr { get; set; }

        public string Address { get; set; }

        public string SensorCodeImg { get; set; }

        public string SensorCodeTc { get; set; }

        public double PressureReading { get; set; }

        public DialogResult DialogFlag { get; set; }

        public string iteSlot { get; set; }

        public FormHiVacTest()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.btnQuit = new Button();
            this.label3 = new Label();
            this.labelPressureReading = new Label();
            this.label5 = new Label();
            this.lblHour = new Label();
            this.lblMinutes = new Label();
            this.lblSeconds = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Modern No. 20", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label1.Location = new Point(53, 54);
            this.label1.Name = "label1";
            this.label1.Size = new Size(510, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Waiting for Test-Port pressure to go below 5E-4 Torr";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Modern No. 20", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label2.Location = new Point(215, 92);
            this.label2.Name = "label2";
            this.label2.Size = new Size(182, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "within 45 minutes";
            this.btnQuit.Location = new Point(264, 266);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new Size(85, 51);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new EventHandler(this.btnQuit_Click);
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label3.Location = new Point(170, 215);
            this.label3.Name = "label3";
            this.label3.Size = new Size(90, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pressure :";
            this.labelPressureReading.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.labelPressureReading.ForeColor = Color.ForestGreen;
            this.labelPressureReading.Location = new Point(266, 214);
            this.labelPressureReading.Name = "labelPressureReading";
            this.labelPressureReading.Size = new Size(105, 22);
            this.labelPressureReading.TabIndex = 4;
            this.labelPressureReading.Text = "0.00 E-00";
            this.labelPressureReading.TextAlign = ContentAlignment.MiddleRight;
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label5.ForeColor = Color.ForestGreen;
            this.label5.Location = new Point(373, 215);
            this.label5.Name = "label5";
            this.label5.Size = new Size(41, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Torr";
            this.lblHour.AutoSize = true;
            this.lblHour.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblHour.Location = new Point(248, 149);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new Size(26, 18);
            this.lblHour.TabIndex = 6;
            this.lblHour.Text = "00";
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblMinutes.Location = new Point(290, 149);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new Size(26, 18);
            this.lblMinutes.TabIndex = 7;
            this.lblMinutes.Text = "00";
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblSeconds.Location = new Point(335, 149);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new Size(26, 18);
            this.lblSeconds.TabIndex = 8;
            this.lblSeconds.Text = "00";
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label8.Location = new Point(277, 148);
            this.label8.Name = "label8";
            this.label8.Size = new Size(13, 18);
            this.label8.TabIndex = 9;
            this.label8.Text = ":";
            this.label9.AutoSize = true;
            this.label9.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label9.Location = new Point(322, 148);
            this.label9.Name = "label9";
            this.label9.Size = new Size(13, 18);
            this.label9.TabIndex = 10;
            this.label9.Text = ":";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            //this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(617, 340);
            this.Controls.Add((Control)this.label9);
            this.Controls.Add((Control)this.label8);
            this.Controls.Add((Control)this.lblSeconds);
            this.Controls.Add((Control)this.lblMinutes);
            this.Controls.Add((Control)this.lblHour);
            this.Controls.Add((Control)this.label5);
            this.Controls.Add((Control)this.labelPressureReading);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.btnQuit);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new Size(633, 378);
            this.MinimizeBox = false;
            this.MinimumSize = new Size(633, 378);
            this.Name = "FormHiVacTest";
            this.Text = "VSLD High Vacuum Test";
            this.Load += new EventHandler(this.FormHiVacTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FormHiVacTest_Load(object sender, EventArgs e)
        {
            try
            {
                XgsVacuumController.Init(this.ComPortStr);
                XgsVacuumController.SetPressureUnitAsTorr(this.Address);
                Thread.Sleep(1000);
                // Check XGS configuration
                // todo: set XGS CNV as gate -user label GATE1 GATE2 GATE3 GATE4
                //XgsVacuumController.SetEmission(true, this.Address, this.SensorCodeImg);
                this.myThread = new Thread(new ThreadStart(this.DoHighVacuumTest));
                this.myThread.Name = "MyThread";
                this.myThread.Start();
                this.timingThread = new Thread(new ThreadStart(this.DoCountdown));
                this.timingThread.Name = "TimingThread";
                this.timingThread.Start();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void DoHighVacuumTest()
        {
            try
            {
                double tcPressure = 760.0;
                double imgPressure = 0.001;
                // Get TC gauge reading
                string tcString = XgsVacuumController.ReadPressureAsString(this.Address, this.SensorCodeTc);
                // Wait for 1 secs
                Thread.Sleep(1000);
                // Get IMG reading
                string imgString = XgsVacuumController.ReadPressureAsString(this.Address, this.SensorCodeImg);
                double.TryParse(imgString, out imgPressure);

                // Wait until img set to emission ON
                while (imgPressure == 0)
                {
                    Thread.Sleep(1000);
                    imgString = XgsVacuumController.ReadPressureAsString(this.Address, this.SensorCodeImg);
                    double.TryParse(imgString, out imgPressure);
                    Thread.Sleep(1000);
                    tcString = XgsVacuumController.ReadPressureAsString(this.Address, this.SensorCodeTc);
                    this.UpdatePressureReading(tcString);
                }

                // Now IMG set to Emission ON
                // Wait for 30 seconds for the IMG to initiate
                Trace.WriteLine(iteSlot + "IMG is stabilizing...");
                Thread.Sleep(30000);
                Trace.WriteLine(iteSlot + "... stabilized!");

                // Start getting IMG pressure reading
                Trace.WriteLine(iteSlot + "Waiting for IMG pressure less than 5.0E-4 Torr");
                while (imgPressure >= 0.00049)
                {
                    // read ion gauge
                    imgString = XgsVacuumController.ReadPressureAsString(this.Address, this.SensorCodeImg);
                    double.TryParse(imgString, out imgPressure);
                    this.UpdatePressureReading(imgString);
                    
                    if (imgPressure == 0)   // if IMG is off
                    {
                        imgPressure = 0.001;    // set dummy number to continue get pressure reading from IMG
                    }
                    Thread.Sleep(1000);
                }

                // Now IMG reading is less than 5E-4 Torr
                Trace.WriteLine(iteSlot + "IMG pressure now is less than 5.0E-4 torr");
                Trace.WriteLine(iteSlot + "Getting final pressure reading from the IMG100");
                this.PressureReading = XgsVacuumController.ReadPressureAsDouble(this.Address, this.SensorCodeImg);
                this.UpdatePressureReading(this.PressureReading.ToString());
                Trace.WriteLine(string.Format("IMG pressure is = {0} Torr", this.PressureReading));
                this.DialogFlag = DialogResult.OK;
                Thread.Sleep(3000);// wait for 3 seconds for display resting purpose.

                XgsVacuumController.Close();
                this.CloseForm();
            }
            catch (Exception ex)
            {
                XgsVacuumController.SetEmission(false, this.Address, this.SensorCodeImg);
                XgsVacuumController.Close();
                this.CloseForm();
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure you want to cancel this test?", "Abort", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    this.PressureReading = 9999;    // let this test failed
                    this.DialogFlag = DialogResult.Abort;
                    XgsVacuumController.Close();
                    this.CloseForm();
                }
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(iteSlot + "Error at btn quit clicked!");
                Trace.WriteLine(ex.Message);
            }
        }

        public void UpdatePressureReading(string value)
        {
            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Delegate)new FormHiVacTest.UpdatePressureReadingCallBack(this.UpdatePressureReading), new object[1]
                    {
            (object) value
                    });
                else
                    this.labelPressureReading.Text = value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CloseForm()
        {
            try
            {
                if (this.InvokeRequired)
                    this.BeginInvoke((Delegate)new FormHiVacTest.CloseFormCallback(this.CloseForm));
                else
                    this.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DoCountdown()
        {
            try
            {
                long num = 2700000L;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                while (elapsedMilliseconds < num)
                {
                    this.UpdateTimer(stopwatch.Elapsed);
                    Thread.Sleep(1000);
                }
                //XgsVacuumController.SetEmission(false, this.Address, this.SensorCodeImg);
                XgsVacuumController.Close();
                this.PressureReading = 999.0;
                this.DialogFlag = DialogResult.Abort;
                this.CloseForm();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateTimer(TimeSpan value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((Delegate)new FormHiVacTest.UpdateTimerCallback(this.UpdateTimer), new object[1]
                    {
            (object) value
                    });
                }
                else
                {
                    this.lblHour.Text = value.Hours.ToString();
                    this.lblMinutes.Text = value.Minutes.ToString();
                    this.lblSeconds.Text = value.Seconds.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private delegate void UpdatePressureReadingCallBack(string value);

        private delegate void CloseFormCallback();

        private delegate void UpdateTimerCallback(TimeSpan value);
    }
}
