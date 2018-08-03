using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomFormLibrary;
using SerialPortIO;
using PluginSequence;
using System.Windows.Forms;
using SystemIO;
using Agilent.TMFramework.InstrumentIO;
using System.Threading;
using System.Diagnostics;
using VSLDtest.TestForms;
using PreheatMeasure;
using System.Data.SqlClient;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_2
    {
        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set;  }
        public static int locX { get; set; }
        public static int locY { get; set; }
        public string SN { get; set; }

        public static TestInfo DoSeq5_2(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata, ref CommonData myCommonData)
        {
            string Display;
            string reading;
            string retval;
            int step = 1;
            Helper.comPort = comPort;

            //VSLeakDetector myLD = new VSLeakDetector(comPort);

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.2.4 Communication_Setup":
                        {
                            step = 1;

                            //@@ Connect the power cord to the main AC supply. @@//

                            Trace.WriteLine("Power ON the UUT...");
                            
                                InstrumentIO.DAS_Power_ON(myuutdata.Options);
                            
                            myTestInfo.ResultsParams[step].Result = "ok";
                           
                            Trace.WriteLine("Test point complete.");
                           
                            step++;


                            //@@ Serial port communication test @@//

                            Trace.WriteLine("Verify the serial port communication between the UUT and the PC...");

                            //myLD.Open();
                            int timeout = 1;
                            myLD.Timeout = 1000;
                            myLD.Terminator = "\r\n";

                            while (timeout > 0)
                            {
                                retval = myLD.Read();

                                if (retval.Contains("ok"))
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    Trace.WriteLine("Test point complete.");

                                    break;
                                }

                                if (timeout > 100)
                                {
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                Thread.Sleep(1000);
                                timeout++;
                            }

                            //myLD.Close();
                            break;
                        }
                       
                    case "5.2.4 PreheatSetup":
                        {
                            string conStr1 = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                            SQL_34970A.Update_DMM_Status(myuutdata.Options, "No", conStr1);
                            string SN = myuutdata.SerNum;
                            string Model = myuutdata.Model;
                            MyForm myPreheatForm = new MyForm(myLD);
                            //myPreheatForm.comPort = comPort;
                            myPreheatForm.ShowDialog();

                            string disValue = myPreheatForm.Pvvalue;
                            if (disValue == "Abort")
                            {
                                MessageBox.Show("Aborted by user","ABORTED", MessageBoxButtons.OK);
                                myCommonData.Mode = "Abort";
                            }

                            disValue = disValue.Trim(new Char[] { ' ', '?', 'P', 'V'});
                            disValue = disValue.TrimEnd(new Char[] { ' ','o', 'k','\r','\n' });
                            myTestInfo.ResultsParams[1].Result = disValue;
                            myTestInfo.ResultsParams[2].Result = Convert.ToString(myPreheatForm.checkvolt);
                            string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                            using (SqlConnection my_sql = new SqlConnection(conStr))
                            {
                                my_sql.Open();

                                SqlCommand command = new SqlCommand("insert into PV(LOT,MODEL,PREHEAT,RESULT,DAC,FIRMWARE,ION_SOURCE)" +
                                   "VALUES('" + SN +"','" +Model+ "','" + myPreheatForm.checkvolt + "','PASS','" + disValue + "','1.04'" + ",'" + myPreheatForm.ISsn +"')", my_sql);
                                command.ExecuteNonQuery();
                                my_sql.Close();
                            }

                            break;
                        }
                    case "5.2.5 Measure_DCVoltage":
                        {
                            step = 1;

                            //Lock DMM

                            string conStr = myTestInfo.TestParams[1].Value;
                            int checking = 1;

                            //while (checking > 0)
                            //{
                            //    string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                            //    if (DMMstatus == "No")
                            //    {
                            //        SQL_34970A.Update_DMM_Status(myuutdata.Options, "Yes", conStr);    //Yes = Lock     No = Unlock
                            //        break;
                            //    }

                            //    Thread.Sleep(5000);
                            //}


                            //@@ Measure the voltage between the black and red wire of the chasis fan connector. The acceptable range is 22.8V ~ 25.2V @@//

                            Trace.WriteLine("Measuring the voltage of the chasis fan connector...");
                            FormDcPowerSupply dcvForm = new FormDcPowerSupply();
                            dcvForm.StartPosition = FormStartPosition.Manual;
                            dcvForm.Location = new System.Drawing.Point(locX, locY);
                            dcvForm.ShowDialog();
                            double dcvReading = 0;
                            if (dcvForm.DialogResult == DialogResult.OK)
                            {
                                dcvReading = dcvForm.DcVolt;
                            }
                            else
                            {
                                dcvReading = dcvForm.DcVolt;
                                //throw new Exception("24V measurement canceled by user!");
                            }

                            myTestInfo.ResultsParams[1].Result = dcvReading.ToString();
                            //reading = InstrumentIO.Measure_DCV("");

                            //if (Convert.ToDouble(reading) >= 22.8 && Convert.ToDouble(reading) <= 25.2)
                            //{
                            //    Trace.WriteLine("Test point complete.");
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //}
                            
                            //else
                            //{
                            //    myTestInfo.ResultsParams[step].Result = reading;
                            //    //throw new Exception("Measured value out of acceptable range.");
                            //}

                            //Unlock DMM

                            //Trace.WriteLine("Unlocking the DMM...");
                            //SQL_34970A.Update_DMM_Status(myuutdata.Options, "No", conStr);

                            break;
                        }

                    case "5.2.6 Check_Fan":
                        {
                            step = 1;

                            //@@ To verify the condition of both fans @@//

                            Trace.WriteLine("Verify the condition of both fans...");

                            Display = "Seq 5.2.6:\nPlease verify the condition of BOTH fans. Are they running as expected? \n\nEnter 'ok' if pass. Else 'no'";
                            reading = Helper.LabelDisplay(Display);

                            if (reading == "ok")
                            {
                                Trace.WriteLine("Test point complete.");
                                myTestInfo.ResultsParams[step].Result = reading;
                            }

                            else if(reading == "no")
                            {
                                myTestInfo.ResultsParams[step].Result = reading;
                                //throw new Exception("One or both fans are not functioning properly.");
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
