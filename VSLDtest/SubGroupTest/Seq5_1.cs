using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using CustomFormLibrary;
using System.Windows.Forms;
using SystemIO;
using Agilent.TMFramework.InstrumentIO;
using System.Threading;
using System.Diagnostics;
using SerialPortIO;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_1
    {
        public static string iteSlot { get; set; }
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static TestInfo DoSeq5_1(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata)
        { 
            string reading;
            userprompt my_userprompt = new userprompt();

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.1.1 Power":
                        {
                            //@@ Ensure that the power cord is not plugged into the UUT & the main AC supply outlet. @@//

                            Trace.WriteLine(iteSlot + "Disconnect the power supply from the UUT...");

                            recheck:
                            string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                            string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                            if (DMMstatus == "No")
                            {

                            }
                            else
                            {
                                Trace.WriteLine("DMM locked");
                                Thread.Sleep(5000);
                                goto recheck;
                            }

                            InstrumentIO.DAS_Power_OFF(myuutdata.Options);
                            myTestInfo.ResultsParams[1].Result = "ok";

                            break;
                        }

                    case "5.1.2 Check_PumpVoltage":
                        {
                            /*@@ SKIP* Check the LD has the correct voltage mechanical pump installed (115VAC or 230VAC) or that the voltage selector switch
                            on the vacuum pump is set to meet the rated mains AC requirements for that model LD. @@*/

                            myTestInfo.ResultsParams[1].Result = "ok";

                            break;
                        }

                    case "5.1.3 Check_Setup":
                        {
                            //@@ Prompt user to ensure that all KF clamps, grounding straps, cable and wire connections between the PCBs, valves and pumps are in place. @@//

                            string display = "Seq 5.1.3:\nEnsure that all KF clamps, grounding straps, cable and wire connections between the PCBs,\nvalves and pumps are in place. Enter 'ok' once done.";
                            string Reading = Helper.LabelDisplay(display);

                            myTestInfo.ResultsParams[1].Result = Reading;

                            break;
                        }

                    case "5.1.5 Measure_Resistance":
                        {
                            //Lock the DDM from other test sockets while it is been used. Before that, verify whether it is locked or not.
                            int step = 1;
                            int checking = 1;
                            string DMMstatus = "";
                            string conStr = myTestInfo.TestParams[1].Value;
                            string res_range = myTestInfo.TestParams[2].Value;

                            Trace.WriteLine(iteSlot + "Check before Locking the DMM...");

                            while (checking > 0)
                            {
                                recheck:
                                DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                                if (DMMstatus == "No")
                                {
                                      SQL_34970A.Update_DMM_Status(myuutdata.Options, "Yes", conStr);    //Yes = Lock     No = Unlock
                                    Thread.Sleep(5000);
                                    break;
                                }
                                else
                                {
                                    MessageBox.Show("DMM locked","warning",MessageBoxButtons.OK);
                                    Thread.Sleep(5000);
                                    goto recheck;
                                }

                                
                            }

                            MessageBox.Show("Please flip Leak Detector switch to OFF positions", "OFF Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            //@@ Measure resistance between line blade and neutral blade OFF (0.8~1.2) @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between line blade and neutral blade OFF");

                            //reading = InstrumentIO.Measure_Resistance(4, res_range);
                            reading = InstrumentIO.Measure_Res_LB_NB_OFF();
                            Trace.WriteLine(iteSlot + "Test point complete.");
                            myTestInfo.ResultsParams[step].Result = reading;

                            step++;


                            //@@ Measure resistance between line blade and ground blade OFF @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between line blade and ground blade OFF...");
                            reading = InstrumentIO.Measure_Resistance(2, res_range);

                            if (Convert.ToDouble(reading) > 10000000)
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "100000000";
                            }
                            else
                            {
                                myTestInfo.ResultsParams[step].Result = reading;
                                //throw new Exception("Measured value out of acceptable range.");
                            }

                            step++;


                            //@@ Measure resistance between neutral blade and ground blade OFF @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between neutral blade and ground blade OFF...");
                            reading = InstrumentIO.Measure_Resistance(3, res_range);

                            if (Convert.ToDouble(reading) > 10000000)
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "100000000";
                            }

                            else
                            {
                                myTestInfo.ResultsParams[step].Result = reading;
                                //throw new Exception("Measured value out of acceptable range.");
                            }
                            step++;

                            MessageBox.Show("Please flip Leak Detector switch to ON positions", "ON Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //@@ Measure resistance between the gnd blade and tp metal (< 0.8) @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between the gnd blade and tp metal (< 0.8)...");
                            double res1 = InstrumentIO.Measure_Resistance(0, "100", "0.001");
                            //reading = InstrumentIO.Measure_Resistance(0, res_range);
                            myTestInfo.ResultsParams[step].Result = res1.ToString();
                            step++;


                            //@@ Measure resistance between the ground blade and grounding stud. (< 0.8) @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between the ground blade and grounding stud. (< 0.8)...");
                            //reading = InstrumentIO.Measure_Resistance(1, res_range);
                            double res2 = InstrumentIO.Measure_Resistance(0, "100", "0.001");
                            myTestInfo.ResultsParams[step].Result = res2.ToString();
                            Trace.WriteLine(iteSlot + "Test point complete.");
                            step++;


                            //@@ Measure resistance between line blade and neutral blade ON (Based on model) @@//
                            List<string> listOfLimits = new List<string>();
                            listOfLimits.Add(myTestInfo.TestParams[3].Value);
                            listOfLimits.Add(myTestInfo.TestParams[4].Value);
                            listOfLimits.Add(myTestInfo.TestParams[5].Value);
                            listOfLimits.Add(myTestInfo.TestParams[6].Value);
                            listOfLimits.Add(myTestInfo.TestParams[7].Value);
                            listOfLimits.Add(myTestInfo.TestParams[8].Value);

                            Trace.WriteLine(iteSlot + "Measure resistance between line blade and neutral blade ON (Based on model)...");
                            string resReading = InstrumentIO.Measure_Res_LB_NB_ON(myuutdata.Model, res_range, listOfLimits.ToArray(), ref myTestInfo, step);
                            Trace.WriteLine(iteSlot + "Test point completed");
                            myTestInfo.ResultsParams[step].Result = resReading;
                            step++;


                            //@@ Measure resistance between line blade and ground blade ON @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between line blade and ground blade ON...");
                            reading = InstrumentIO.Measure_Resistance(2, res_range);

                            if (Convert.ToDouble(reading) > 10000000)
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "100000000";
                            }

                            else
                            {
                                myTestInfo.ResultsParams[step].Result = reading;
                                //throw new Exception("Measured value out of acceptable range.");
                            }

                            step++;


                            //@@ Measure resistance between neutral blade and ground blade ON @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between neutral blade and ground blade ON...");
                            reading = InstrumentIO.Measure_Resistance(3, res_range);

                            if (Convert.ToDouble(reading) > 10000000)
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[step].Result = "100000000";
                            }

                            else
                            {
                                myTestInfo.ResultsParams[step].Result = reading;
                                //throw new Exception("Measured value out of acceptable range.");
                            }

                            step++;

                            //Unlock DMM

                            Trace.WriteLine(iteSlot + "Unlocking the DMM...");
                            conStr = myTestInfo.TestParams[1].Value;

                           // SQL_34970A.Update_DMM_Status(myuutdata.Options, "No", conStr);
                            step++;

                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Helper.Fail_Test(ref myTestInfo, ex.Message, 1);
                throw;
            }

            return myTestInfo;
        }
    }
}
