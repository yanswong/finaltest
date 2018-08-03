using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialPortIO;
using PluginSequence;
using CustomFormLibrary;
using System.Diagnostics;
using System.Threading;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_11
    {
        private static Single gain;
        private static double stdleak;
        private static double leakrate;
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static Single Gain
        {
            get { return gain; }

            set { gain = value; }
        }

        public static double Stdleak
        {
            get { return stdleak; }

            set { stdleak = value; }
        }

        public static double Leakrate
        {
            get { return leakrate; }

            set { leakrate = value; }
        }

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static TestInfo DoSeq5_11(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            string Display;
            string Reading;
            int step = 1;
            Helper.comPort = comPort;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.11.1 Rough":
                        {
                            //Access to full command

                            Trace.WriteLine(iteSlot + "Access to full command...");
                            Helper.SendCommand(myLD, ref status, "XYZZY", "ok");

                            //Obtain stdleak rate

                            Trace.WriteLine(iteSlot + "Obtain the stdleak rate...");
                            retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");


                            //@@ Rough the UUT @@//

                            Trace.WriteLine(iteSlot + "Roughing the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");

                            step++;


                            //@@ Wait for FINETEST @@//

                            status = Helper.Wait_FineTest(myLD);

                            if(status == true)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[step].Result = "FAILED";

                            break;
                        }

                    case "5.11.2 OpenStdleak_btn":
                        {
                            step = 1;

                            //@@ Prompt user to open stdleak @@//

                            Trace.WriteLine(iteSlot + "Waiting for user to open the stdleak...");

                            Display = "Seq 5.11.2: OPEN the stdleak through the LCD screen of the UUT.\n\nThe option can be found in the CONTROL PANEL. 'STDLEAK' will be displayed on the bottom left corner of the LCD screen\n\nEnter 'ok' after the stdleak is OPEN.";
                            Reading = Helper.LabelDisplay(Display);

                            if (Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.3 Audio_btn":
                        {
                            step = 1;

                            //@@ Prompt user to press audio up button @@//

                            Trace.WriteLine(iteSlot + "Waiting for user to press the audio up button...");

                            Display = "Seq 5.11.3: Press the AUDIO UP button.\n\nThe option can be found in the CONTROL PANEL. An alarm will triggered.\n\nEnter 'ok' when done.";
                            Reading = Helper.LabelDisplay(Display);

                            if (Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.4 CloseStdleak_btn":
                        {
                            step = 1;

                            //@@ Prompt user to close stdleak @@//

                            Trace.WriteLine(iteSlot + "Waiting for user to close the stdleak...");

                            Display = "Seq 5.11.4: CLOSE the stdleak through the LCD screen of the UUT.\n\nThe option can be found in the CONTROL PANEL. 'STDLEAK' will disappear after STDLEAK has fully closed.\n\nEnter 'ok' after the stdleak is CLOSE.";
                            Reading = Helper.LabelDisplay(Display);

                            if (Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.5 Audio_btn":
                        {
                            step = 1;

                            //@@ Prompt user to press audio down button @@//

                            Trace.WriteLine(iteSlot + "Waiting for user to press the audio down button...");

                            Display = "Seq 5.11.5: Press the AUDIO DOWN button.\n\nThe option can be found in the CONTROL PANEL. An alarm will be triggered.\n\nEnter 'ok' when done.";
                            Reading = Helper.LabelDisplay(Display);

                            if (Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.1-5 btn_confirmation":
                        {
                            step = 1;

                            //@@ Prompt user whether to continue or fail the test @@//

                            Trace.WriteLine(iteSlot + "Waiting for user for confirmation...");

                            Display = "Is everything working as expected?\n\nIf yes, enter 'ok' to proceed. Else, enter 'no'.";
                            Reading = Helper.LabelDisplay(Display);

                            if (Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.6_Init_FIL2":
                        {
                            step = 1;

                            //@@ Sets the operating filament in the ion source. @@//

                            Trace.WriteLine(iteSlot + "Sets the operating filament in the ion source...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "2 INIT-FILAMENT", "ok", step, "ok");
                            Thread.Sleep(2000);                 //MOD: Provide some time for the filament to initialize.

                            break;
                        }

                    case "5.11.7_WaitSystemReady_FIL2":
                        {
                            step = 1;

                            //@@ Wait for system ready. if the system is ready, value '-1' will be returned, else '0' . The timeout is 3 mins. @@//

                            Trace.WriteLine(iteSlot + "Wait for system to get ready...");

                            status = Helper.Wait_SystemReady(myLD);

                            if(status == true)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[step].Result = "FAILED";

                            break;
                        }

                    case "5.11.8_Rough_Zero_FIL2":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Rough the UUT @@//

                                Trace.WriteLine(iteSlot + "Roughing the UUT...");
            
                                status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                step++;

                                Thread.Sleep(20000);


                                //@@ Sets the current leak rate measurement to be 0.0 atm .cc/s in the most sensitive range. @@//

                                Trace.WriteLine(iteSlot + "Sets the current leak rate measurement to be 0.0 atm .cc/s in the most sensitive range...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ZERO", "ok", step, "ok");
                                step++;
                                
                                Thread.Sleep(5000);


                                //@@ Wait for FINETEST mode @@//

                                Trace.WriteLine(iteSlot + "Wait for system to enter FINETEST mode...");

                                status = Helper.Wait_FineTest(myLD);
                                
                                if(status == true)
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }    
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";

                                step++;
                            }

                            break;
                        }

                    case "5.11.9_Calibration_FIL2":
                        {
                            int num = 1;

                            //@@ Calibrate the UUT. If fail, auto recalibrate. If fail 3 times consistently, fail the test @@//
                            //@@ MOD: Test 5.11.10.1 and 5.11.10.2 are combine together with 5.11.9 to create a recalibration loop. Therefore, if the initial calibration process failed, recalibration process will take places for up to maximum of 3 times. 
                            while (num <= Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            {                              
                                status = Helper.Recalibrate(myLD);

                                if (status == true)
                                {
                                    Trace.WriteLine(iteSlot + "Test point complete.");

                                    myTestInfo.ResultsParams[1].Result = "ok";                      //Result param = Calibrate
                                    myTestInfo.ResultsParams[2].Result = "ok";                      //Result param = Wait_Finetest
                                    myTestInfo.ResultsParams[3].Result = "ok";                      //Result param = Report_cal_status

                                    goto pass;
                                }
                                else
                                {
                                    Trace.WriteLine(iteSlot + "Number of time(s) failed: " + num);
                                    num++;
                                }
                            }

                            if (num > Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            {
                                myTestInfo.ResultsParams[1].Result = "FAILED";                          //Result param = Calibrate
                                myTestInfo.ResultsParams[2].Result = "FAILED";                          //Result param = Wait_Finetest
                                myTestInfo.ResultsParams[3].Result = "FAILED";                          //Result param = Report_cal_status

                                //throw new Exception("Recalibration failed three times.");
                            }

                        pass:
                            break;
                        }

                    case "5.11.10_Gain_FIL2":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Open the stdleak @@//

                                Trace.WriteLine(iteSlot + "Open the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                step++;


                                Trace.WriteLine(iteSlot + "Provide time for the UUT to read the stdleak...");
                                Thread.Sleep(60000);


                                //@@ Obtain the gain value of the UUT @@//

                                Trace.WriteLine(iteSlot + "Obtain the gain value of the UUT...");

                                retval = Helper.SendCommand(myLD, ref status, "?GAIN", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?GAIN ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Gain = Convert.ToSingle(response[0]);

                                    Trace.WriteLine(iteSlot + "Gain = " + Gain);
                                    Trace.WriteLine(iteSlot + "Test point complete.");

                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Verify the gain value obtained 0.2 ~2.5 @@//

                                Trace.WriteLine(iteSlot + "Verify the gain value of filament 2...");

                                if (Gain >= Convert.ToSingle(myTestInfo.ResultsParams[step].SpecMin) && Gain <= Convert.ToSingle(myTestInfo.ResultsParams[step].SpecMax))
                                {
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                    myTestInfo.ResultsParams[step].Result = Convert.ToString(Gain);
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = Convert.ToString(Gain);
                                    //throw new Exception("Test point failed.");
                                }          

                                step++;


                                //@@ Obtain the leak rate @@//

                                Trace.WriteLine(iteSlot + "Obtain the leak rate...");
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Leakrate = Convert.ToDouble(response[0]);

                                    Trace.WriteLine(iteSlot + "Measured leakrate = " + Leakrate + "Std .cc/s");
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }

                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Compare the leak rate with stdleak rate @@//
                                Trace.WriteLine(iteSlot + "Compare the obtained leak rate with the stdleak installed...");
                                retval = Helper.SendCommand(myLD, ref status, "?STDLEAKt", "ok");

                                double stdleakt = 0;
                                double exp_stdleak = 0;

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] response2 = response[0].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                    stdleakt = Convert.ToDouble(response[0]);
                                    exp_stdleak = Convert.ToDouble(response2[1]);
                                }
                                // hairus added to get comparison stdleak properly
                                double maxLeakDiff = stdleakt + 0.2E-7;
                                double minLeakDiff = stdleakt - 0.2E-7;
                                myTestInfo.ResultsParams[step].SpecMax = maxLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].SpecMin = minLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].Nominal = stdleakt.ToString();

                                // set the result
                                myTestInfo.ResultsParams[step].Result = Leakrate.ToString();

                                //Trace.WriteLine(iteSlot + "Compare the obtained leak rate with the stdleak installed...");
                                //retval = Helper.SendCommand(myLD, ref status, "?STDLEAKt", "ok");

                                //double stdleak = 0;
                                //double exp_stdleak = 0;

                                //if (status == true)
                                //{
                                //    string[] response = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                //    string[] response2 = response[0].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                //    stdleak = Convert.ToDouble(response[0]);
                                //    exp_stdleak = Convert.ToDouble(response2[1]);
                                //}

                                //if (Leakrate >= (stdleak - 0.2 * Math.Pow(10, exp_stdleak)) && Leakrate <= (stdleak + 0.2 * Math.Pow(10, exp_stdleak)))
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "ok";
                                //    Trace.WriteLine(iteSlot + "Test point complete.");
                                //}

                                //else
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "FAILED";
                                //    //throw new Exception("Test point failed.");
                                //}

                                step++;
                            }

                            break;
                        }

                    case "5.11.11_Stdleak_FIL2":
                        {
                            step = 1;

                            //@@ Close the stdleak @@//

                            Trace.WriteLine(iteSlot + "Close the stdleak...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                            Thread.Sleep(3000);

                            int counter = 1;        //YS WONG
                            recheck:
                            string checker = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");
                            if (!checker.Contains("MIDSTAGE"))
                            {
                                Thread.Sleep(2000);
                                counter++;
                                if (counter > 5)
                                { Trace.WriteLine("MIDSTAGE FAIL"); break; }
                                else
                                { goto recheck; }
                            }                       // YS WONG


                            break;
                        }

                    case "5.11.12_Vent_FIL2":
                        {
                            step = 1;

                            //@@ Vent the UUT @@//

                            Trace.WriteLine(iteSlot + "Venting the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");

                            int counter = 1;            // YS WONG
                            recheck:
                            string checker = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "VENT");
                            if (!checker.Contains("VENT"))
                            {
                                Thread.Sleep(2000);
                                counter++;
                                if (counter > 5)
                                { Trace.WriteLine("VENT FAIL"); break; }
                                else
                                { goto recheck; }
                            }
                            Thread.Sleep(2000);     // YS WONG

                            break;
                        }
                    




                    //Filament 1 will be selected and the tests above will repeat again from 5.11.7 to 5.11.12

                    case "5.11.13_Init_FIL1":
                        {
                            step = 1;

                            //@@ Sets the operating filament in the ion source. @@//

                            Trace.WriteLine(iteSlot + "Sets the operating filament in the ion source...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "1 INIT-FILAMENT", "ok", step, "ok");
                            Thread.Sleep(2000);                 //MOD: Provide some time for the filament to initialize
                            break;
                        }

                    case "5.11.7_WaitSystemReady_FIL1":
                        {
                            step = 1;

                            //@@ Wait for system ready. if the system is ready, value '-1' will be returned, else '0' . The timeout is 3 mins. @@//

                            Trace.WriteLine(iteSlot + "Wait for system to get ready...");

                            status = Helper.Wait_SystemReady(myLD);

                            if(status == true)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[step].Result = "FAILED";

                            break;
                        }

                    case "5.11.8_Rough_Zero_FIL1":
                        {
                            //@@ Rough the UUT @@//

                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Rough the UUT @@//

                                Trace.WriteLine(iteSlot + "Roughing the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                step++;

                                Thread.Sleep(20000);


                                //@@ Sets the current leak rate measurement to be 0.0 atm .cc/s in the most sensitive range. @@//

                                Trace.WriteLine(iteSlot + "Sets the current leak rate measurement to be 0.0 atm .cc/s in the most sensitive range...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ZERO", "ok", step, "ok");
                                step++;

                                Thread.Sleep(5000);


                                //@@ Wait for FINETEST mode @@//

                                Trace.WriteLine(iteSlot + "Wait for system to enter FINETEST mode...");

                                status = Helper.Wait_FineTest(myLD);

                                if(status == true)
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";

                                step++;
                            }

                            break;
                        }

                    case "5.11.9_Calibration_FIL1":
                        {
                            step = 1;
                            int num = 1;
                            
                            //@@ Calibrate the UUT. If fail, auto recalibrate. If fail 3 times consistently, fail the test @@//

                            while (num <= Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            {
                                status = Helper.Recalibrate(myLD);

                                if (status == true)
                                {
                                    Trace.WriteLine(iteSlot + "Test point complete.");

                                    myTestInfo.ResultsParams[1].Result = "ok";
                                    myTestInfo.ResultsParams[2].Result = "ok";
                                    myTestInfo.ResultsParams[3].Result = "ok";

                                    goto pass;
                                }
                                else
                                {
                                    Trace.WriteLine(iteSlot + "Number of time(s) failed: " + num);
                                    num++;
                                }
                            }

                            if (num > Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            {
                                myTestInfo.ResultsParams[1].Result = "FAILED";
                                myTestInfo.ResultsParams[2].Result = "FAILED";
                                myTestInfo.ResultsParams[3].Result = "FAILED";

                                //throw new Exception("Recalibration failed three times.");
                            }

                        pass:
                            break;
                        }

                    case "5.11.10_Gain_FIL1":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Open the stdleak @@//

                                Trace.WriteLine(iteSlot + "Open the stdleak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                step++;


                                Trace.WriteLine(iteSlot + "Provide time for the UUT to read the stdleak...");
                                Thread.Sleep(60000);


                                //@@ Obtain the gain value of the UUT @@//

                                Trace.WriteLine(iteSlot + "Obtain the gain value of the UUT...");

                                retval = Helper.SendCommand(myLD, ref status, "?GAIN", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?GAIN ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Gain = Convert.ToSingle(response[0]);

                                    Trace.WriteLine(iteSlot + "Gain = " + Gain);
                                    Trace.WriteLine(iteSlot + "Test point complete.");

                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Verify the gain value obtained 0.2~2.5 @@//

                                Trace.WriteLine(iteSlot + "Verify the gain value of filament 1...");

                                if (Gain >= Convert.ToSingle(myTestInfo.ResultsParams[step].SpecMin) && Gain <= Convert.ToSingle(myTestInfo.ResultsParams[step].SpecMax))
                                {
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                    myTestInfo.ResultsParams[step].Result = Convert.ToString(Gain);
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = Convert.ToString(Gain);
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Obtain the leak rate @@//

                                Trace.WriteLine(iteSlot + "Obtain the leak rate...");
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Leakrate = Convert.ToDouble(response[0]);

                                    Trace.WriteLine(iteSlot + "Measured leakrate = " + Leakrate + "Std .cc/s");
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }

                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ rate with stdleak rate @@//
                                Trace.WriteLine(iteSlot + "Compare the obtained leak rate with the stdleak installed...");
                                retval = Helper.SendCommand(myLD, ref status, "?STDLEAKt", "ok");

                                double stdleakt = 0;
                                double exp_stdleak = 0;

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] response2 = response[0].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                    stdleakt = Convert.ToDouble(response[0]);
                                    exp_stdleak = Convert.ToDouble(response2[1]);
                                }
                                // hairus added to get comparison stdleak properly
                                double maxLeakDiff = stdleakt + 0.2E-7;
                                double minLeakDiff = stdleakt - 0.2E-7;
                                myTestInfo.ResultsParams[step].SpecMax = maxLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].SpecMin = minLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].Nominal = stdleakt.ToString();

                                // set the result
                                myTestInfo.ResultsParams[step].Result = Leakrate.ToString();
                                //Trace.WriteLine(iteSlot + "Compare the obtained leak rate with the stdleak installed...");
                                //retval = Helper.SendCommand(myLD, ref status, "?STDLEAKt", "ok");

                                //double stdleak = 0;
                                //double exp_stdleak = 0;

                                //if (status == true)
                                //{
                                //    string[] response = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                //    string[] response2 = response[0].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                //    stdleak = Convert.ToDouble(response[0]);
                                //    exp_stdleak = Convert.ToDouble(response2[1]);
                                //}

                                //if (Leakrate >= (stdleak - 0.2 * Math.Pow(10, exp_stdleak)) && Leakrate <= (stdleak + 0.2 * Math.Pow(10, exp_stdleak)))
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "ok";
                                //    Trace.WriteLine(iteSlot + "Test point complete.");
                                //}

                                //else
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "FAILED";
                                //    //throw new Exception("Test point failed.");
                                //}

                                step++;
                            }

                            break;
                        }

                    case "5.11.11_Stdleak_FIL1":
                        {
                            step = 1;

                            //@@ Close the stdleak @@//

                            Trace.WriteLine(iteSlot + "Close the stdleak...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                            Thread.Sleep(3000);
                            int counter = 1;            // YS WONG
                            recheck:
                            string checker = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");
                            if (!checker.Contains("MIDSTAGE"))
                            {
                                Thread.Sleep(2000);
                                counter++;
                                if (counter > 5)
                                { Trace.WriteLine("MIDSTAGE FAIL"); break; }
                                else
                                { goto recheck; }
                            }                           // YS WONG

                            break;
                        }

                    case "5.11.12_Vent_FIL1":
                        {
                            step = 1;

                            //@@ Vent the UUT @@//

                            Trace.WriteLine(iteSlot + "Venting the UUT...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");
                            int counter = 1;            // YS WONG
                            recheck:
                            string checker = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "VENT");
                            if (!checker.Contains("VENT"))
                            {
                                Thread.Sleep(2000);
                                counter++;
                                if (counter > 5)
                                { Trace.WriteLine("VENT FAIL"); break; }
                                else
                                { goto recheck; }
                            }
                            Thread.Sleep(2000);     // YS WONG


                            break;
                        }

                    case "5.11.15 Final_verification":
                        {
                            step = 1;

                            //@@ Get initialization parameters from the UUT and verify them @@//

                            /*@@ Verify the reading of the initialization parameters
                            Responds with four lines. Each begins with a <cr><lf>. 
                            The first line reports the ion chamber value.
                            The second line reports the emission value.
                            The third line reports the value of the offset variable.
                            The fourth line reports the gain value. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the initialization parameters of the UUT...");
                            retval = Helper.SendCommand(myLD, ref status, "?ALL", "ok");

                            if (status == true)
                            {
                                //Obtain ionchamber value
                                string[] response1 = retval.Split(new string[] { "IONCHAMBER ", "EMISSION" }, StringSplitOptions.RemoveEmptyEntries);
                                //Obtain emmision current
                                string[] response2 = retval.Split(new string[] { "EMISSION ", "OFFSET" }, StringSplitOptions.RemoveEmptyEntries);
                                //Obtain midstage offset variable
                                string[] response3 = retval.Split(new string[] { "OFFSET ", "GAIN" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] response5 = response3[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                //Obtain gain value 
                                string[] response4 = retval.Split(new string[] { "GAIN ", "ok" }, StringSplitOptions.RemoveEmptyEntries);

                                var Ionchamber = Convert.ToSingle(response1[1]);
                                var Emmisioncurrent = Convert.ToSingle(response2[1]);
                                var OffsetContraFlow = Convert.ToInt32(response5[0]);
                                var OffsetMidstage = Convert.ToInt32(response5[1]);
                                var Gain = Convert.ToSingle(response4[1]);

                                Trace.WriteLine(iteSlot + "Ionchamber = " + Ionchamber + "   Emmission Current = " + Emmisioncurrent + "   Offset = " + OffsetMidstage + "   Gain = " + Gain);
                                myTestInfo.ResultsParams[1].Result = "ok";
                                myTestInfo.ResultsParams[2].Result = Ionchamber.ToString();
                                myTestInfo.ResultsParams[3].Result = Emmisioncurrent.ToString();
                                myTestInfo.ResultsParams[4].Result = Gain.ToString();
                                myTestInfo.ResultsParams[5].Result = OffsetContraFlow.ToString();
                                myTestInfo.ResultsParams[6].Result = OffsetMidstage.ToString();
                                //Retrieve the data for initialization parameters from the UUT and compare them with the values that wished to be set
                                //if (Ionchamber >= Convert.ToSingle(myTestInfo.TestParams[1].Value) && Ionchamber <= Convert.ToSingle(myTestInfo.TestParams[2].Value) && Emmisioncurrent >= Convert.ToSingle(myTestInfo.TestParams[3].Value) && Emmisioncurrent <= Convert.ToSingle(myTestInfo.TestParams[4].Value) && Offset >= Convert.ToInt32(myTestInfo.TestParams[5].Value) && Offset <= Convert.ToInt32(myTestInfo.TestParams[6].Value) && Gain >= Convert.ToSingle(myTestInfo.TestParams[7].Value))
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "ok";
                                //    Trace.WriteLine(iteSlot + "Test point complete.");
                                //}
                                //else
                                //{
                                //    myTestInfo.ResultsParams[step].Result = "FAILED";
                                //    throw new Exception("Test point failed.");
                                //}
                            }
                            else
                            {
                                myTestInfo.ResultsParams[1].Result = "FAILED";
                                throw new Exception("Test point failed.");
                            }
                            break;
                        }

                    case "5.11.16 Verify_Setup":
                        {
                            step = 1;

                            //@@ Retrieve the setup parameters from the UUT and compare it to the current detected leak rate @@//

                            Trace.WriteLine(iteSlot + "Obtain and verify the setup parameters of the UUT...");
                            retval = Helper.SendCommand(myLD, ref status, "?SETUP", "ok");

                            if (status == true)
                            {
                                //Obtain stdleak rate
                                string[] response = retval.Split(new string[] { "stdleak     ", "\n\routput" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] response2 = response[1].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                double Setup_stdleak = Convert.ToDouble(response[1]);
                                int Setup_exp = Convert.ToInt32(response2[1]);

                                //Retrieve the data for setup parameters from the UUT and compare them with the values that wished to be set
                                if (Leakrate >= (Setup_stdleak - 0.2*Math.Pow(10, Setup_stdleak)) && Leakrate <= (Setup_stdleak + 0.2*Math.Pow(10, Setup_stdleak)))
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine(iteSlot + "Test point complete.");
                                }
                                else
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.11.17 Vent":
                        {
                            //@@ Vent the UUT @@//

                            step = 1;

                            Trace.WriteLine(iteSlot + "Vent the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");
                            checkagain:
                            retval = Helper.SendCommand(myLD, ref status, "?PRESSURES", "ok");

                            string[] resp = retval.Split(new string[] { "(mTorr): ", "\r\nSpectrometer" }, StringSplitOptions.RemoveEmptyEntries);
                            int Pressure = Convert.ToInt32(resp[1]);


                            Trace.WriteLine(iteSlot + "Pressure: " + Pressure + "mTorr   System Pressure: ");
                            if (!(Pressure >= 700000 && Pressure <= 760000))
                            {
                                Thread.Sleep(2000);
                                goto checkagain;
                            }
                            // YS Wong added to make sure vented properly
                            
                            Thread.Sleep(1000);
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
