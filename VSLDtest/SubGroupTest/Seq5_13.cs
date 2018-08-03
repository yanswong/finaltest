using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using PluginSequence;
using CustomFormLibrary;
using SystemIO;
using System.Windows.Forms;
using Agilent.TMFramework.InstrumentIO;
using SerialPortIO;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_13
    {
        internal static string iteSlot;

        public static int locX { get; set; }
        public static int locY { get; set; }

        public static TestInfo DoSeq5_13(VSLeakDetector myLD,ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            Boolean status = false;
            string reading;
            int step = 1;
            //Helper.comPort = comPort;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.13.1 Power":
                        {
                            //Lock the DMM

                            int checking = 1;
                            string conStr = myTestInfo.TestParams[1].Value;

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


                            //@@ Disconnect power from the UUT @@//

                            Trace.WriteLine(iteSlot + "Disconnect power from the UUT...");
                            if (iteSlot.Contains("P1"))
                            {
                                InstrumentIO.DAS_Power_OFF("1");
                                Trace.WriteLine(iteSlot + "Unlocking the DMM...");
                                SQL_34970A.Update_DMM_Status("1", "No", conStr);
                            }
                            else
                            {
                                InstrumentIO.DAS_Power_OFF("2");
                                Trace.WriteLine(iteSlot + "Unlocking the DMM...");
                                SQL_34970A.Update_DMM_Status("2", "No", conStr);
                            }
                            Trace.WriteLine(iteSlot + "Test point complete.");
                            myTestInfo.ResultsParams[step].Result = "ok";

                            
                            break;
                        }

                    case "5.13.3 Measure_Resistane":
                        {
                            string res_range = myTestInfo.TestParams[2].Value;

                            //@@ Measure resistance between the gnd blade and tp metal (< 0.8) @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between the gnd blade and tp metal (< 0.8)...");
                            double res1 = InstrumentIO.Measure_Resistance(0, "100", "0.001");
                            //reading = InstrumentIO.Measure_Resistance(0, res_range);
                            myTestInfo.ResultsParams[step].Result = res1.ToString();
                            //Trace.WriteLine(iteSlot + "Test point complete.");
                            //if (Convert.ToDouble(reading) < 0.8)
                            //{
                                
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //}

                            //else
                            //{
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //    //throw new Exception("Measured value out of acceptable range.");
                            //}

                            step++;


                            //@@ Measure resistance between the ground blade and grounding stud. (< 0.8) @@//

                            Trace.WriteLine(iteSlot + "Measure resistance between the ground blade and grounding stud. (< 0.8)...");
                            //reading = InstrumentIO.Measure_Resistance(1, res_range);
                            double res2 = InstrumentIO.Measure_Resistance(0, "100", "0.001");
                            myTestInfo.ResultsParams[step].Result = res2.ToString();
                            Trace.WriteLine(iteSlot + "Test point complete.");
                            //if (Convert.ToDouble(reading) < 0.8)
                            //{
                                
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //}

                            //else
                            //{
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //    //throw new Exception("Measured value out of acceptable range.");
                            //}

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

                            //Unlock DMM

                            step++;
                        }

                        break;

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
