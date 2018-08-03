using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using CustomFormLibrary;
using SerialPortIO;
using PluginSequence;
using System.Diagnostics;

namespace VSLDtest.SubGroupTest
{
    /*
     * In Future, the IO board testing can be fully automated if we buy Keysight 34907A module. This module can be slotted in at slot#3 of the mainframe 34970A.
     * The module is multifunction card that can set digital in and out 
     */
    public class Seq5_12
    {
        private static double stdleak;
        public static int step = 1;
        internal static string iteSlot;

        public static bool IsWirelessEnabled { get; set; }
        public static bool IsIOEnabled { get; set; }

        public static int  locX { get; set; }
        public static int locY { get; set; }

        public static double Stdleak
        {
            get { return stdleak; }
            set { stdleak = value; }
        }


        public static TestInfo DoSeq5_12(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.12.1 Test_IOBoard (Opt.)":
                        {
                            if (IsIOEnabled)
                            {
                                DoIOBoardTest(myLD, ref myTestInfo, ref myuutdata);
                            }
                            else
                            {
                                step = 1;
                                while (step <= myTestInfo.ResultsParams.NumResultParams)
                                {
                                    myTestInfo.ResultsParams[step].SpecMax = "Skip";
                                    myTestInfo.ResultsParams[step].SpecMin = "Skip";
                                    myTestInfo.ResultsParams[step].Nominal = "Skip";
                                    myTestInfo.ResultsParams[step].Result = "Skip";

                                    step++;
                                }
                            }
                        }
                        break;
                    case "5.12.2 Wireless_Remote (Opt.)":
                        {
                            if (IsWirelessEnabled)
                            {
                                DoWirelessTest(myLD, ref myTestInfo, ref myuutdata);
                            }
                            else
                            {
                                step = 1;
                                while (step <= myTestInfo.ResultsParams.NumResultParams)
                                {
                                    myTestInfo.ResultsParams[step].SpecMax = "Skip";
                                    myTestInfo.ResultsParams[step].SpecMin = "Skip";
                                    myTestInfo.ResultsParams[step].Nominal = "Skip";
                                    myTestInfo.ResultsParams[step].Result = "Skip";

                                    step++;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

                //if (mystartupform.Customer_opt == "IO Board")
                //{
                //    IOBoard(ref myTestInfo, ref myuutdata);
                //}

                //else if (mystartupform.Customer_opt == "Wireless Remote")
                //{
                //    Wireless_Remote(ref myTestInfo, ref myuutdata);
                //}

                //else if( mystartupform.Customer_opt == "Both")
                //{
                //    Both(ref myTestInfo, ref myuutdata);
                //}

                //else
                //{
                //    None(ref myTestInfo, ref myuutdata);
                //}
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Helper.Fail_Test(ref myTestInfo, ex.Message, step);
                throw;
            }
            
            return myTestInfo;
        }

        public static void DoIOBoardTest(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            try
            {
                Boolean status = false;
                string retval;

                //Access to full command 

                Trace.WriteLine(iteSlot + "Access to full command...");
                Helper.SendCommand(myLD, ref status, "XYZZY", "XYZZY ok");

                //Obtain stdleak rate

                Trace.WriteLine(iteSlot + "Obtain the stdleak rate...");
                retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                if (status == true)
                {
                    string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                    Stdleak = Convert.ToDouble(response[0]);
                }


                //lock DMM from other test sockets while it is being used

                int checking = 1;
                string conStr = myTestInfo.TestParams[1].Value;

                Trace.WriteLine(iteSlot + "Check before Locking the DMM...");

                while (checking > 0)
                {
                    string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                    if (DMMstatus == "No")
                    {
                        SQL_34970A.Update_DMM_Status(myuutdata.Options, "Yes", conStr);    //Yes = Lock     No = Unlock
                        break;
                    }

                    Thread.Sleep(5000);
                }


                //@@ Prompt user to turn off the leak detector and attach the cabbling @@//

                step++;


                //@@ Prompt user to turn on the unit @@//

                step++;


                //@@ Sets and leaves the Parallel_Enable_In value high @@//

                step++;


                Trace.WriteLine(iteSlot + "Startup wait...");
                Thread.Sleep(30000);


                //@@ Sends ?IOBOARD command to the unit @@//

                step++;


                //@@ Wait for the Ready_out line value to go HIGH @@//

                step++;


                //@@ Sends VENT bit on the appropriate output port. @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(10000);


                //@@ Reads VENT_out bit on the appropriate input port. @@//

                step++;


                Thread.Sleep(5000);


                //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E-3 INIT-CL-XFER", "ok", step, "ok");
                step++;


                //@@ Set Test_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(3000);


                //@@ Verify Start_Out @@//

                step++;


                //@@ Verify Test_Out @@//

                step++;


                //@@ Wait for the START_OUT line value to go LOW @@//

                step++;


                //@@ Verify Test_Out @@//

                step++;


                //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E+1 INIT-CL-XFER", "ok", step, "ok");
                step++;


                //@@ Set RDSTDLK_IN High @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(1000);


                //@@ Check STD valve state.vi @@//

                step++;


                //@@ Set RDSTDLK_IN low @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(1000);


                //@@ Check STD valve state.vi @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(8000);


                //@@ Set Hold_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(1000);


                //@@ Verify Hold_Out @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(5000);


                //@@ Set Test_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(15000);


                //@@ Set Zero_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(10000);


                //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                Helper.DoThis(myLD, ref myTestInfo, "1 INIT-QUICK-CAL", "ok", step, "ok");
                step++;


                //@@ Set CAL_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(2000);


                //@@ Verify CAL_Out @@//

                step++;


                //@@ Wait for CAL_Out line value to go LOW @@//

                step++;


                //@@ Verify CAL_Out_ok @@//

                step++;


                //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                Helper.DoThis(myLD, ref myTestInfo, "0 INIT-QUICK-CAL", "ok", step, "ok");
                step++;


                //@@ Set AUTO_Manual_Range_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(2000);


                //@@ Sends .AUTOMAN command to the unit @@//

                Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "manual ok", step, "ok");
                step++;


                //@@ Set AUTO_Manual_Range_In @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(2000);


                //@@ Sends .AUTOMAN command to the unit @@//

                Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "automatic ok", step, "ok");
                step++;


                //@@ Sets the leak rate value for Reject Set Point #1. The helium leak rate value is in atm cc/sec. @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-1REJECT", "ok", step, "ok");
                step++;


                //@@ Sets the leak rate value for Reject Set Point #2. The helium leak rate value is in atm cc/sec. @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-2REJECT", "ok", step, "ok");
                step++;


                //@@ Sets the leak rate value for Reject Set Point #3. The helium leak rate value is in atm cc/sec. @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E-00 INIT-3REJECT", "ok", step, "ok");
                step++;


                //@@ Sets the leak rate value for Reject Set Point #4. The helium leak rate value is in atm cc/sec. @@//

                Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-4REJECT", "ok", step, "ok");
                step++;


                //@@ Pressure reject set point #1. @@//

                Helper.DoThis(myLD, ref myTestInfo, "LOW-1REJECT", "ok", step, "ok");
                step++;


                //@@ Pressure reject set point #2. @@//

                Helper.DoThis(myLD, ref myTestInfo, "LOW-2REJECT", "ok", step, "ok");
                step++;


                //@@ Pressure reject set point #3. @@//

                Helper.DoThis(myLD, ref myTestInfo, "LOW-3REJECT", "ok", step, "ok");
                step++;


                //@@ Pressure reject set point #4. @@//

                Helper.DoThis(myLD, ref myTestInfo, "LOW-4REJECT", "ok", step, "ok");
                step++;


                //@@ Turn ON reject set point #1 @@//

                Helper.DoThis(myLD, ref myTestInfo, "ENABLE-1REJECT", "ok", step, "ok");
                step++;


                //@@ Turn ON reject set point #2 @@//

                Helper.DoThis(myLD, ref myTestInfo, "ENABLE-2REJECT", "ok", step, "ok");
                step++;


                //@@ Turn ON reject set point #3 @@//

                Helper.DoThis(myLD, ref myTestInfo, "ENABLE-3REJECT", "ok", step, "ok");
                step++;


                //@@ Turn ON reject set point #4 @@//

                Helper.DoThis(myLD, ref myTestInfo, "ENABLE-4REJECT", "ok", step, "ok");
                step++;


                //@@ Set VENT_IN @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                Thread.Sleep(10000);


                //@@ Set TEST_IN @@//

                step++;


                Trace.WriteLine(iteSlot + "Waits for system leak rate to go below 8.0E-09...");
                Thread.Sleep(35000);


                //@@ Verify REJECT1_OUT @@//

                step++;


                //@@ Verify REJECT2_OUT @@//

                step++;


                //@@ Verify REJECT3_OUT @@//

                step++;


                //@@ Verify REJECT4_OUT @@//

                step++;


                //@@ Turn OFF reject set point #1 @@//

                Helper.DoThis(myLD, ref myTestInfo, "DISABLE-1REJECT", "ok", step, "ok");
                step++;


                //@@ Turn OFF reject set point #2 @@//

                Helper.DoThis(myLD, ref myTestInfo, "DISABLE-2REJECT", "ok", step, "ok");
                step++;


                //@@ Turn OFF reject set point #3 @@//

                Helper.DoThis(myLD, ref myTestInfo, "DISABLE-3REJECT", "ok", step, "ok");
                step++;


                //@@ Turn OFF reject set point #4 @@//

                Helper.DoThis(myLD, ref myTestInfo, "DISABLE-4REJECT", "ok", step, "ok");
                step++;


                //@@ Set PARALLEL_ENABLE_IN @@//

                step++;


                //Lorcfffdervvck DIO



                //@@ Prompt user to disconnect the I/O tester @@//

                step++;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void DoWirelessTest(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }


        // We are not using below functions anymore

        public static void IOBoard(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval;

            switch (myTestInfo.TestLabel)
            {
                case "5.12.1 Test_IOBoard (Opt.)":
                    {
                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            //Access to full command 

                            Trace.WriteLine(iteSlot + "Access to full command...");
                            Helper.SendCommand(myLD, ref status, "XYZZY", "XYZZY ok");

                            //Obtain stdleak rate

                            Trace.WriteLine(iteSlot + "Obtain the stdleak rate...");
                            retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                Stdleak = Convert.ToDouble(response[0]);
                            }


                            //lock DMM from other test sockets while it is being used

                            int checking = 1;
                            string conStr = myTestInfo.TestParams[1].Value;

                            Trace.WriteLine(iteSlot + "Check before Locking the DMM...");

                            while (checking > 0)
                            {
                                string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                                if (DMMstatus == "No")
                                {
                                    SQL_34970A.Update_DMM_Status(myuutdata.Options, "Yes", conStr);    //Yes = Lock     No = Unlock
                                    break;
                                }

                                Thread.Sleep(5000);
                            }


                            //@@ Prompt user to turn off the leak detector and attach the cabbling @@//

                            step++;


                            //@@ Prompt user to turn on the unit @@//

                            step++;


                            //@@ Sets and leaves the Parallel_Enable_In value high @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Startup wait...");
                            Thread.Sleep(30000);


                            //@@ Sends ?IOBOARD command to the unit @@//

                            step++;


                            //@@ Wait for the Ready_out line value to go HIGH @@//

                            step++;


                            //@@ Sends VENT bit on the appropriate output port. @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Reads VENT_out bit on the appropriate input port. @@//

                            step++;


                            Thread.Sleep(5000);


                            //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-3 INIT-CL-XFER", "ok", step, "ok");
                            step++;


                            //@@ Set Test_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(3000);


                            //@@ Verify Start_Out @@//

                            step++;


                            //@@ Verify Test_Out @@//

                            step++;


                            //@@ Wait for the START_OUT line value to go LOW @@//

                            step++;


                            //@@ Verify Test_Out @@//

                            step++;


                            //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E+1 INIT-CL-XFER", "ok", step, "ok");
                            step++;


                            //@@ Set RDSTDLK_IN High @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Check STD valve state.vi @@//

                            step++;


                            //@@ Set RDSTDLK_IN low @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Check STD valve state.vi @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(8000);


                            //@@ Set Hold_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Verify Hold_Out @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(5000);


                            //@@ Set Test_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(15000);


                            //@@ Set Zero_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1 INIT-QUICK-CAL", "ok", step, "ok");
                            step++;


                            //@@ Set CAL_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Verify CAL_Out @@//

                            step++;


                            //@@ Wait for CAL_Out line value to go LOW @@//

                            step++;


                            //@@ Verify CAL_Out_ok @@//

                            step++;


                            //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "0 INIT-QUICK-CAL", "ok", step, "ok");
                            step++;


                            //@@ Set AUTO_Manual_Range_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Sends .AUTOMAN command to the unit @@//

                            Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "manual ok", step, "ok");
                            step++;


                            //@@ Set AUTO_Manual_Range_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Sends .AUTOMAN command to the unit @@//

                            Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "automatic ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #1. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #2. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #3. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-00 INIT-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #4. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #1. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #2. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #3. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #4. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #1 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #2 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #3 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #4 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Set VENT_IN @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Set TEST_IN @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for system leak rate to go below 8.0E-09...");
                            Thread.Sleep(35000);


                            //@@ Verify REJECT1_OUT @@//

                            step++;


                            //@@ Verify REJECT2_OUT @@//

                            step++;


                            //@@ Verify REJECT3_OUT @@//

                            step++;


                            //@@ Verify REJECT4_OUT @@//

                            step++;


                            //@@ Turn OFF reject set point #1 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #2 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #3 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #4 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Set PARALLEL_ENABLE_IN @@//

                            step++;


                            //Lorcfffdervvck DIO



                            //@@ Prompt user to disconnect the I/O tester @@//

                            step++;
                        }

                        break;
                    }

