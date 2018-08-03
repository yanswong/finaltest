using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using SerialPortIO;
using CustomFormLibrary;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_8
    {
        private static double stdleak;

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static int locX { get; set; }
        public static int locY { get; set; }

        public static double Stdleak
        {
            get { return stdleak; }

            set { stdleak = value; }
        }

        public static TestInfo DoSeq5_8(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval;
            int step = 1;
            Helper.comPort = comPort;


            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.8.3 Leak_Reading":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //Access to full command

                                Trace.WriteLine(iteSlot + "Access to full command...");
                                Helper.SendCommand(myLD, ref status, "XYZZY", "ok");
                                

                                //Obtain stdleak reading

                                Trace.WriteLine(iteSlot + "Obtain the stdleak reading...");
                                retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                                if(status == true)
                                {                                    
                                    string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Stdleak = Convert.ToDouble(response[0]);                                 
                                }
                                else
                                {
                                    Stdleak = -999;
                                }


                                //@@ Read and observe the leak rate by using helium spray probe. A dialog will be shown to monitor the leak rate @@//

                                Trace.WriteLine(iteSlot + "Monitoring the UUT leak reading when STDLEAK CLOSED...");

                                Leak_Reading monitor_leak_reading = new Leak_Reading(myLD);
                                monitor_leak_reading.StartPosition = FormStartPosition.Manual;
                                monitor_leak_reading.Location = new Point(locX, locY);
                                Leak_Reading.Stdleak_status = "CLOSE";
                                monitor_leak_reading.ShowDialog();

                                if (monitor_leak_reading.DialogResult == DialogResult.Yes)
                                {
                                    monitor_leak_reading.Hide();

                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                    myTestInfo.ResultsParams[step].Result = "ok";

                                    step++;
                                }

                                else if (monitor_leak_reading.DialogResult == DialogResult.No)
                                {
                                    monitor_leak_reading.Dispose();
                                                                   
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }


                                //@@ Open the Stdleak @@//

                                Trace.WriteLine(iteSlot + "Opening the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");

                                Thread.Sleep(12000);  //(MOD: Time is required to fully open the stdleak contained inside the UUT. In order to prevent any interruption while it is opening, a small interval must be implemented.
                                step++;


                                //@@ Read and observe the stdleak rate by using helium spray probe. A dialog will be shown to monitor the leak rate @@//

                                Trace.WriteLine(iteSlot + "Monitoring the UUT leak reading when STDLEAK OPEN...");

                                Leak_Reading.Stdleak_status = "OPEN";
                                monitor_leak_reading.ShowDialog();

                                if(monitor_leak_reading.DialogResult == DialogResult.Yes)
                                {
                                    monitor_leak_reading.Dispose();

                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                    myTestInfo.ResultsParams[step].Result = "ok";
                         
                                    step++;
                                }

                                else if(monitor_leak_reading.DialogResult == DialogResult.No)
                                {
                                    monitor_leak_reading.Dispose();
                                                                    
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }
   

                                //@@ Close the Stdleak @@//

                                Trace.WriteLine(iteSlot + "Closing the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                step++;
                            }

                            break;
                        }

                    case "5.8.6 Final_setup":
                        {
                            step = 1;

                            //@@ Sets the leak rate analog output voltage to Linear. Not preceded by a value. @@//

                            Trace.WriteLine(iteSlot + "Sets the leak rate analog output voltage to linear...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "INIT-LINEAR", "ok", step, "ok");

                            step++;


                            //@@ Sets the status of Auto-zero < 0.  Preceded by 0 or 1, 0 = OFF, 1 = ON. @@//

                            Trace.WriteLine(iteSlot + "Sets the status of Auto Zero to ON...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "1 INIT-AZ<0", "ok", step, "ok");

                            break;
                        }

                    case "5.8.7 Valve_cycle":
                        {
                            step = 1;

                            while(step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Access to full command @@//

                                Trace.WriteLine(iteSlot + "Access to full command...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "XYZZY", "ok", step, "ok");
                                step++;


                                //Read the stdleak rate

                                Trace.WriteLine(iteSlot + "Obtain the stdleak reading...");

                                retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                                if(status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?STDLEAK ", "ok"}, StringSplitOptions.RemoveEmptyEntries);
                                    Stdleak = Convert.ToDouble(response[0]);
                                }


                                //@@ Vent the UUT @@//

                                Trace.WriteLine(iteSlot + "Venting the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");
                                step++;
                                checkagain:
                                retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                                string[] resp = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);
                                int Pressure = Convert.ToInt32(resp[1]);


                                Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr   System Pressure: ");
                                // commented out below manual limit checking, use test executive to do the limit checking and display the result correctly
                                if (!(Pressure >= 700000 && Pressure <= 760000))
                                {
                                    Thread.Sleep(2000);
                                    goto checkagain;
                                }
                                // YS Wong added to make sure vented properly
                              
                                Thread.Sleep(1000);



                                //@@ Rough the UUT @@//

                                Trace.WriteLine(iteSlot + "Roughing the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                Thread.Sleep(30000);
                                                //YS Wong
                                string update;
                                int counter = 1;
                                recheck:
                                update = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");
                                if (!update.Contains("MIDSTAGE"))
                                    {
                                    Thread.Sleep(2000);
                                    update = null;
                                    counter++;
                                        if (counter > 10)
                                        { break; }
                                        else
                                        { goto recheck; }
                                    }
                                
                                step++;
                                update = null;  // YS Wong
                                


                                //@@ STDleak ON @@//

                                Trace.WriteLine(iteSlot + "Open the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                step++;

                                Thread.Sleep(10000);


                                //@@ Vent the UUT @@//

                                Trace.WriteLine(iteSlot + "Venting the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");

                                checkagain2:
                                retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                                string[] resp2 = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);
                                int Pressure2 = Convert.ToInt32(resp2[1]);


                                Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr   System Pressure: ");
                                // commented out below manual limit checking, use test executive to do the limit checking and display the result correctly
                                if (!(Pressure >= 700000 && Pressure <= 760000))
                                {
                                    Thread.Sleep(2000);
                                    goto checkagain2;
                                }
                                // Hairus added to make sure vented properly
                                //myLD.Open();
                                Thread.Sleep(1000);

                                step++;

                                


                                //@@ Rough the UUT @@//

                                Trace.WriteLine(iteSlot + "Roughing the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                Thread.Sleep(30000);
                                            // YS Wong

                                update = null; counter = 1;
                                recheck2:
                                update = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");
                                if (!update.Contains("MIDSTAGE"))
                                {
                                    Thread.Sleep(2000);
                                    update = null;
                                    counter++;
                                    if (counter > 10)
                                    { break; }
                                    else
                                    { goto recheck2; }
                                }

                                step++;
                                update = null; // YS Wong

                                //@@ STDleak OFF @@//

                                Trace.WriteLine(iteSlot + "Close the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                step++;

                                Thread.Sleep(30000);

                                // After valve cycle test completed, wait until the leak rate stabilize before proceed for calibration
                                //VSLeakDetector myLD = new VSLeakDetector(comPort);
                               // myLD.iteSlot = iteSlot;
                                string myRetVal = "";
                                //myLD.Open();
                                myLD.WaitForStdLeakState(ref myRetVal);
                                Thread.Sleep(1000);
                                bool isStabilized = myLD.WaitForStabilizedReading(ref myRetVal, 60, 0.97, 10);
                                if (isStabilized)
                                    Trace.WriteLine(iteSlot + "Std Leak reading is now stabilized");
                                else
                                    Trace.WriteLine(iteSlot + "Unable to get stable leak rate reading");
                                //myLD.Close();
                                //myLD = null;
                            }

                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Helper.Fail_Test(ref myTestInfo, ex.Message, step);
                throw;
            }

            return myTestInfo;
        } 
    }
}
