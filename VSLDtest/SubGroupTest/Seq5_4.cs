using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using CustomFormLibrary;
using SerialPortIO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using VacuumControllerIO;
using System.IO.Ports;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_4
    {
        private static int pressure;
        private static int speed;
        private static int system_pressure;
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static int Pressure
        {
            get { return pressure; }

            set { pressure = value; }
        }

        public static int System_pressure
        {
            get { return system_pressure; }

            set { system_pressure = value; }
        }

        public static int Speed
        {
            get { return speed; }

            set { speed = value; }
        }

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static TestInfo DoSeq5_4(VSLeakDetector myLD, ref TestInfo MyTestInfo, ref UUTData myuutdata)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            string retval;
            Boolean status = false;
            int step = 1;
            Helper.comPort = comPort;

            try
            {
                switch(MyTestInfo.TestLabel)
                {
                    case "5.4.1 uut_config":
                        {
                            step = 1;
                            while (step <= MyTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Enable Auto ranging mode @@//

                                Trace.WriteLine(iteSlot + "Enable auto ranging mode...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "AUTO", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Turns the auto sequencer function OFF @@//

                                Trace.WriteLine(iteSlot + "Disable auto sequencer function...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "DISABLE-SEQUENCER", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Set test preessure to 1.0 Torr @@//

                                Trace.WriteLine(iteSlot + "Set test pressure to unity");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "10.0E-0 INIT-CL-XFER", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                /*@@ An integer that scales the leak rate to account for deviations in helium compression ratios 
                                  between contraflow and midstage modes in fast turbo speed @@*/

                                Trace.WriteLine(iteSlot + "Scales the leak rate to account for deviations in helium compression ratios betwen Test & FineTest modes in fast turbo speed...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "90 INIT-FAST-VGAIN", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 = FULL cal, 1 = FAST cal @@//

                                Trace.WriteLine(iteSlot + "Run full calibration...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "0 INIT-QUICK-CAL", "ok", step, "0");
                                step++;

                                Thread.Sleep(2000);


                                //@@ GETeeTCs @@//

                                Trace.WriteLine(iteSlot + "Send command: GETeeTCs...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "GETeeTCs", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ ON Softstart @@//

                                Trace.WriteLine(iteSlot + "Informs turbo to start with softstart ON...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "SOFTSTARTON", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Prompt the operator to prepare to listen for the valves actuating @@//

                                Trace.WriteLine(iteSlot + "Ready to listen for the valves actuating...");

                                string display = "Seq 5.4.1: \nPlease prepare to listen for the valve actuating.\n\nEnter 'ok' to proceed the test once ready.";
                                retval = Helper.LabelDisplay(display);

                                if (retval == "ok")
                                {
                                    MyTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }

                                else
                                {
                                    MyTestInfo.ResultsParams[step].Result = "FAILED";
                                    Trace.WriteLine(iteSlot + "Test point failed.");
                                }

                                step++;
                                Thread.Sleep(2000);


                                //@@ Open the vent valve and a sound shall be heard when changing state @@//

                                Trace.WriteLine(iteSlot + "Opening vent valve...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "VENT_VALVE OPEN", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Verify audible run prompt @@//

                                Trace.WriteLine(iteSlot + "Verify whether the actuation is heard during the opening of the vent valve...");

                                display = "Seq 5.4.1: \nEnter 'ok' if the actuation is heard when the valve is opened.\n\nEnter 'no' if nothing is heard after the valve has opened.";
                                retval = Helper.LabelDisplay(display);

                                if (retval == "ok")
                                {
                                    MyTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }

                                else
                                {
                                    MyTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;
                                Thread.Sleep(2000);
                            }

                            break;
                        }

                    case "5.4.2 TestPortConvectorr_ATM":
                    case "5.4.14 TestPortConvectorr_ATM":
                        {
                           
                            step = 1;
                            // wait for system ready first
                            Trace.WriteLine(iteSlot + "Waiting for system ready...");
                            var sysReady = Helper.Wait_SystemReady(myLD);
                            Trace.WriteLine(iteSlot + "... ready!");
                            Thread.Sleep(2000); // delay two seconds
                                                //while (step <= MyTestInfo.ResultsParams.NumResultParams)
                                                ///{
                            //@@ Sets the current test port thermocouple reading to represent atmospheric pressure when the test port is exposed to atmosphere @@//

                            Trace.WriteLine(iteSlot + "Sets the current test port thermocouple reading to ATM when exposing to atmosphere...");

                            status = Helper.DoThis(myLD, ref MyTestInfo, "TPTCATM", "ok", step, "ok");
                            step++;

                            Thread.Sleep(2000);


                            /*@@ Obtain pressure level. Reports two lines. each begins with a <cr><lf>. 
                            The first line consists of the words test port TC followed by a number in mTorr. The second line consists of the words system TC followed by a number in uTorr. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the TP and system pressure level...");

                            retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                            if (status == true)
                            {
                                //Obtain test port pressure level
                                string[] response = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);

                                //obtain system pressure level
                                string[] response2 = retval.Split(new string[] { "Spectrometer (uTorr): ", " ok\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                                Pressure = Convert.ToInt32(response[1]);
                                System_pressure = Convert.ToInt32(response2[1]);

                                Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr   System Pressure: " + System_pressure + "uTorr");
                                // commented out below manual limit checking, use test executive to do the limit checking and display the result correctly
                                MyTestInfo.ResultsParams[2].Result = Pressure.ToString();
                                MyTestInfo.ResultsParams[3].Result = system_pressure.ToString();
                                //if (Pressure >= 700000 && Pressure <= 760000 && System_pressure < 2)
                                //{
                                //    MyTestInfo.ResultsParams[step].Result = "ok";
                                //    Trace.WriteLine(iteSlot + "Test point complete.");
                                //}
                                //else
                                //{
                                //    MyTestInfo.ResultsParams[step].Result = "FAILED";
                                //    //throw new Exception("Test point failed.");
                                //}
                            }
                            else
                            {
                                MyTestInfo.ResultsParams[step].Result = "FAILED";
                                throw new Exception("Test point failed.");
                            }

                            step++;
                            Thread.Sleep(5000);
                            //}

                            
                        }
                        break;
                    case "5.4.7 turbo_pump Init":
                        {
                            step = 1;

                            while (step <= MyTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Access full command @@//

                                Trace.WriteLine(iteSlot + "Access to full command...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "XYZZY", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Verify the state of softstart @@//

                                Trace.WriteLine(iteSlot + "Verify the status of softstart...");

                                //status = Helper.DoThis(ref MyTestInfo, "?SOFTSTART", "on ok", step, "ok");
                                status = Helper.DoThis(myLD, ref MyTestInfo, "?SOFTSTART", new string[] { "on", "ok" }, step, "ok");  // Hairus changed to check array contains 26 Jul 17
                                Thread.Sleep(6000);

                                step++;

                                //(MOD: It is necessary to wait for the UUT to fully stabilized before roughing the system)

                                Trace.WriteLine(iteSlot + "Wait for system to get ready...");

                                Helper.Wait_SystemReady(myLD);


                                //@@ Causes the leak detector to begin roughing on the test port only @@//

                                Trace.WriteLine(iteSlot + "Rough the UUT...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "ROUGH", "ok", step, "ok");
                                step++;

                                Thread.Sleep(2000);


                                //@@ Obtain the details about the state of the turbo pump @@//

                                Trace.WriteLine(iteSlot + "Verify the status and condition of the turbo pump...");

                                retval = Helper.SendCommand(myLD, ref status, "?TURBO", "Turbo Ready", "Turbo No Fault");

                                if (status == true)
                                {
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                    MyTestInfo.ResultsParams[step].Result = "ok";

                                    //string[] response = retval.Split(new string[] { "(RPM): ", " \r\nTurbo Temp" }, StringSplitOptions.RemoveEmptyEntries);
                                    //Speed = Convert.ToInt32(response[1]);

                                    string[] newResponse = retval.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    string turboSpeedResponse = newResponse.SingleOrDefault(x => x.Contains("(RPM)"));
                                    string speedText = turboSpeedResponse.Split(':').LastOrDefault();
                                    Speed = Convert.ToInt32(speedText);
                                }

                                else
                                {
                                    MyTestInfo.ResultsParams[step].Result = "FAILED";
                                    throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Retrieve and verify the turbo pump speed in RPM @@//

                                Trace.WriteLine(iteSlot + "Verify the measured speed of the turbo pump in RPM...(Measured speed = " + Speed + ")");

                                if (Speed >= Convert.ToInt32(MyTestInfo.ResultsParams[step].SpecMin) && Speed <= Convert.ToInt32(MyTestInfo.ResultsParams[step].SpecMax))
                                {
                                    MyTestInfo.ResultsParams[step].Result  = Convert.ToString(Speed);
                                    MyTestInfo.ResultsParams[step].Nominal = Convert.ToString(Speed);

                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }
                                else
                                {
                                    MyTestInfo.ResultsParams[step].Result = "FAILED";
                                    throw new Exception("Test point failed.");
                                }

                                step++;
                                Thread.Sleep(2000);
                            }

                            break;
                        }

                    case "5.4.10 Valve State":
                        {
                            step = 1;

                            while( step <= MyTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Prompt user to press ZERO if unit is stuck in CONTRAFLOW @@//

                                Trace.WriteLine(iteSlot + "Auto 'ZERO' if unit is stuck in TEST mode...");

                                status = Helper.Stuck_Contra(myLD);

                                if(status == true)
                                {
                                    MyTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }
                                
                                step++;


                                //@@ Turn softstart OFF @@//

                                Trace.WriteLine(iteSlot + "Informs turbo to start with softstart OFF...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "SOFTSTARTOFF", "ok", step, "ok");
                                step++;

                                Thread.Sleep(30000);


                                //@@ Verify Valve State @@//

                                Trace.WriteLine(iteSlot + "Verify the valve state...");

                                status = Helper.DoThis(myLD, ref MyTestInfo, "?VALVESTATE", "MIDSTAGE", step, "ok");
                                step++;
                            }

                            break;
                        }

                    case "5.4.11 IMG100_Test":
                        {
                            recheck:
                            string XGSstatus = SQL_XGS.Read_XGS_status();
                            if (XGSstatus != "No")
                            {
                                MessageBox.Show("XGS locked","warning",MessageBoxButtons.OK);
                                Thread.Sleep(5000);
                                goto recheck;
                            }
                                

                            step = 1;
                            string slotNumber = myuutdata.Options;
                            string imgCode = "IMG" + slotNumber;
                            string cnvCode = "GATE" + slotNumber;
                            string comm = "COM6";
                            //@@ IMG 100 Test, to ensure the pressure contained inside the UUT is below 5E-4 Torr @@//


                            // DB close IMG 100 YS WONG to add in
                            SQL_XGS.Update_XGS(slotNumber,"Yes");

                            WaitIMG100HighVacuumTest my_waitIMG100 = new WaitIMG100HighVacuumTest();
                            var pressure_reading = my_waitIMG100.GetHighVacuumReading(comm, "00", imgCode, cnvCode);
                            
                            MyTestInfo.ResultsParams[step].Result = pressure_reading.ToString();



                            break;
                        }

                    case "5.4.12 TestPortConvectorr_Zero":
                        {
                            step = 1;

                            //while (step <= MyTestInfo.ResultsParams.NumResultParams)
                            //{
                                //@@ Calibrate the test port convectorr gauge to 'zero' @@//

                            Trace.WriteLine(iteSlot + "Calibrate the test port convectorr gauge to ZERO...");

                            status = Helper.DoThis(myLD, ref MyTestInfo, "TPTCZERO", "ok", step, "ok");
                            step++;


                            //@@ Obtain system and test port pressure level @@//

                            Trace.WriteLine(iteSlot + "Obtain and verify the TP and system pressure level...");

                            retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);                                
                                string[] response2 = retval.Split(new string[] { "Spectrometer (uTorr): ", " ok\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                                Pressure = Convert.ToInt32(response[1]);
                                System_pressure = Convert.ToInt32(response2[1]);

                                Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr   System Pressure: " + System_pressure + "uTorr");

                                //Verify the qualification of both pressure level
                                MyTestInfo.ResultsParams[step].Result = Pressure.ToString();
                                step++;
                                MyTestInfo.ResultsParams[step].Result = System_pressure.ToString();
                                
                            }
                            else
                            {
                                MyTestInfo.ResultsParams[step].Result = "FAILED";
                                throw new Exception("Test point failed.");
                            }
                            // DB to open back IMG 100n
                            SQL_XGS.Update_XGS(myuutdata.Options,"No");
                            step++;
                            //}

                            break;
                        }

                    case "5.4.13 Vent to ATM":
                        {
                            step = 1;


                            //@@ Vent to Atmosphere @@//

                            Trace.WriteLine(iteSlot + "Vent the UUT to atmosphere...");

                            status = Helper.DoThis(myLD, ref MyTestInfo, "VENT", "VENT ok", step, "ok");
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
                            // YS wong added to make sure vented properly
                            
                            Thread.Sleep(1000);

                            //@@ Wait for UUT to vent to atmosphere @@//

                            Trace.WriteLine(iteSlot + "Wait for UUT to vent to atmosphere...");

                           
                            MyTestInfo.ResultsParams[step].Result = "ok";

                            Trace.WriteLine(iteSlot + "Test point complete.");

                            step++;


                            //@@ Obtain pressure level @@//

                            Trace.WriteLine(iteSlot + "Obtain and verify the TP pressure level...");

                            retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);

                                Pressure = Convert.ToInt32(response[1]);

                                Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr");
                                MyTestInfo.ResultsParams[step].Result = Convert.ToString(Pressure);
                                Trace.WriteLine(iteSlot + "Test point complete.");
                            }
                            else
                            {
                                MyTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }



                            break;
                        }
                    case "5.4.16 ReloadUUT":
                        {
                            step = 1;


                            //@@ wait for the UUT to respond with its 'wake up' prompt after sending command 'RELOAD' @@//

                            Trace.WriteLine(iteSlot + "Reloading the UUT...");

                            status = Helper.Reload(myLD);

                            if (status == true)
                            {
                                MyTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                MyTestInfo.ResultsParams[step].Result = "FAILED";
                            }
                            step++;


                            //@@ After reload, wait for system ready. if the system is ready, value '-1' will be returned, else '0' . The timeout is 3 mins. @@//

                            Trace.WriteLine(iteSlot + "Wait for system to get ready...");

                            status = Helper.Wait_SystemReady(myLD);

                            if (status == true)
                            {
                                MyTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                MyTestInfo.ResultsParams[step].Result = "FAILED";
                            }
                            step++;


                            break;
                        }

                    case "5.4.17 Final Verification":
                        {
                            step = 1;
                            //@@ Access to full command @@//

                            Trace.WriteLine(iteSlot + "Access to full command...");

                            status = Helper.DoThis(myLD, ref MyTestInfo, "XYZZY", "ok", step, "ok");
                            step++;


                            //@@ Verify softstart state @@//

                            Trace.WriteLine(iteSlot + "Verify the status of softstart...");

                            status = Helper.DoThis(myLD, ref MyTestInfo, "?SOFTSTART", "ok", step, "ok");

                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Helper.Fail_Test(ref MyTestInfo, ex.Message, step);
                throw;
            }

            return MyTestInfo;
        }
    }
}
