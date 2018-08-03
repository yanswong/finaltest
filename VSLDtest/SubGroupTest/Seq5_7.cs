using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialPortIO;
using PluginSequence;
using CustomFormLibrary;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_7
    {
        private static string stdleak;
        private static string setup_stdleak;
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static string Stdleak
        {
            get { return stdleak; }

            set { stdleak = value; }
        }

        public static string Setup_stdleak
        {
            get { return setup_stdleak; }

            set { setup_stdleak = value; }
        }

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static TestInfo DoSeq5_7(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            int step = 1;
            Helper.comPort = comPort;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.7.1 Startup":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Access to full command @@//

                                Trace.WriteLine(iteSlot + "Access to full command...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "XYZZY", "ok", step, "ok");
                                step++;


                                //@@ Read Stdleak from UUT @@//

                                Trace.WriteLine(iteSlot + "Read the stdleak installed in the UUT...");
                                retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");
                                
                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Stdleak = response[0];
                                                                 
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;
                            }

                            break;
                        }

                    case "5.7.6 Setup_Parameters":
                        {
                            step = 1;

                            //@@ Verify setup parameters @@//

                            /*@@ Reports seven lines, each begins with a <cr><lf>. 
                            The first line reports the turbo pump speed with the RPM value or OFF.
                            The second line reports the selected manual leak rate range, and the ranging method as auto or manual. 
                            The third line reports the least sensitive leak rate range. 
                            The fourth line reports the reject set point leak rate. 
                            The fifth line reports the internal calibrated leak rate value. 
                            The sixth line reports the DAC output method as Linear, Log(2V), or Log(3V). 
                            The seventh line reports the active filament status as One or Two, followed by Lit or Out. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the setup parameter of the UUT...");
                            retval = Helper.SendCommand(myLD, ref status, "?SETUP", "ok");

                            if (status == true)
                            {
                                //Obtain stdleak rate
                                string[] response = retval.Split(new string[] { "stdleak     ", "\n\routput" }, StringSplitOptions.RemoveEmptyEntries);

                                for (int j = 0; j < 2; j++)
                                {
                                    Setup_stdleak = response[j];
                                }

                                //Retrieve the data for setup parameters from the UUT and compare them with the values that wished to be set
                                myTestInfo.ResultsParams[step].SpecMax = Stdleak;
                                myTestInfo.ResultsParams[step].SpecMin = Stdleak;
                                myTestInfo.ResultsParams[step].Nominal = Stdleak;
                                myTestInfo.ResultsParams[step].Result = Setup_stdleak;
                                //if (Setup_stdleak == Stdleak)
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "ok";
                                //    Trace.WriteLine(iteSlot + "Test point complete.");
                                //}
                                //else
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "FAILED";
                                //    //throw new Exception("Test point failed.");
                                //}
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.7.7 Off_stdleak":
                        {
                            step = 1;

                            //@@ Turn off standard leak @@//

                            Trace.WriteLine(iteSlot + "Close the stdleak...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                            Thread.Sleep(12000); //(MOD: Time is required to fully close the stdleak contained inside the UUT. In order to prevent any interruption while it is closing, a small interval must be implemented.

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