                case "5.12.2 Wireless_Remote (Opt.)":
                    {
                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            myTestInfo.ResultsParams[step].SpecMax = "Skip";
                            myTestInfo.ResultsParams[step].SpecMin = "Skip";
                            myTestInfo.ResultsParams[step].Nominal = "Skip";
                            myTestInfo.ResultsParams[step].Result  = "Skip";

                            step++;
                        }

                        break;
                    }
            }     
        }

        public static void Wireless_Remote(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            switch (myTestInfo.TestLabel)
            {
                case "5.12.1 Test_IOBoard (Opt.)":
                    {
                        while(step <= myTestInfo.ResultsParams.NumResultParams)
                        {                           
                            myTestInfo.ResultsParams[step].SpecMax = "Skip";
                            myTestInfo.ResultsParams[step].SpecMin = "Skip";
                            myTestInfo.ResultsParams[step].Nominal = "Skip";
                            myTestInfo.ResultsParams[step].Result  = "Skip";

                            step++;
                        }

                        break;
                    }

                case "5.12.2 Wireless_Remote (Opt.)	":
                    {
                        step = 1;

                        while(step <= myTestInfo.ResultsParams.NumResultParams)
                        {

                        }

                        break;
                    }
            }
        }

        public static void Both(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string retval;

            switch (myTestInfo.TestLabel)
            {
                case "5.12.1 Test_IOBoard (Opt.)":
                    {
                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            //Access to full command 

                            Trace.WriteLine(iteSlot + "Access to full command...");
                            Helper.SendCommand(myLD, ref status, "XYZZY", "XYZZY ok");

                            //Obtain stdleak rate

                            Trace.WriteLine(iteSlot + "Obtain the stdleak rate...");
                            retval = Helper.SendCommand(myLD, ref status, "?STDLEAK", "ok");

                            if (status == true)
                            {
                                string[] response = retval.Split(new string[] { "?STDLEAK ", "ok" }, StringSplitOptions.RemoveEmptyEntries);
                                Stdleak = Convert.ToDouble(response[0]);
                            }


                            //lock DMM from other test sockets while it is being used

                            int checking = 1;
                            string conStr = myTestInfo.TestParams[1].Value;
                            Trace.WriteLine(iteSlot + "Check before Locking the DMM...");

                            while (checking > 0)
                            {
                                string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                                if (DMMstatus == "No")
                                {
                                    SQL_34970A.Update_DMM_Status(myuutdata.Options, "Yes", conStr);    //Yes = Lock     No = Unlock
                                    break;
                                }

                                Thread.Sleep(5000);
                            }


                            //@@ Prompt user to turn off the leak detector and attach the cabbling @@//

                            step++;


                            //@@ Prompt user to turn on the unit @@//

                            step++;


                            //@@ Sets and leaves the Parallel_Enable_In value high @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Startup wait...");
                            Thread.Sleep(30000);


                            //@@ Sends ?IOBOARD command to the unit @@//

                            step++;


                            //@@ Wait for the Ready_out line value to go HIGH @@//

                            step++;


                            //@@ Sends VENT bit on the appropriate output port. @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Reads VENT_out bit on the appropriate input port. @@//

                            step++;


                            Thread.Sleep(5000);


                            //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-3 INIT-CL-XFER", "ok", step, "ok");
                            step++;


                            //@@ Set Test_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(3000);


                            //@@ Verify Start_Out @@//

                            step++;


                            //@@ Verify Test_Out @@//

                            step++;


                            //@@ Wait for the START_OUT line value to go LOW @@//

                            step++;


                            //@@ Verify Test_Out @@//

                            step++;


                            //@@ Sets the value of the contra-flow mode crossover pressure in Torr @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E+1 INIT-CL-XFER", "ok", step, "ok");
                            step++;


                            //@@ Set RDSTDLK_IN High @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Check STD valve state.vi @@//

                            step++;


                            //@@ Set RDSTDLK_IN low @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Check STD valve state.vi @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(8000);


                            //@@ Set Hold_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(1000);


                            //@@ Verify Hold_Out @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(5000);


                            //@@ Set Test_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(15000);


                            //@@ Set Zero_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1 INIT-QUICK-CAL", "ok", step, "ok");
                            step++;


                            //@@ Set CAL_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Verify CAL_Out @@//

                            step++;


                            //@@ Wait for CAL_Out line value to go LOW @@//

                            step++;


                            //@@ Verify CAL_Out_ok @@//

                            step++;


                            //@@ Sets which type of calibration will be run if the CALIBRATE command is initiated.  Preceded by 0 or 1, 0 for FULL cal, 1 for FAST cal. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "0 INIT-QUICK-CAL", "ok", step, "ok");
                            step++;


                            //@@ Set AUTO_Manual_Range_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Sends .AUTOMAN command to the unit @@//

                            Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "manual ok", step, "ok");
                            step++;


                            //@@ Set AUTO_Manual_Range_In @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(2000);


                            //@@ Sends .AUTOMAN command to the unit @@//

                            Helper.DoThis(myLD, ref myTestInfo, ".AUTOMAN", "automatic ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #1. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #2. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #3. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-00 INIT-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Sets the leak rate value for Reject Set Point #4. The helium leak rate value is in atm cc/sec. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "1.0E-08 INIT-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #1. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #2. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #3. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Pressure reject set point #4. @@//

                            Helper.DoThis(myLD, ref myTestInfo, "LOW-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #1 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #2 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #3 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn ON reject set point #4 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "ENABLE-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Set VENT_IN @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for unit to respond to new DI value...");
                            Thread.Sleep(10000);


                            //@@ Set TEST_IN @@//

                            step++;


                            Trace.WriteLine(iteSlot + "Waits for system leak rate to go below 8.0E-09...");
                            Thread.Sleep(35000);


                            //@@ Verify REJECT1_OUT @@//

                            step++;


                            //@@ Verify REJECT2_OUT @@//

                            step++;


                            //@@ Verify REJECT3_OUT @@//

                            step++;


                            //@@ Verify REJECT4_OUT @@//

                            step++;


                            //@@ Turn OFF reject set point #1 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-1REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #2 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-2REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #3 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-3REJECT", "ok", step, "ok");
                            step++;


                            //@@ Turn OFF reject set point #4 @@//

                            Helper.DoThis(myLD, ref myTestInfo, "DISABLE-4REJECT", "ok", step, "ok");
                            step++;


                            //@@ Set PARALLEL_ENABLE_IN @@//

                            step++;


                            //Lorcfffdervvck DIO



                            //@@ Prompt user to disconnect the I/O tester @@//

                            step++;
                        }

                        break;
                    }

                case "5.12.2 Wireless_Remote (Opt.)":
                    {
                        step = 1;

                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            

                        }

                        break;
                    }
            }
        }

        public static void None(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            switch (myTestInfo.TestLabel)
            {
                case "5.12.1 Test_IOBoard (Opt.)":
                    {
                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            myTestInfo.ResultsParams[step].SpecMax = "Skip";
                            myTestInfo.ResultsParams[step].SpecMin = "Skip";
                            myTestInfo.ResultsParams[step].Nominal = "Skip";
                            myTestInfo.ResultsParams[step].Result  = "Skip";

                            step++;
                        }

                        break;
                    }

                case "5.12.2 Wireless_Remote (Opt.)":
                    {
                        while (step <= myTestInfo.ResultsParams.NumResultParams)
                        {
                            myTestInfo.ResultsParams[step].SpecMax = "Skip";
                            myTestInfo.ResultsParams[step].SpecMin = "Skip";
                            myTestInfo.ResultsParams[step].Nominal = "Skip";
                            myTestInfo.ResultsParams[step].Result  = "Skip";

                            step++;
                        }

                        break;
                    }
            }
        }
    }
}
