using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using System.Windows.Forms;
using CustomFormLibrary;
using SerialPortIO;
using System.Threading;
using VSLDtest.SubGroupTest;
using System.Diagnostics;
using System.Globalization;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_6
    {
        //All setup parameters
        private static int setup_turbospeed;
        private static string setup_stdleak;

        //All initialization parameters
        private static Single ionchamber;
        private static Single emmisioncurrent;
        private static int offsetMidstage;
        private static int offsetContraflow;
        private static Single gain;

        //The real and exponent value for the leakrate
        private static double exponent;
        private static double leakrate;

        public static int locX { get; set; }
        public static int locY { get; set; }

        

        public static double Leakrate
        {
            get { return leakrate; }

            set { leakrate = value; }
        }

        public static double Exponent
        {
            get { return exponent; }

            set { exponent = value; }
        }

        public static int Setup_turbospeed
        {
            get { return setup_turbospeed; }

            set { setup_turbospeed = value; }
        }

        public static string Setup_stdleak
        {
            get { return setup_stdleak; }

            set { setup_stdleak = value; }
        }

        public static Single Ionchamber
        {
            get { return ionchamber; }

            set { ionchamber = value; }
        }

        public static Single Emmisioncurrent
        {
            get { return emmisioncurrent; }

            set { emmisioncurrent = value; }
        }

        public static int OffsetContraflow
        {
            get { return offsetContraflow; }
            set { offsetContraflow = value; }
        }

        public static int OffsetMidstage
        {
            get { return offsetMidstage; }

            set { offsetMidstage = value; }
        }

        public static Single Gain
        {
            get { return gain; }

            set { gain = value; }
        }

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }

        public static TestInfo DoSeq5_6(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            string retval = string.Empty;
            int step = 1;
            Helper.comPort = comPort;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.6.1 Internal_cal-leak":
                        {
                            //Access full command

                            Helper.SendCommand(myLD, ref status, "XYZZY", "ok");


                            //@@ Define the internal helium standard leak rate @@//

                            Trace.WriteLine(iteSlot + "Define the internal helium standard leak rate...");
                            if (myLD.iteSlot.Contains("P1"))
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak1 + " INIT-STDLEAK", "ok", 1, "ok");
                            else
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak2 + " INIT-STDLEAK", "ok", 1, "ok");

                            break;
                        }

                    case "5.6.2 Stdleak_expiration":
                        {
                            //@@ Enter the internal helium standard leak expiration date @@//

                            Trace.WriteLine(iteSlot + "Enter the internal helium standard leak expiration date...");
                            if (myLD.iteSlot.Contains("P1"))
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_Exp_date1 + " INIT-LKEXPIRE", "ok", 1, "ok");
                            else
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_Exp_date2 + " INIT-LKEXPIRE", "ok", 1, "ok");
                            break;
                        }

                    case "5.6.3 Cal-leak_setup":
                        {
                            step = 1;


                            //@@ Verify the expiration date @@//

                            Trace.WriteLine(iteSlot + "Verify the configured expiration date...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "?LKEXPIRE", "ok", step, "ok");
                            step++;
                            // add checking allowable expiration period. Fresh Cal leak must be more than 13 months usable period: Hairus added
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            string expiryDateStr = myLD.Query("?LKEXPIRE", ref status);
                            var arrays = expiryDateStr.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            var month = Convert.ToInt32(Convert.ToInt32(arrays[0]).ToString("00"));
                            var day = Convert.ToInt32(Convert.ToInt32(arrays[1]).ToString("00"));
                            var year = Convert.ToInt32(arrays[2]);
                            if (status)
                            {
                                var expiryDate = new DateTime(year, month, day);
                                var monthDiff = expiryDate.Subtract(DateTime.Now).TotalDays / 30;
                                myTestInfo.ResultsParams[step].Result = Convert.ToInt32(monthDiff).ToString();
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "-1";
                            }
                            //myLD.Close();
                            
                            step++;


                            //@@ Enter leak temperature @@//

                            Trace.WriteLine(iteSlot + "Enter the leak temperature...");
                            if (myLD.iteSlot.Contains("P1"))
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_temp1 + " INIT-LEAKTEMP", "ok", step, "ok");
                            else
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_temp2 + " INIT-LEAKTEMP", "ok", step, "ok");
                            step++;


                            //@@ Verify leak temperature @@//

                            Trace.WriteLine(iteSlot + "Verify the configured leak temperature...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "?LEAKTEMP", "ok", step, "ok");
                            step++;
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            var tempStr = myLD.Query("?LEAKTEMP", ref status);
                            //myLD.Close();
                            //myTestInfo.ResultsParams[step].Nominal = UUT_Parameters.Stdleak_temp;
                            //myTestInfo.ResultsParams[step].SpecMax = UUT_Parameters.Stdleak_temp;
                            //myTestInfo.ResultsParams[step].SpecMin = UUT_Parameters.Stdleak_temp;
                            if (status)
                            {
                                myTestInfo.ResultsParams[step].Result = tempStr.Replace("ok", "").Trim();
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "ERROR";
                            }
                            step++;


                            //@@ Enter leak temperature factor @@//

                            Trace.WriteLine(iteSlot + "Enter the leak temperature factor...");
                            if (myLD.iteSlot.Contains("P1"))
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_factor1 + " INIT-TEMPFACTOR", "ok", step, "ok");
                            else
                                status = Helper.DoThis(myLD, ref myTestInfo, UUT_Parameters.Stdleak_factor2 + " INIT-TEMPFACTOR", "ok", step, "ok");
                            step++;


                            //@@ Verify the leak temperature factor @@//

                            Trace.WriteLine(iteSlot + "Verify the leak temperature factor...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "?TEMPFACTOR", "ok", step, "ok");
                            step++;
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            var tempFactorStr = myLD.Query("?TEMPFACTOR", ref status);
                           // myLD.Close();
                            if (status)
                            {
                                myTestInfo.ResultsParams[step].Result = tempFactorStr.Replace("ok","").Trim();
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "ERROR";
                            }
                            step++;


                            //@@ Set to full calibration @@//

                            Trace.WriteLine(iteSlot + "Run full calibration...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "0 INIT-QUICK-CAL", "ok", step, "0");
                            step++;


                            break;
                        }

                    case "5.6.4 Ion_source":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Sets the ion voltage of the ion source. Preceded by a three-digit number (counts) in the range 0-255. @@//

                                Trace.WriteLine(iteSlot + "Sets the ion voltage of the ion source...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "90 INIT-ION", "ok", step, "ok");
                                step++;


                                /*@@ Sets the value of the gain used for adjusting the helium signal to match a calibration standard leak.
                                Preceded by a two-digit number with a decimal point after the first digit, in the range 1.0 to 6.0 @@*/

                                Trace.WriteLine(iteSlot + "Sets the value of the gain used for adjusting the helium signal to match a calibration standard leak...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "1.0 INIT-GAIN", "ok", step, "ok");
                                step++;


                                //@@ Send command 'LINEAR' to the UUT @@//

                                Trace.WriteLine(iteSlot + "Sets the leak rate analog output voltage to Linear...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "LINEAR", "ok", step, "ok");
                                step++;


                                //@@ Sets the Internal Calibrated Leak as the leak to be used during calibration. @@//

                                Trace.WriteLine(iteSlot + "Sets the Internal Calibrated Leak as the leak to be used during calibration.");

                                status = Helper.DoThis(myLD, ref myTestInfo, "INTERNAL", "ok", step, "ok");
                                step++;

                            }

                            break;
                        }

                    case "5.6.5 Setup_Parameters":
                        {
                            step = 1;

                            /*@@ Reports seven lines, each begins with a <cr><lf>. 
                            The first line reports the turbo pump speed with the RPM value or OFF.
                            The second line reports the selected manual leak rate range, and the ranging method as auto or manual. 
                            The third line reports the least sensitive leak rate range. 
                            The fourth line reports the reject set point leak rate. 
                            The fifth line reports the internal calibrated leak rate value. 
                            The sixth line reports the DAC output method as Linear, Log(2V), or Log(3V). 
                            The seventh line reports the active filament status as One or Two, followed by Lit or Out. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the setup parameters of the UUT...");
                            retval = Helper.SendCommand(myLD, ref status, "?SETUP", "ok");

                            if (status == true)
                            {
                                //Obtain turbo speed in RPM
                                string[] response1 = retval.Split(new string[] { "(RPM): ", " \n\rrange" }, StringSplitOptions.RemoveEmptyEntries);
                                //Obtain stdleak rate
                                string[] response2 = retval.Split(new string[] { "stdleak     ", "\n\routput" }, StringSplitOptions.RemoveEmptyEntries);

                                Setup_turbospeed = Convert.ToInt32(response1[1]);
                                Setup_stdleak = response2[1];

                                //Retrieve the data for setup parameters from the UUT and compare them with the values that wished to be set
                                if (myLD.iteSlot.Contains("P1"))
                                {
                                    if (Setup_turbospeed == Seq5_4.Speed && Setup_stdleak == UUT_Parameters.Stdleak1)
                                    {
                                        myTestInfo.ResultsParams[step].Result = "ok";
                                        Trace.WriteLine(iteSlot + "Test point complete.");
                                    }
                                    else
                                    {
                                        myTestInfo.ResultsParams[step].Result = "FAILED";
                                        throw new Exception("Test point failed.");
                                    }
                                }
                                else
                                {
                                    if (Setup_turbospeed == Seq5_4.Speed && Setup_stdleak == UUT_Parameters.Stdleak2)
                                    {
                                        myTestInfo.ResultsParams[step].Result = "ok";
                                        Trace.WriteLine(iteSlot + "Test point complete.");
                                    }
                                    else
                                    {
                                        myTestInfo.ResultsParams[step].Result = "FAILED";
                                        throw new Exception("Test point failed.");
                                    }
                                }
                                
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "FAILED";
                                throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.6.6 System_Recheck":
                        {

                            //@@ Wait for system ready. if the system is ready, value '-1' will be returned, else '0' . The timeout is 3 mins. @@//

                            Trace.WriteLine(iteSlot + "Wait for system to get ready...");

                            status = Helper.Wait_SystemReady(myLD);

                            if (status == true)
                            {
                                myTestInfo.ResultsParams[1].Result = "ok";
                            }
                            else
                                myTestInfo.ResultsParams[1].Result = "FAILED";


                            /*@@ Verify the reading of the initialization parameters
                            Responds with four lines. Each begins with a <cr><lf>. 
                            The first line reports the ion chamber value.
                            The second line reports the emission value.
                            The third line reports the value of the offset variable.
                            The fourth line reports the gain value. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the initialization parameters of the UUT...");
                            retval = Helper.SendCommand(myLD, ref status, "?ALL", "ok");
                            retval = retval.Replace("\r", "");
                            retval = retval.Replace("\n", "");

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


                                Ionchamber = Convert.ToSingle(response1[1]);
                                Emmisioncurrent = Convert.ToSingle(response2[1]);
                                OffsetContraflow = Convert.ToInt32(response5[0]);
                                OffsetMidstage = Convert.ToInt32(response5[1]);
                                Gain = Convert.ToSingle(response4[1]);

                                Trace.WriteLine(iteSlot + "Ionchamber = " + Ionchamber + "   Emmission Current = " + Emmisioncurrent + "   Offset = " + OffsetMidstage + "   Gain = " + Gain);
                                myTestInfo.ResultsParams[2].Result = Ionchamber.ToString();
                                myTestInfo.ResultsParams[3].Result = Emmisioncurrent.ToString();
                                myTestInfo.ResultsParams[4].Result = Gain.ToString();
                                myTestInfo.ResultsParams[5].Result = OffsetContraflow.ToString();
                                myTestInfo.ResultsParams[6].Result = OffsetMidstage.ToString();
                                
                                //Retrieve the data for initialization parameters from the UUT and compare them with the values that wished to be set
                                //if (Ionchamber >= Convert.ToSingle(myTestInfo.TestParams[1].Value) && Ionchamber <= Convert.ToSingle(myTestInfo.TestParams[2].Value) && Emmisioncurrent >= Convert.ToSingle(myTestInfo.TestParams[3].Value) && Emmisioncurrent <= Convert.ToSingle(myTestInfo.TestParams[4].Value) && Offset >= Convert.ToInt32(myTestInfo.TestParams[5].Value) && Offset <= Convert.ToInt32(myTestInfo.TestParams[6].Value) && Gain == Convert.ToSingle(myTestInfo.TestParams[7].Value))
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
                                myTestInfo.ResultsParams[0].Result = "FAILED";
                                throw new Exception("Error sending ?ALL command");
                            }
                        }
                        break;
                    case "5.6.7 Rough_&_Stablize4cal":
                        {
                            step = 1;

                            //while (step <= myTestInfo.ResultsParams.NumResultParams)
                            //{
                            //@@ Vent the UUT to atmosphere @@//

                            Trace.WriteLine(iteSlot + "Vent the UUT to atmosphere...");
                            
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
                            // Hairus added to make sure vented properly
                            //myLD.Open();
                            Thread.Sleep(1000);
                            step++;

                            //myLD.WaitForAtmosphericPressure(700000, 30000);
                            //double pressure = myLD.GetTestPortPressure();
                            //while (pressure < 700000)
                            //{
                            //    pressure = myLD.GetTestPortPressure();

                            //}
                            //if (myLD.trigger == false)
                            //myTestInfo.ResultsParams[step].Result = "notok";

                            //myLD.Close();


                            //@@ Rough the UUT @@//

                            Trace.WriteLine(iteSlot + "Rough the UUT...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "ROUGH", "ok", step, "ok");

                            Thread.Sleep(2000);
                            step++;

                            //@@ Wait for a stable leak rate reading. Reports the current Leak Rate, Pressures and Valve state. The timeout is 5 mins @@//

                            Trace.WriteLine(iteSlot + "Wait for a stable leak rate reading during FINE TEST...");
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();

                            Thread.Sleep(500);

                            int count = 1;
                            repeat:

                            // status = Helper.DoThis(myLD, ref myTestInfo, "?VALVESTATE", "MIDSTAGE", step, "ok");
                            string check = Helper.SendCommand(myLD, ref status, "?VALVESTATE", "MIDSTAGE");

                            if (check.Contains("MIDSTAGE"))
                            {
                               
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                count++;
                                if (count == 60)
                                {
                                    goto failed;
                                }
                                goto repeat;
                            }
                            //bool returnValue = myLD.WaitForFineTest(ref retval);

                            // Wait for stabilized reading
                            //bool isStabilized = myLD.WaitForStabilizedReading(ref retval);
                            //double leakReading = Convert.ToDouble(retval);
                            //if (isStabilized)
                            failed:
                            if (status)
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                            }
                            else
                            {
                                bool isStabilized = myLD.WaitForStabilizedReading(ref retval);
                                double leakReading = Convert.ToDouble(retval);
                                if (isStabilized)
                                {
                                    if (leakReading > 5E-9)
                                        myTestInfo.ResultsParams[step].Result = "notok";
                                    else
                                        myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                
                            }

                            //myLD.Close();

                            //    int timeout = 1;

                            //while (timeout > 0)
                            //{
                            //    myLD.Write("?LPV");

                            //    retval = myLD.Read();

                            //    if (retval.Contains("MIDSTAGE"))
                            //    {
                            //        int stability = 1;

                            //        string[] response = retval.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            //        double previous_leakrate = Convert.ToDouble(response[1]);

                            //        while (timeout <= Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            //        {
                            //            myLD.Write("?LPV");
                            //            retval = myLD.Read();

                            //            response = retval.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            //            string[] response2 = response[1].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                            //            Leakrate = Convert.ToDouble(response[1]);
                            //            Exponent = Convert.ToDouble(response2[1]);

                            //            //Acceptable range
                            //            if (Exponent >= -10 && Exponent <= -9)
                            //            {
                            //                if (Leakrate >= (previous_leakrate - 2 * Math.Pow(10, Exponent)) && Leakrate <= (previous_leakrate + 2 * Math.Pow(10, Exponent)))
                            //                {
                            //                    stability++;
                            //                }
                            //                else
                            //                {
                            //                    previous_leakrate = Leakrate;
                            //                    stability = 0;
                            //                }
                            //                //If the leak rate maintain and stabilize within the acceptable range declared above, the test will pass
                            //                if (stability > Convert.ToInt32(myTestInfo.TestParams[2].Value))
                            //                {
                            //                    myTestInfo.ResultsParams[step].Result = "ok";
                            //                    Trace.WriteLine(iteSlot + "Test point complete.");

                            //                    goto Done;
                            //                }
                            //                //timer
                            //                timeout++;
                            //                Thread.Sleep(1000);
                            //            }
                            //            else
                            //                timeout++;
                            //        }
                            //    }
                            //    //if the leak rate is still unstable after 4 minutes, the test will fail
                            //    if (timeout > Convert.ToInt32(myTestInfo.TestParams[1].Value))
                            //    {
                            //        myTestInfo.ResultsParams[step].Result = "FAILED";
                            //        throw new Exception("Test point failed.");
                            //    }
                            //    //timer      
                            //    Thread.Sleep(1000);
                            //    timeout++;
                            //}

                            //Done:
                            //step++;
                        }
                            break;
                        //}
                    case "5.6.8 Calibration":
                        {
                            step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                /*@@ Initiates a Full or Fast calibration depending on system settings. 
                                The CPU software tunes, then adjusts the gain so that the current helium signal causes the current leak rate measurement to be the same 
                                as the most recently input using INIT-STDLEAK. If the gain is 2.9 or higher, a normal calibration is performed. Success is indicated by
                                the normal ok response. @@*/

                                Trace.WriteLine(iteSlot + "Initiates a FULL or FAST calibration based on system configurations...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "CALIBRATE", "ok", step, "ok");
                                step++;


                                //@@ Wait for 979 Calibration VI @@//

                                status = Helper.Wait_FineTest(myLD);

                                if (status == true)
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                }
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";

                                step++;
                            }

                            break;
                        }

                    case "5.6.9 Internal-leak_Calibration":
                        {
                            step = 1;
                            retval = "";
                            bool isStdLeakState = false;
                            //while (step <= myTestInfo.ResultsParams.NumResultParams)
                            //{
                            //@@ Report and verify the status of the last calibration. @@//

                            Trace.WriteLine(iteSlot + "Report and verify the status of the last calibration...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "?CALOK", "Yesok", step, "ok");
                            step++;


                            //@@ Turn ON internal calibrate leak @@//
                            Thread.Sleep(5000);
                            Trace.WriteLine(iteSlot + "Open the stdleak...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "STDLEAK", "ok", step, "ok");
                            step++;


                            //@@ Gives UUT time to read the STD Leak @@// Read_Delay
                            //myLD = new VSLeakDetector(comPort);
                            //myLD.iteSlot = iteSlot;
                            //myLD.Open();
                            int count = 0;
                            again:
                            //    bool isStdLeakState = myLD.WaitForStdLeakState(ref retval);
                            Thread.Sleep(8000);
                            isStdLeakState = Helper.DoThis(myLD, ref myTestInfo, "?VALVESTATE", "STDLEAK", step, "ok");

                            if (!isStdLeakState)
                            {
                                Thread.Sleep(1000);
                                count++;
                                if (count == 5)
                                { isStdLeakState = false; }
                                else
                                {goto again;}
                            }
                       //     myLD.WaitForStabilizedReading(ref retval, 60, 0.97, 10);
                            //myLD.Close();
                            Trace.WriteLine(iteSlot + "Provide time for the UUT to read the Stdleak...");
                            Thread.Sleep(5000);                            
                      //      if (isStdLeakState)
                      //          myTestInfo.ResultsParams[step].Result = "ok";
                      //      else
                      //          myTestInfo.ResultsParams[step].Result = "not StdLeak";
                      //      Trace.WriteLine(iteSlot + "Test point complete.");

                            step++;

                            int stdleakfailcount = 1;
                            //@@ Get leak rate @@//
                            recheckstdleak:
                            Trace.WriteLine(iteSlot + "Obtain the leak rate...");
                            retval = Helper.SendCommand(myLD, ref status, "?LEAK", "ok");
                            string[] response = retval.Split(new string[] { "?LEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                            Leakrate = Convert.ToDouble(response[0]);

                            Trace.WriteLine(iteSlot + "Measured leakrate = " + Leakrate + "Std .cc/s");
                            // changes to recheck before test YS Wong 18 Jul 2018
                            /*
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
                                                            throw new Exception("Test point failed.");
                                                        }

                                                        step++;
                            */
                            

                            //@@ Compare the leak rate with stdleak rate @@//
                            
                            Trace.WriteLine(iteSlot + "Compare the obtained leak rate with the stdleak installed...");
                            retval = Helper.SendCommand(myLD, ref status, "?STDLEAKt", "ok");

                            double stdleakt = 0;
                            double exp_stdleak = 0;

                            if (status == true)
                            {
                                string[] resp = retval.Split(new string[] { "?STDLEAKt ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                string[] response2 = response[0].Split(new string[] { "E" }, StringSplitOptions.RemoveEmptyEntries);

                                stdleakt = Convert.ToDouble(resp[0]);
                                exp_stdleak = Convert.ToDouble(response2[1]);
                            }
                            // hairus added to get comparison stdleak properly
                            double maxLeakDiff = stdleakt + 0.2E-7;
                            double minLeakDiff = stdleakt - 0.2E-7;
                            


                            if (!((Leakrate >= minLeakDiff) & (Leakrate <= maxLeakDiff)))
                            {
                                if (stdleakfailcount >= 4)
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    step++;
                                    myTestInfo.ResultsParams[step].SpecMax = maxLeakDiff.ToString();
                                    myTestInfo.ResultsParams[step].SpecMin = minLeakDiff.ToString();
                                    myTestInfo.ResultsParams[step].Nominal = stdleakt.ToString();
                                    myTestInfo.ResultsParams[step].Result = Leakrate.ToString();
                                    break;
                                }
                                Thread.Sleep(2000);
                                stdleakfailcount++;
                                goto recheckstdleak;
                            }

                            else
                            {
                                myTestInfo.ResultsParams[step].Result = "ok";
                                step++;
                                myTestInfo.ResultsParams[step].SpecMax = maxLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].SpecMin = minLeakDiff.ToString();
                                myTestInfo.ResultsParams[step].Nominal = stdleakt.ToString();
                                myTestInfo.ResultsParams[step].Result = Leakrate.ToString();
                            }
                           
                            // set the result
                            //myTestInfo.ResultsParams[step].Result = Leakrate.ToString();

                            //if (Leakrate >= (stdleakt - 0.2 * Math.Pow(10, exp_stdleak)) && Leakrate <= (stdleakt + 0.2 * Math.Pow(10, exp_stdleak)))
                            //{
                            //    myTestInfo.ResultsParams[step].Result = "ok";
                            //    Trace.WriteLine(iteSlot + "Test point complete.");
                            //}

                            //else
                            //{
                            //    myTestInfo.ResultsParams[step].Result = "FAILED";
                            //    throw new Exception("Test point failed.");
                            //}
                            step++;
                            //}

                            break;
                        }

                    case "5.6.10 Final_verification":
                        {


                            /*@@ Verify the reading of the initialization parameters
                            Responds with four lines. Each begins with a <cr><lf>. 
                            The first line reports the ion chamber value.
                            The second line reports the emission value.
                            The third line reports the value of the offset variable.
                            The fourth line reports the gain value. @@*/

                            Trace.WriteLine(iteSlot + "Obtain and verify the initialization parameters of the UUT...");
                            Thread.Sleep(1000);
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

                                Ionchamber = Convert.ToSingle(response1[1]);
                                Emmisioncurrent = Convert.ToSingle(response2[1]);
                                OffsetContraflow = Convert.ToInt32(response5[0]);
                                OffsetMidstage = Convert.ToInt32(response5[1]);
                                Gain = Convert.ToSingle(response4[1]);

                                Trace.WriteLine(iteSlot + "Ionchamber = " + Ionchamber + "   Emmission Current = " + Emmisioncurrent + "   Offset = " + OffsetMidstage + "   Gain = " + Gain);
                                myTestInfo.ResultsParams[1].Result = "ok";
                                myTestInfo.ResultsParams[2].Result = Ionchamber.ToString();
                                myTestInfo.ResultsParams[3].Result = Emmisioncurrent.ToString();
                                myTestInfo.ResultsParams[4].Result = Gain.ToString();
                                myTestInfo.ResultsParams[5].Result = OffsetContraflow.ToString();
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



                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Helper.Fail_Test(ref myTestInfo, ex.Message, step);
                //if (myLD != null)
                    //myLD.Close();
                throw;
            }

            return myTestInfo;
        }
    }
}