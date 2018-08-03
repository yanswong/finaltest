using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialPortIO;
using PluginSequence;
using System.Diagnostics;
using CustomFormLibrary;
using System.Windows.Forms;
using System.Threading;
using SystemIO;
using System.IO;

namespace VSLDtest
{
    public static class Helper
    {
        //@@ Retrieve the pump type of the UUT based on its model number. @@//
        //public static string iteSlot { get; set; }
        public static string comPort { get; internal set; }

        public static int locX { get; set; }
        public static int locY { get; set; }

        public static string GetTestSequenceFilePath(string modelNumber)
        {
            try
            {
                string filepath = "";
                switch (modelNumber)
                {
                    default:
                        break;
                }

                return filepath;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetPumpType(string modelNumber)
        {
            try
            {
                string pumpType;
                switch (modelNumber)
                {
                    //case for Wet VS
                    case "VSPR021":
                    case "VSPR022":
                    case "VSMR151":
                    case "VSMR152":
                    case "VSBR151":
                    case "VSBR152":
                    case "MSPLL10779":
                    case "MSPLL10767":
                    // case for wet London
                    case "G8610-64000":
                    case "G8610-64001":
                    case "G8611-64000":
                    case "G8611-64001":
                    case "G8611-64006":
                    case "G8612-64000":
                    case "G8612-64001":
                    case "G8612-64006":
                        {
                            pumpType = "Wet";
                        }
                        break;

                    //case for Dry
                    case "VSMD301":
                    case "VSMD302":
                    case "VSPD030":
                    case "VSPD031":
                    case "VSPD032":
                    case "VSBD301":
                    case "VSBD302":
                    case "G8601-64004":
                    case "G8601-64005":
                    case "G8602-64004":
                    case "G8602-64005":
                    // case for Dry London
                    case "G8610-64002":
                    case "G8610-64003":
                    case "G8610-64004":
                    case "G8611-64002":
                    case "G8611-64003":
                    case "G8611-64004":
                    case "G8611-64005":
                    case "G8612-64002":
                    case "G8612-64003":
                    case "G8612-64004":
                    case "G8612-64005":
                        {
                            pumpType = "Dry";
                        }
                        break;

                    default:
                        pumpType = "Unknown";
                        break;
                }

                return pumpType;
            }

            catch (Exception)
            {

                throw;
            }
        }

        public static bool IsLDWithDiaphragmPump(string modelNumber)
        {
            try
            {
                switch (modelNumber)
                {
                    //case for LD without diaphragm pump
                    case "VSPR021":
                    case "VSPR022":
                    case "VSMR151":
                    case "VSMR152":
                    case "VSBR151":
                    case "VSBR152":
                    // case for LD without diaphragm pump London
                    case "G8610-64000":
                    case "G8610-64001":
                    case "G8611-64000":
                    case "G8611-64001":
                    case "G8611-64006":
                    case "MSPLL10779":
                    case "G8612-64000":
                    case "G8612-64001":
                    case "G8612-64006":
                    case "MSPLL10767":
                        return false;
                    //case for LD with diaphragm pump
                    case "VSMD301":
                    case "VSMD302":
                    case "VSPD030":
                    case "VSPD031":
                    case "VSPD032":
                    case "VSBD301":
                    case "VSBD302":
                    case "G8601-64004":
                    case "G8601-64005":
                    case "G8602-64004":
                    case "G8602-64005":
                    // case for LD with diaphragm pump London
                    case "G8610-64002":
                    case "G8610-64003":
                    case "G8610-64004":
                    case "G8611-64002":
                    case "G8611-64003":
                    case "G8611-64004":
                    case "G8611-64005":
                    case "G8612-64002":
                    case "G8612-64003":
                    case "G8612-64004":
                    case "G8612-64005":
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public static Boolean GetModel_Vgain(string modelnumber, double vgain)
        {          
            try
            {
                Boolean status = false;

                if(modelnumber.Contains("PR02") || modelnumber.Contains("PD03"))
                {
                    if (vgain >= 18 && vgain <= 120)
                        status = true;
                    else
                        throw new Exception("The calculate vgain = " + vgain + ", is out of acceptable range (18~120).");                
                }

                else if(modelnumber.Contains("MR15") || modelnumber.Contains("BR15"))
                {
                    if(vgain >= 18 && vgain <= 55)
                        status = true;
                    else
                        throw new Exception("The calculate vgain = " + vgain + ", is out of acceptable range. (18~55)");
                }

                else if(modelnumber.Contains("MD30") || modelnumber.Contains("BD30") || modelnumber == "G8601-60004" || modelnumber == "G8601-60005" || modelnumber == "G8602-60004" || modelnumber == "G8602-60005")
                {
                    if (vgain >= 18 && vgain <= 35)
                        status = true;
                    else
                        throw new Exception("The calculate vgain = " + vgain + ", is out of acceptable range. (18~35)");
                }
                
                return status;               
            }

            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This method will return Vgain limits for each LD pump type and voltage type
        /// </summary>
        /// <param name="modelNumber">Part number of LD</param>
        /// <param name="limitsArray">Limits array from plugin sequence test params</param>
        /// <returns>VGain limits in comma seperated value string</returns>
        public static string GetVgainLimit(string modelNumber, string[] limitsArray)
        {
            try
            {
                // first step, get voltage for this part number
                int voltage = GetLDVoltage(modelNumber);
                string VGainLimits1A = limitsArray[0];      // 110V Portable LD (IDP3, DS40, DS40M)
                string VGainLimits1B = limitsArray[1];      // 220V Portable LD (IDP3, DS40, DS40M)
                string VGainLimits2A = limitsArray[2];      // 110V Mobile/Bench (DS302, DS602)
                string VGainLimits2B = limitsArray[3];      // 220V Mobile/Bench (DS302, DS602)
                string VGainLimits3A = limitsArray[4];      // 110V Mobile/Bench (TS620, IDP15)
                string VGainLimits3B = limitsArray[5];      // 220V Mobile/Bench (TS620, IDP15)

                string VGainLimit = "";

                // case selection for different type of voltage
                switch (voltage)
                {
                    case 110:
                        {
                            // selection for LD part number
                            switch (modelNumber)
                            {
                                // Portable LD
                                case "VSPR021":
                                case "VSPD031":
                                case "G8610-64000": // start London
                                case "G8610-64002":
                                case "G8610-64003":
                                    {
                                        VGainLimit = VGainLimits1A;
                                    }
                                    break;
                                // Mobile or Bench DS302 DS602
                                case "VSMR151":
                                case "VSBR151":
                                case "G8611-64000": // start London
                                case "G8612-64000":
                                    {
                                        VGainLimit = VGainLimits2A;
                                    }
                                    break;
                                // Mobile or Bench TS620 IDP15
                                case "G8601-64004":
                                case "G8602-64004":
                                case "VSMD301":
                                case "VSBD301":
                                case "G8611-64002": // start London
                                case "G8611-64004":
                                case "G8612-64002":
                                case "G8612-64004":
                                    {
                                        VGainLimit = VGainLimits3A;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case 220:
                        {
                            switch (modelNumber)
                            {
                                // Portable 220 LD
                                case "VSPR022":
                                case "VSPD032":
                                case "G8610-64001": // start London
                                case "G8610-64004":
                                    {
                                        VGainLimit = VGainLimits1B;
                                    }
                                    break;
                                // Mobile or Bench DS302 DS602
                                case "VSMR152":
                                case "VSBR152":
                                case "G8611-64001": // start London
                                case "G8612-64001":
                                case "G8611-64006":
                                case "MSPLL10779":
                                case "MSPLL10767":
                                case "G8612-64006":
                                    {
                                        VGainLimit = VGainLimits2B;
                                    }
                                    break;
                                // Mobile or Bench TS620 IDP15
                                case "G8601-64005":
                                case "G8602-64005":
                                case "VSMD302":
                                case "VSBD302":
                                case "G8611-64003": // start London
                                case "G8611-64005":
                                case "G8612-64003":
                                case "G8612-64005":
                                    {
                                        VGainLimit = VGainLimits3B;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        //Trace.WriteLine(myLD.iteSlot + "Unable to get Vgain limit for " + modelNumber);
                        break;
                }

                // Return the VGain Limit (comma seperated value)
                return VGainLimit;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This method will retunr LD voltage rating for specifica part number provided
        /// </summary>
        /// <param name="LDPartNumber">Part Number of the LD</param>
        /// <returns>voltage of the LD</returns>
        public static int GetLDVoltage(string LDPartNumber)
        {
            try
            {
                int voltage = 0;
                switch (LDPartNumber)
                {
                    //100-120V - VS Units
                    case "VSBD301":
                    case "VSBR151":
                    case "VSPD030":
                    case "VSMD301":
                    case "VSPD031":
                    case "VSPR021":
                    case "VSMR151":
                    case "G8601-64004":
                    case "G8602-64004":
                    //100-120V - London Units
                    case "G8610-64000":
                    case "G8610-64002":
                    case "G8610-64003":
                    case "G8611-64000":
                    case "G8611-64002":
                    case "G8611-64004":
                    case "G8612-64000":
                    case "G8612-64002":
                    case "G8612-64004":
                        {
                            voltage = 110;
                        }
                        break;
                    //220-240V - VS Units
                    case "VSBD302":
                    case "VSPR022":
                    case "VSPD032":
                    case "VSMR152":
                    case "VSMD302":
                    case "VSBR152":
                    case "G8601-64005":
                    case "G8602-64005":
                    case "MSPLL10779":
                    case "MSPLL10767":
                    //220-240V - London Units
                    case "G8610-64001":
                    case "G8610-64004":
                    case "G8611-64001":
                    case "G8611-64003":
                    case "G8611-64005":
                    case "G8612-64001":
                    case "G8612-64003":
                    case "G8612-64005":
                    case "G8611-64006":
                    case "G8612-64006":
                        {
                            voltage = 220;
                        }
                        break;
                    default:
                        voltage = 0;
                        break;
                }

                return voltage;
            }
            catch (Exception)
            {

                throw;
            }
        }
              

        //@@ Send command and standby for a return value, then proceed with verification by checking the return value @@//

        public static Boolean DoThis(VSLeakDetector myLD, ref TestInfo myTestInfo, string command, string retval, int i, string result)
        {
            Boolean Status = false;
            int check = 1;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
                //myLD.Open();
                recheck:
                myLD.Write(command);
                String Retval = myLD.Read();

                //if the return value obtained is correct, the test point will passed.
                if (Retval.Contains(retval))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");

                    myTestInfo.ResultsParams[i].Result = result;
                    Status = true;
                }
                else
                {
                    if (check < 3)
                    { check++;Thread.Sleep(1000); Retval = null; goto recheck; }         
                    else
                    myTestInfo.ResultsParams[i].Result = "FAILED "+command ;
                    //if (myLD != null) myLD.Close();
                   // throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Status;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }


        //@@ Send command together with specified terminator and standby for a return value, then proceed with verification by checking the return value @@/

        public static Boolean DoThis(VSLeakDetector myLD, ref TestInfo myTestInfo, string command, string retval, int i, string result, string terminator)
        {
            Boolean Status = false;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
               

                //myLD.Open();
                myLD.Terminator = terminator;

                myLD.Write(command);

                String Retval = myLD.Read();

                //if the return value obtained is correct, the test point will passed.
                if (Retval.Contains(retval))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");

                    myTestInfo.ResultsParams[i].Result = result;
                    Status = true;
                }
                else
                {                  
                    myTestInfo.ResultsParams[i].Result = "FAILED";
                    //if (myLD != null) myLD.Close();
                    throw new Exception("Error while sending command: " + command);
                }

               // myLD.Close();
                return Status;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }


        //@@ Send command together with specified timeout limit and standby for a return value, then proceed with verification by checking the return value @@/
        
        public static Boolean DoThis(VSLeakDetector myLD, ref TestInfo myTestInfo, string command, string retval, int i, string result, int timeout)
        {
            Boolean Status = false;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
                

                //myLD.Open();
                myLD.Timeout = timeout;

                myLD.Write(command);

                String Retval = myLD.Read();

                //if the return value obtained is correct, the test point will passed.
                if (Retval.Contains(retval))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");

                    myTestInfo.ResultsParams[i].Result = result;
                    Status = true;
                }
                else
                {                   
                    myTestInfo.ResultsParams[i].Result = "FAILED";
                    //if (myLD != null) myLD.Close();
                    throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Status;
            }

            catch (Exception)
            {
               // myLD.Close();
                throw;
            }
        }


        //@@ Send command together with specified terminator and timeout limit and standby for a return value, then proceed with verification by checking the return value @@/

        public static Boolean DoThis(VSLeakDetector myLD, ref TestInfo myTestInfo, string command, string retval, int i, string result, string terminator, int timeout)
        {
            Boolean Status = false;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
                

                //myLD.Open();
                myLD.Terminator = terminator;
                myLD.Timeout = timeout;

                myLD.Write(command);

                string Retval = myLD.Read();

                //if the return value obtained is correct, the test point will passed.
                if (Retval.Contains(retval))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");

                    myTestInfo.ResultsParams[i].Result = result;
                    Status = true;
                }
                else
                {                  
                    myTestInfo.ResultsParams[i].Result = "FAILED";
                    //if (myLD != null) myLD.Close();
                    throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Status;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }

        public static Boolean DoThis(VSLeakDetector myLD, ref TestInfo myTestInfo, string command, string[] retvals, int i, string result)
        {
            Boolean Status = false;
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
                
                
                //myLD.Open();
                myLD.Write(command);

                String Retval = myLD.Read();

                //if the return value obtained is correct, the test point will passed.
                if (retvals.All(Retval.Contains))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");

                    myTestInfo.ResultsParams[i].Result = result;
                    Status = true;
                }
                else
                {
                    myTestInfo.ResultsParams[i].Result = "FAILED";
                    //if (myLD != null) myLD.Close();
                    throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Status;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }

        //@@ Function for query @@//
        public static string SendCommand(VSLeakDetector myLD, ref Boolean status, string command, string retval)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {

                int check=1;
                recheck:
                //myLD.Open();
                myLD.Write(command);

                string Retval = myLD.Read();

                if (Retval.Contains(retval))
                {
                    status = true;
                }
                else
                {
                    if (check < 4)
                    {
                        check++;
                        Thread.Sleep(1000);
                        Retval = null;
                        goto recheck;
                    }
                    status = false;
                    //if (myLD != null) myLD.Close();
                  //  throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Retval;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }

        //@@ Function for query with one additional return value as verification @@//
        public static string SendCommand(VSLeakDetector myLD, ref Boolean status, string command, string retval, string retval2)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            try
            {
                

                //myLD.Open();
                myLD.Write(command);

                string Retval = myLD.Read();

                if (Retval.Contains(retval) && Retval.Contains(retval2))
                {
                    status = true;
                }
                else
                {
                    status = false;
                    //if (myLD != null) myLD.Close();
                    throw new Exception("Error while sending command: " + command);
                }

                //myLD.Close();
                return Retval;
            }

            catch (Exception)
            {
                //myLD.Close();
                throw;
            }
        }


        //@@ Function when test failed to proceed @@//

        public static void Fail_Test(ref TestInfo myTestInfo, string msg_exception, int step)
        {
           Test_Failed myTest_Failed = new Test_Failed();

        Back:

            myTest_Failed.Testgroup = myTestInfo.TestLabel;
            myTest_Failed.Testpoint = myTestInfo.ResultsParams[step].Label;
            myTest_Failed.Message_Exception = msg_exception;

            myTest_Failed.ShowDialog();

            if (myTest_Failed.DialogResult == DialogResult.Abort)
            {
                myTest_Failed.Dispose();
                Application.Exit();
            }

            else if (myTest_Failed.DialogResult == DialogResult.Cancel)
                goto Back;          
        }


        //@@ User prompt @@//

        public static string LabelDisplay(string display)
        {
            userprompt my_userprompt = new userprompt();
            my_userprompt.StartPosition = FormStartPosition.Manual;
            my_userprompt.Location = new System.Drawing.Point(locX, locY);

        X:
            my_userprompt.DesLabel = display;

            my_userprompt.ShowDialog();

            if (my_userprompt.DialogResult == DialogResult.OK)
            {
                display = my_userprompt.User_Input;
                my_userprompt.Close();
            }

            else if (my_userprompt.DialogResult == DialogResult.Abort)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to abort the entire testing procedure?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_userprompt.Dispose();
                }

                else if (result == DialogResult.No)
                {
                    goto X;
                }
            }
            return display;
        }


        //@@ Press ZERO if UUT stuck in CONTRAFLOW @@//

        public static bool Stuck_Contra(VSLeakDetector myLD)
        {
            try
            {
                //VSLeakDetector myLD = new VSLeakDetector(comPort);
                //myLD.iteSlot = iteSlot;
                Boolean status = false;
                string retval;
                int IsStuck = 1;

                //myLD.Open();

                while (IsStuck > 0)
                {
                    repeat:
                    myLD.Write("?VALVESTATE");
                    retval = myLD.Read();

                    if (retval.Contains("CONTRAFLOW"))
                    {
                        IsStuck++;
                        Thread.Sleep(1000);
                        goto repeat;
                    }

                    if (retval.Contains("MIDSTAGE"))
                    {
                        status = true;
                        break;
                    }

                    if (IsStuck > 100)
                    {
                        myLD.Write("ZERO");
                        status = true;
                        Thread.Sleep(60000);

                        break;
                    }
                }

                //myLD.Close();
                return status;
            }

            catch (Exception)
            {

                throw;
            }
        }

        //@@ Auto recalibrate when the initial calibration fails. Seq 5.11 @@//

        public static bool Recalibrate(VSLeakDetector myLD)
        {
            try
            {
                //VSLeakDetector myLD = new VSLeakDetector(comPort);
                //myLD.iteSlot = iteSlot;
                Boolean status = false;

                /*@@ Initiates a Full or Fast calibration depending on system settings. The CPU software tunes,
                then adjusts the gain so that the current helium signal causes the current leak rate measurement
                to be the same as the most recently input using INIT-STDLEAK. If the gain is 2.9 or higher, a normal 
                calibration is performed. Success is indicated by the normal ok response. @@*/

                Trace.WriteLine(myLD.iteSlot + "Calibrating...");
               // myLD.Open();
                myLD.Write("CALIBRATE");

                string retval = myLD.Read();

                if(retval.Contains("ok"))
                {
                    //@@ Wait for 979 Calibration VI @@//

                    Trace.WriteLine(myLD.iteSlot + "Wait for system to enter FINETEST mode...");

                    int timeout = 1;
                    status = false;

                    while (timeout > 0)
                    {
                        myLD.Write("?VALVESTATE");

                        retval = myLD.Read();

                        if (retval.Contains("MIDSTAGE") && retval.Contains("ok"))
                        {
                            //@@ Report and verify the status of the last calibration. @@//

                            Trace.WriteLine(myLD.iteSlot + "Report and verify the status of the last calibration...");

                            myLD.Write("?CALOK");
                            retval = myLD.Read();

                            if (retval.Contains("Yesok"))
                            {
                                status = true;
                                break;
                            }
                            
                        }

                        if (timeout > 300)
                        {
                            status = false;
                            break;
                        }

                        Thread.Sleep(1000);
                        timeout++;
                    }
                }

                //myLD.Close();
                return status;
            }

            catch (Exception)
            {
                
                throw;
            }
        }

        public static Boolean Reload(VSLeakDetector myLD)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;
            //myLD.Open();
            myLD.Terminator = "\r\n";
            myLD.Write("RELOAD");
            //Thread.Sleep(5000);
            Thread.Sleep(1000);
            Boolean status = false;
            string retval;
            int timeout = 1;

            while (timeout > 0)
            {
                retval = myLD.Read();
                Trace.WriteLine(timeout);
                if (retval.Contains("Agilent"))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");
                    Trace.WriteLine(timeout);
                    status = true;
                    break;
                }

                if (timeout > 15)
                {
                    //myLD.Close();
                    throw new Exception("Test point failed.");
                }

                Thread.Sleep(1000);
                timeout++;
            }

           // myLD.Close();
            return status;
        }

        public static Boolean Wait_SystemReady(VSLeakDetector myLD)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;

            //myLD.Open();
            Boolean status = false;
            string retval;
            int timeout = 1;

            while (timeout > 0)
            {
                myLD.Write("SystemReady .");
                retval = myLD.Read();

                if (retval.Contains("-1 ok"))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");
                    status = true;
                    break;
                }

                if (timeout > 360)
                {
                    //myLD.Close();
                    //throw new Exception("Test point failed.");
                    Trace.WriteLine(myLD.iteSlot + "Test point failed.");
                    status = false;
                }

                Thread.Sleep(1000);
                timeout++;
            }

            //myLD.Close();
            return status;
        }

        public static Boolean Wait_FineTest(VSLeakDetector myLD)
        {
            try
            {
                return Wait_FineTest(myLD, 300);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Boolean Wait_FineTest(VSLeakDetector myLD, int timeoutLimit)
        {           
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;

           // myLD.Open();
            Boolean status = false;
            int timeout = 1;
            string retval;

            Trace.WriteLine(myLD.iteSlot + "Wait for system to enter FINETEST mode...");

            while (timeout > 0)
            {
                myLD.Write("?VALVESTATE");
                retval = myLD.Read();

                if (retval.Contains("MIDSTAGE") && retval.Contains("ok"))
                {
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");
                    status = true;
                    break;
                }

                if (timeout > timeoutLimit)
                {
                    //myLD.Close();
                    return false;
                }

                Thread.Sleep(1000);
                timeout++;
            }

           // myLD.Close();
            return status;
        }

        public static Boolean Wait_Test(VSLeakDetector myLD)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;

            //myLD.Open();
            Boolean status = false;
            int timeout = 1;
            string retval;

            Trace.WriteLine(myLD.iteSlot + "Wait for system to enter TEST mode...");

            while (timeout > 0)
            {
                myLD.Write("?VALVESTATE");

                retval = myLD.Read();

                if (retval.Contains("CONTRAFLOW") && retval.Contains("ok"))
                {                   
                    Trace.WriteLine(myLD.iteSlot + "Test point complete.");
                    status = true;
                    break;
                }

                if (timeout > 60)
                {
                    //myLD.Close();
                    throw new Exception("Test point failed.");
                }

                Thread.Sleep(1000);
                timeout++;
            }

           // myLD.Close();
            return status;
        }
    }
}
