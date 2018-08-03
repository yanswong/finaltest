using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomFormLibrary;
using PluginSequence;
using SerialPortIO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using SystemIO;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_9
    {
        private static double leakrate;
        private static string exponent;
        private static double stdleak;
        private static double contra_leak;
        private static double midstage_leak;
        private static double vgain;
        private static int slotNum;

        public static int locX { get; set; }
        public static int locY { get; set; }

        public static int SlotNum
        {
            get { return slotNum; }
            set { slotNum = value; }
        }

        public static double Contra_leak
        {
            get { return contra_leak; }

            set { contra_leak = value; }
        }

        public static double Midstage_leak
        {
            get { return midstage_leak; }

            set { midstage_leak = value; }
        }

        public static double Leakrate
        {
            get { return leakrate; }

            set { leakrate = value; }
        }

        public static string Exponent
        {
            get { return exponent; }

            set { exponent = value; }
        }

        public static double Stdleak
        {
            get { return stdleak; }

            set { stdleak = value; }
        }

        public static double Vgain
        {
            get { return vgain; }

            set { vgain = value; }
        }

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static TestInfo DoSeq5_9(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval = "";
            int step = 1;

            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            Helper.comPort = comPort;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.9.1 Calibration":
                        {
                            step = 1;

                            //Access to full command

                            Trace.WriteLine(iteSlot + "Access to full command...");
                            Helper.SendCommand(myLD, ref status, "XYZZY", "ok");


                            //Obtain stdleak reading

                            Trace.WriteLine(iteSlot + "Obtain the stdleak reading...");
                            retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                Stdleak = Convert.ToDouble(response[0]);                         
                            }

                            /*@@ Initiates a Full or Fast calibration depending on system settings. The CPU software tunes,
                            then adjusts the gain so that the current helium signal causes the current leak rate measurement
                            to be the same as the most recently input using INIT-STDLEAK. If the gain is 2.9 or higher, a normal 
                            calibration is performed. Success is indicated by the normal ok response. @@*/

                            Trace.WriteLine(iteSlot + "Rough process will be triggered if UUT is not in FINE TEST mode.");

                            //myLD.Open();
                            myLD.Write("?VALVESTATE");
                            retval = myLD.Read();
                            //myLD.Close();

                            if (!retval.Contains("MIDSTAGE"))
                            {
                                Trace.WriteLine(iteSlot + "Roughing the UUT...");
                                Helper.SendCommand(myLD, ref status, "ROUGH", "ok");
                            }

                            status = Helper.Wait_FineTest(myLD);
                            //Wait for stabilization
                            Trace.WriteLine(iteSlot + "Wait for stabilization...");
                            Thread.Sleep(60000);


                            if (status == true)
                            {
                                Trace.WriteLine(iteSlot + "Initiates a FULL or FAST calibration based on system configurations...");
                                status = Helper.DoThis(myLD, ref myTestInfo, "CALIBRATE", "ok", step, "ok");
                            }

                            step++;
                            Thread.Sleep(3000);
                

                            //@@ Wait for 979 Calibration VI @@//

                            status = Helper.Wait_FineTest(myLD);

                            if (status == true)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[step].Result = "FAILED";

                            break;
                        }

                    case "5.9.2 Verify_Calibration":
                        {
                            step = 1;

                            //@@ Report and verify the status of the last calibration. @@//

                            Trace.WriteLine(iteSlot + "Report and verify the status of the last calibration...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "?CALOK", "Yesok", step, "ok");

                            break;
                        }

                    case "5.9.3 Verify_LeakReading":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Turn ON internal calibrated leak @@//
                                Thread.Sleep(2000);
                                Trace.WriteLine(iteSlot + "Open the stdleak...");

                                Helper.SendCommand(myLD, ref status, "STDLEAK", "ok");
                                if (status == false)
                                { break; }
                                //Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                                //Thread.Sleep(13000);
                                stdleakcheck:
                                status = Helper.DoThis(myLD, ref myTestInfo, "?VALVESTATE", "STDLEAK", step, "ok");
                                if (status == false)
                                {
                                    //ys wong
                                    Thread.Sleep(1000);
                                    goto stdleakcheck;
                                }
                                step++;


                                Trace.WriteLine(iteSlot + "Provide time for the UUT to read the Stdleak...");
                                // Hairus added to dynamically wait until the stdleak reading is stabilized.
                                //myLD = new VSLeakDetector(comPort);
                               // myLD.iteSlot = iteSlot;
                               // myLD.Open();
                                //bool isStdLeakState = myLD.WaitForStdLeakState(ref retval);
                               // Thread.Sleep(5000);
                               //bool isLeakStabilized = myLD.WaitForStabilizedReading(ref retval, 120, 0.97, 10);
                                Thread.Sleep(6000);
                                //myLD.Close();

                                //@@ Obtain the leakrate reading @@//
                                readleakagain:
                                Trace.WriteLine(iteSlot + "Obtain the leakrate...");
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Leakrate = Convert.ToDouble(response[0]);

                                    Trace.WriteLine(iteSlot + "Measured leak rate = " + Leakrate + "Std .cc/s");
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
                                double exp_stdleakt = 0;

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] response2 = response[0].Split(new string[] { "E-" }, StringSplitOptions.RemoveEmptyEntries);

                                    stdleakt = Convert.ToDouble(response[0]);
                                    exp_stdleakt = Convert.ToDouble(response2[1]);
                                }

                                // set max and minimum limit reading for the leak rate verification compared to stdleakt
                                // The displayed leak rate value shall be no more than +-0.2 from the ?STDLEAKt (temperature compensated cal leak) value.
                                double maxLeakDiff = stdleakt + 0.2E-7;
                                double minLeakDiff = stdleakt - 0.2E-7;
                                myTestInfo.ResultsParams[step].SpecMax = maxLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].SpecMin = minLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].Nominal = stdleakt.ToString();
                                // set the result
                                int cycle = 1;
                                recheckintcal:
                                if (!((minLeakDiff <= Leakrate) & (Leakrate <= maxLeakDiff)))
                                {
                                    status = false;
                                  
                                    retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                                    if (status == true)
                                    {
                                        string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                        Leakrate = Convert.ToDouble(response[0]);
                                        Trace.WriteLine(iteSlot + "Measured leak rate = " + Leakrate + "Std .cc/s");
                                        if (cycle >= 4)
                                        {
                                            myTestInfo.ResultsParams[step].Result = "FAILED";
                                            break;
                                        }
                                        goto recheckintcal;                                      
                                    }

                                    else
                                    {
                                        myTestInfo.ResultsParams[step].Result = "FAILED";
                                        //throw new Exception("Test point failed.");
                                    }
                                }
                                myTestInfo.ResultsParams[step].Result = Leakrate.ToString();
                                //if (Leakrate >= (stdleak - 0.2 * Math.Pow(10, exp_stdleakt)) && Leakrate <= (stdleak + 0.2 * Math.Pow(10, exp_stdleakt)))
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

                    case "5.9.4":
                        {
                            step = 1;

                            //@@ Close the internal calibrate leak @@//

                            Trace.WriteLine(iteSlot + "Close the stdleak...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");

                            step++;

                            int counter=1;       //YS WONG
                            recheck:  
                            string checker = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");
                            if (!checker.Contains("MIDSTAGE"))  
                            {
                                Thread.Sleep(2000);
                                counter++;
                                if (counter > 5)
                                { Trace.WriteLine("MIDSTAGE FAIL"); }//break; }
                                else
                                { goto recheck; }
                            }
                            // YS WONG

                            // wait until the LD fully closed STDLEAK before vent to atmospheric
                            //Thread.Sleep(5000);

                            ////////////@@ Vent the UUT @@//
                            Thread.Sleep(1000);
                            Trace.WriteLine(iteSlot + "Venting the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");

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

                            Thread.Sleep(1000);

                            break;
                        }

                    case "5.9.5 Init_Extleak":
                        {
                            step = 1;
                            //while (step <= myTestInfo.ResultsParams.NumResultParams)// again why need to do while loop here..
                            //{
                            //@@ Initiate external calibrated leak @@//
                            Trace.WriteLine(iteSlot + "Roughing the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");

                            Thread.Sleep(5000);
                            status = Helper.DoThis(myLD, ref myTestInfo, "KEEP", "ok", step, "ok");
                            Thread.Sleep(3000);
                            Trace.WriteLine(iteSlot + "Initiate external leak rate...");
                            if (myLD.iteSlot.Contains("P1"))
                                Helper.DoThis(myLD, ref myTestInfo, External_leak_Parameters.Ext_leakrate1 + " INIT-EXTLEAK", "ok", step, "ok");
                            else
                                Helper.DoThis(myLD, ref myTestInfo, External_leak_Parameters.Ext_leakrate2 + " INIT-EXTLEAK", "ok", step, "ok");
                            step++;

                            Trace.WriteLine(iteSlot + "Open the external calibrated leak...");
                            InstrumentIO.Open_Extleak(slotNum);                            //Open external calibrated leak valve
                            Thread.Sleep(2000);     // give some time to valve to fully open before roughing.
                            


                            //@@ Rough the UUT @@//

                            Trace.WriteLine(iteSlot + "Roughing the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                            step++;


                            //@@ Wait for fine test @@//
                           
                            int repeat = 1;
                            again:
                            Thread.Sleep(7000);
                            //status = Helper.Wait_FineTest(myLD, 120);
                            string check = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");


                            if (check.Contains("MIDSTAGE"))
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                //InstrumentIO.Close_Extleak(slotNum);
                                //Thread.Sleep(2000);
                                //InstrumentIO.Open_Extleak(slotNum);
                                    if (repeat >= 4)
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    break;
                                }
                                repeat++;
                                goto again;
                            }
                                

                            step++;


                            Trace.WriteLine(iteSlot + "Gives UUT time to read the leak rate...");
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                           // bool isLeakrateStabilized = myLD.WaitForStabilizedReading(ref retval, 120, 0.97, 10);
                            //myLD.Close();
                            //Thread.Sleep(20000);
                           
                            //@@ Midstage leak rate? @@//
                            // recheck:
                            Trace.WriteLine(iteSlot + "Obtain midstage leak rate...");
                            Thread.Sleep(10000);
                            //retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                            if (status == true)
                            {
                                int i = 1;
                                recheck:
                                Thread.Sleep(4000);
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");
                                string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                Midstage_leak = Convert.ToDouble(response[0]);

                                Trace.WriteLine(iteSlot + "Measured leak rate = " + Midstage_leak + "Std .cc/s");
                                
                                //myTestInfo.ResultsParams[step].Result = "ok"; // display midstage leak value instead of 'ok' text.
                                double extStdLeakRate = 0;
                                if (myLD.iteSlot.Contains("P1"))
                                {
                                    extStdLeakRate = Convert.ToDouble(External_leak_Parameters.Ext_leakrate1);
                                }
                                else
                                {
                                    extStdLeakRate = Convert.ToDouble(External_leak_Parameters.Ext_leakrate2);
                                }
                                double maxLeakRate = extStdLeakRate + 0.00000002;
                                double minLeakRate = extStdLeakRate - 0.00000002;
                                myTestInfo.ResultsParams[step].Result = Midstage_leak.ToString();
                                myTestInfo.ResultsParams[step].SpecMax = maxLeakRate.ToString();
                                myTestInfo.ResultsParams[step].SpecMin = minLeakRate.ToString();
                                myTestInfo.ResultsParams[step].Nominal = extStdLeakRate.ToString();
                                
                                if ((minLeakRate < Midstage_leak) && (Midstage_leak < maxLeakRate))
                                {
                                    myTestInfo.ResultsParams[step].Result = Midstage_leak.ToString();
                                }
                                else
                                {
                                    i++;
                                    if (i<120)
                                    {
                                        Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                        Thread.Sleep(4000);
                                        goto recheck;
                                    }
                                    
                                }

                            }
                            else
                            {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }
                            step++;
                        }
                        break;
                        //}

                    case "5.9.6 Fastvgain":
                        {
                            step = 1;

                            /*@@ An integer that scales the leak rate to account for deviations in helium compression ratios between contraflow and midstage modes
                            in fast turbo speed. @@*/

                            Trace.WriteLine(iteSlot + "Scales the leak rate to account for deviations in helium compression ratios between contraflow and midstage modes...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "1 INIT-FAST-VGAIN", "ok", step, "ok");

                            break;
                        }

                    case "5.9.7 Sniff":
                        {
                            step = 1;

                            //@@ Turns the HIGH PRESSURE TEST mode ON. @@//

                            Trace.WriteLine(iteSlot + "Turns the HIGH PRESSURE TEST mode ON....");
                            status = Helper.DoThis(myLD, ref myTestInfo, "SNIFF", "ok", step, "ok");

                            // 6 Nov 17: With London FW rev L01.01, the SNIFF command will not change LD from Fine-Test to Test mode.
                            // If we send ROUGH command it will change it to Test mode. This is some kind of bug in this LD01.01 revision.
                            // below code only for temporary.
                            string val = Helper.SendCommand(myLD, ref status, "ROUGH", "ok");
                            Thread.Sleep(2000);
                            // end temporary

                            break;
                        }

                    case "5.9.8 Measure_Contra":
                        {
                            step = 1;

                            //@@ Wait for Contraflow mode @@//
                            Thread.Sleep(20000);
                            status = Helper.Wait_Test(myLD);

                            if(status == true)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                            step++;

                            // experiment, for VGain enhancement. #1
                            var listOfVGainLimits = new List<string>();
                            listOfVGainLimits.Add(myTestInfo.TestParams[1].Value);  // portable LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[2].Value);  // portable LD 220
                            listOfVGainLimits.Add(myTestInfo.TestParams[3].Value);  // mobile/bench RVP LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[4].Value);  // mobile/bench RVP LD 220
                            listOfVGainLimits.Add(myTestInfo.TestParams[5].Value);  // mobile/bench Scroll Pump LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[6].Value);  // mobile/bench Scroll Pump LD 220
                            string[] limitsArray = listOfVGainLimits.ToArray();
                            string csvVGainLimit = Helper.GetVgainLimit(myuutdata.Model, limitsArray);  // Get VGain limits for specific LD model
                            double vGainLsl = Convert.ToDouble(csvVGainLimit.Split(',').FirstOrDefault());
                            double vGainUsl = Convert.ToDouble(csvVGainLimit.Split(',').LastOrDefault());
                            double vGainNominal = vGainLsl + ((vGainUsl - vGainLsl) / 2);
                            // expected contra-leak for vgain at nominal
                            double expContraSniffLeak = vGainNominal * Midstage_leak;
                            Trace.WriteLine(iteSlot + "Provide time for the UUT to read the leak rate...");
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            //bool isReadingMet = myLD.WaitForSpecificReading(ref retval, 120, expContraSniffLeak, 1);   //ys wong
                            //myLD.Close();

                            //// EXPERIMENT #2 WAIT FOR STABILIZED LEAK RATE AT E-6
                            //Trace.WriteLine(iteSlot + "Provide time for the UUT to read the leak rate...");
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            //isReadingMet = myLD.WaitForStabilizedReading(ref retval, 120, 0.9, 5);
                            //myLD.Close();

                            // Original from ATP
                            //Trace.WriteLine(iteSlot + "Provide time for the UUT to read the leak rate...");
                            Thread.Sleep(20000);

                            //@@ Contraflow leak value @@//
                            again:
                            Trace.WriteLine(iteSlot + "Obtain contraflow leak rate...");
                            retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                Contra_leak = Convert.ToDouble(response[0]);

                                Trace.WriteLine(iteSlot + "Measured leak rate = " + Contra_leak + "Std .cc/s");

                                if (Contra_leak == 0.00E-00)
                                { goto again; }
                                myTestInfo.ResultsParams[step].Result =
                                myTestInfo.ResultsParams[step].SpecMax =
                                myTestInfo.ResultsParams[step].SpecMin =
                                myTestInfo.ResultsParams[step].Nominal = Contra_leak.ToString();
                                //myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            { 
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }
                            break;
                        }

                    case "5.9.11 Fastvgain":
                        {
                            step = 1;

                            /*@@ An integer that scales the leak rate to account for deviations in helium compression ratios between contraflow and midstage modes
                            in fast turbo speed. @@*/

                            Vgain = Math.Round(Contra_leak / Midstage_leak, 0, MidpointRounding.AwayFromZero);

                            Trace.WriteLine(iteSlot + "Scales the leak rate to account for deviations in helium compression ratios between contraflow and midstage modes...");
                            Trace.WriteLine(iteSlot + "Vgain = " + Vgain); 

                            status = Helper.DoThis(myLD, ref myTestInfo, Vgain + " INIT-FAST-VGAIN", "ok", step, "ok");

                            Thread.Sleep(2000);
                            break;
                        }

                    case "5.9.12 Measure_Contra":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Vent the UUT @@//

                                Trace.WriteLine(iteSlot + "Venting the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");
                                step++;

                                //add in pressures //YS Wong
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

                                Thread.Sleep(1000);


                                //@@ Rough the UUT @@//

                                Trace.WriteLine(iteSlot + "Roughing the UUT...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");
                                step++;


                                //@@ Wait for Contraflow mode @@//

                                status = Helper.Wait_Test(myLD);

                                if(status == true)
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";

                                step++;
                                

                                Trace.WriteLine(iteSlot + "Provide time for the UUT to read the leak rate...");
                                //myLD = new VSLeakDetector(comPort);
                                //myLD.iteSlot = iteSlot;
                                //myLD.Open();
                              //  bool isreadingStabilized = myLD.WaitForStabilizedReading(ref retval, 120, 0.97, 10);   YS Wong
                                //myLD.Close();
                                
                                Thread.Sleep(25000);


                                //@@ Contraflow leak value @@//

                                Trace.WriteLine(iteSlot + "Obtain contraflow leak rate...");
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");

                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Contra_leak = Convert.ToDouble(response[0]);

                                    Trace.WriteLine(iteSlot + "Measured leak rate = " + Contra_leak + "Std .cc/s");
                                    myTestInfo.ResultsParams[step].Result =
                                    myTestInfo.ResultsParams[step].SpecMax =
                                    myTestInfo.ResultsParams[step].SpecMin =
                                    myTestInfo.ResultsParams[step].Nominal = Contra_leak.ToString();
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

                    case "5.9.13 Measure_Midstage":
                        {
                            step = 1;

                            while(step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Turns the HIGH PRESSURE TEST function OFF @@//

                                Trace.WriteLine(iteSlot + "Turns the HIGH PRESSURE TEST function OFF...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "NOSNIFF", "ok", step, "ok");
                                step++;


                                //@@ Wait for FINETEST MODE @@//

                                status = Helper.Wait_FineTest(myLD);

                                if(status == true)
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";

                                step++;


                                Trace.WriteLine(iteSlot + "Gives UUT time to read the leak rate...");
                                //myLD = new VSLeakDetector(comPort);
                                //myLD.iteSlot = iteSlot;
                                //myLD.Open();
                                // ys wong              //bool isreadingStabilized = myLD.WaitForStabilizedReading(ref retval, 120, 0.97, 10); 
                                //myLD.Close();
                               
                                Thread.Sleep(20000);

//midstage check not exist??
                                //@@ Midstage leak? @@//

                                Trace.WriteLine(iteSlot + "Obtain midstage leak rate...");;
                                retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");
                                
                                if (status == true)
                                {
                                    string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                    Midstage_leak = Convert.ToDouble(response[0]);

                                    Trace.WriteLine(iteSlot + "Measured leak rate = " + Midstage_leak + "Std .cc/s");
                                    myTestInfo.ResultsParams[step].Result =
                                    myTestInfo.ResultsParams[step].SpecMax =
                                    myTestInfo.ResultsParams[step].SpecMin =
                                    myTestInfo.ResultsParams[step].Nominal = Midstage_leak.ToString();
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

                    case "5.9.14 Verify_Contra_Midstage":
                        {
                            step = 1;

                            //@@ Compare the contraflow leak rate and the midstage leak rate @@//

                            Trace.WriteLine(iteSlot + "Compare the contraflow leak rate and the midstage leak rate...");
                            if (Midstage_leak < 1E-07)
                            {
                                double diff = Math.Abs(Midstage_leak - Contra_leak);
                                
                                myTestInfo.ResultsParams[step].Result = diff.ToString();
                                // good result, difference must be <=0.2E-08
                                myTestInfo.ResultsParams[step].SpecMax = "0.000000002";
                                myTestInfo.ResultsParams[step].SpecMin = "0";
                                myTestInfo.ResultsParams[step].Nominal = "0";
                                
                            }
                            else if (Midstage_leak >= 1E-07)
                            {
                                double diff = Math.Abs(Midstage_leak - Contra_leak);

                                myTestInfo.ResultsParams[step].Result = diff.ToString();
                                // good result, difference must be <=0.2E-07
                                myTestInfo.ResultsParams[step].SpecMax = "0.00000002";
                                myTestInfo.ResultsParams[step].SpecMin = "0";
                                myTestInfo.ResultsParams[step].Nominal = "0";
                            }

                            //if((Midstage_leak < 1E-07 && Math.Abs(Midstage_leak - Contra_leak) <= 0.2E-8) || (Midstage_leak >= 1E-07 && Math.Abs(Midstage_leak - Contra_leak) <= 0.2E-7))
                            //{
                            //    Trace.WriteLine(iteSlot + "Test point complete.");
                            //    myTestInfo.ResultsParams[step].Result = "ok";
                            //}
                            else
                            {          
                                // only magic will reach here               
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.9.15 Verify_Vgain":
                        {
                            step = 1;
                            // Hairus added to get VGain from PluginSequence test params instead of hardcoded
                            var listOfVGainLimits = new List<string>();
                            listOfVGainLimits.Add(myTestInfo.TestParams[1].Value);  // portable LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[2].Value);  // portable LD 220
                            listOfVGainLimits.Add(myTestInfo.TestParams[3].Value);  // mobile/bench RVP LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[4].Value);  // mobile/bench RVP LD 220
                            listOfVGainLimits.Add(myTestInfo.TestParams[5].Value);  // mobile/bench Scroll Pump LD 110
                            listOfVGainLimits.Add(myTestInfo.TestParams[6].Value);  // mobile/bench Scroll Pump LD 220
                            string[] limitsArray = listOfVGainLimits.ToArray();
                            //@@ Based on model number, verify the vgain value @@//

                            Trace.WriteLine(iteSlot + "Verify the vgain value based on the model number of the UUT...");
                            //status = Helper.GetModel_Vgain(myuutdata.Model, Vgain);   // remove hardcoded value
                            string csvVGainLimit = Helper.GetVgainLimit(myuutdata.Model, limitsArray);
                            double vGainLsl = Convert.ToDouble(csvVGainLimit.Split(',').FirstOrDefault());
                            double vGainUsl = Convert.ToDouble(csvVGainLimit.Split(',').LastOrDefault());
                            double vGainNominal = vGainLsl + ((vGainUsl - vGainLsl) / 2);
                            // set result spec limits dynamically
                            myTestInfo.ResultsParams[step].SpecMin = vGainLsl.ToString();
                            myTestInfo.ResultsParams[step].Nominal = vGainNominal.ToString();
                            myTestInfo.ResultsParams[step].SpecMax = vGainUsl.ToString();

                            // retreive the calculated vgain and set as result
                            myTestInfo.ResultsParams[step].Result = Vgain.ToString();

                            //if (status == true)
                            //{
                            //    Trace.WriteLine(iteSlot + "Test point complete.");
                            //    myTestInfo.ResultsParams[step].Result = "ok";
                            //}
                            //else
                            //{

                            //    myTestInfo.ResultsParams[step].Result = "FAILED";
                            //    //throw new Exception("Test point failed.");
                            //}

                            break;
                        }

                    case "5.9.16 Vent":
                        {
                            step = 1;

                            //@@ Vent the UUT @@//

                            Trace.WriteLine(iteSlot + "Venting the UUT...");
                            status = Helper.DoThis(myLD, ref myTestInfo, "VENT", "ok", step, "ok");
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

                            Thread.Sleep(800);
                            break;
                        }

                    case "5.9.17 Remove_Extleak":
                        {
                            step = 1;

                            //@@ Close the external calibrated leak on the test port @@//
                             
                            Trace.WriteLine(iteSlot + "Closing the external calibrated leak on the test port...");
                            InstrumentIO.Close_Extleak(slotNum);

                            Trace.WriteLine(iteSlot + "Test point complete.");
                            myTestInfo.ResultsParams[step].Result = "ok";
                            
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
