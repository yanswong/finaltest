using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using CustomFormLibrary;
using System.Windows.Forms;
using SerialPortIO;
using SystemIO;
using System.Threading;
using System.Diagnostics;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_3
    {
        private static string revdate;

        public static string iteSlot { get; internal set; }
        public static string comPort { get; internal set; }
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static TestInfo DoSeq5_3(VSLeakDetector myLD, ref TestInfo myTestInfo, ref UUTData myuutdata, ref CommonData myCommonData)
        {
            //VSLeakDetector myLD = new VSLeakDetector(comPort);
            //myLD.iteSlot = iteSlot;

            int step = 1;
            string Display;
            string Reading = "";
            string retval = "";           
            string aop = "";
            Boolean status = false;
            Helper.comPort = comPort;

            try     
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.3.4 revdate_checksum":
                        {
                            // For revision and checksum check, VS and London firmware revision and checksum has different limit.
                            var VSFWRev = myTestInfo.TestParams[1].Value;
                            var VSFwChecksum = myTestInfo.TestParams[2].Value;
                            var LondonFWRev = myTestInfo.TestParams[3].Value;
                            var LondonFwChecksum = myTestInfo.TestParams[4].Value;

                            // check Leak Detector family, VS or London or etc
                            if (myuutdata.Model.StartsWith("VS") || myuutdata.Model.StartsWith("G860")|| myuutdata.Model.StartsWith("MSP"))
                            {
                                Trace.WriteLine(iteSlot + "UUT is VS Leak Detector. Setting limits for revision and checksum.");
                                myTestInfo.ResultsParams[1].SpecMax = myTestInfo.ResultsParams[1].SpecMin = myTestInfo.ResultsParams[1].Nominal = VSFWRev;
                                myTestInfo.ResultsParams[2].SpecMax = myTestInfo.ResultsParams[2].SpecMin = myTestInfo.ResultsParams[2].Nominal = VSFwChecksum;
                            }
                            else if (myuutdata.Model.StartsWith("G861"))
                            {
                                Trace.WriteLine(iteSlot + "UUT is London Leak Detector. Setting limits for revision and checksum.");
                                myTestInfo.ResultsParams[1].SpecMax = myTestInfo.ResultsParams[1].SpecMin = myTestInfo.ResultsParams[1].Nominal = LondonFWRev;
                                myTestInfo.ResultsParams[2].SpecMax = myTestInfo.ResultsParams[2].SpecMin = myTestInfo.ResultsParams[2].Nominal = LondonFwChecksum;
                            }
                            else
                                throw new Exception("Invalid model number selected for FW revision and checksum settings");

                            Trace.WriteLine(iteSlot + "Retrieve the revision date and checksum...");
                            retval = Helper.SendCommand(myLD, ref status, "VER", "LD", "ok");
                            
                            if(status == true)
                            {
                                string[] response = retval.Split(new string[] { "VER ", " ok" }, StringSplitOptions.RemoveEmptyEntries);

                                if (response[0].Contains("LD"))
                                {
                                    revdate = response[0];
                                }
                                else
                                {
                                    revdate = response[3];
                                }
                                
                                // Hairus added to split rev number and checksum
                                var fwrev = revdate.Split(' ')[0];
                                var checkSum = revdate.Split(' ')[1];

                                Trace.WriteLine(iteSlot + "Test point complete.");
                                myTestInfo.ResultsParams[1].Result = fwrev;
                                myTestInfo.ResultsParams[2].Result = checkSum;                          
                            }
                            else
                            {                               
                                myTestInfo.ResultsParams[1].Result = "FAILED";
                                myTestInfo.ResultsParams[2].Result = "FAILED";
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.3.5 LCDrev_checksum":
                        {
                            Trace.WriteLine(iteSlot + "Verify the revision date and checksum through reading on LCD display...");

                            Display = "Seq 5.3.5: \nPlease verify the revision date and checksum on the LCD display under system information by comparing them to the result obtained:\n\n" + revdate + "\n\nEnter 'ok' if passed, 'no' if failed.";

                            Reading = Helper.LabelDisplay(Display);
                            myTestInfo.ResultsParams[1].Result = Reading;
                            
                            if(Reading == "ok")
                            {
                                Trace.WriteLine(iteSlot + "Test point complete.");
                            }

                            else if(Reading == "no")
                            {
                                //throw new Exception("Test point failed.");
                            }

                            break;
                        }

                    case "5.3.6 uut_init":
                        {
                            step = 1;
                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Access Full Command @@//

                                Trace.WriteLine(iteSlot + "Access to full command...");

                                status = Helper.DoThis(myLD, ref myTestInfo, "XYZZY", "XYZZY ok", step, "ok");
                                step++;


                                //@@ Set date @@//

                                Trace.WriteLine(iteSlot + "Set date to current date...");
                                var dateNow = DateTime.Now; // hairus added to simplify coding

                                if (dateNow.ToString("tt") == "AM")
                                    aop = "0";

                                else if (dateNow.ToString("tt") == "PM")
                                    aop = "1";

                                string format = "MM dd yyyy hh mm ss";          //DOW, month, day, year, hour, min, sec, am/pm, 12 hr scale         
                                var initDateCommand = Convert.ToString((int)dateNow.DayOfWeek) + " " + dateNow.ToString(format) + " " + aop + " 1 INIT-DATE";
                                Trace.WriteLine(iteSlot + "Sending init-date command...");
                                Trace.WriteLine(initDateCommand);
                                status = Helper.DoThis(myLD, ref myTestInfo, initDateCommand, "ok", step, "ok");
                                step++;


                                //@@ Verify date @@//

                                Trace.WriteLine(iteSlot + "Verify the configured date...");

                                format = "MM/dd/yyyy hh:mm";                   //month, day, year, hour, min, sec
                                //myLD = new VSLeakDetector(comPort);
                                //myLD.iteSlot = iteSlot;
                                // myLD.Open();
                                var retVal = myLD.Query("?CURRDATE", ref status, "?CURRDATE", "ok\r\n", " ");
                                //myLD.Close();
                                if (retVal.Length > 0)
                                {
                                    myTestInfo.ResultsParams[step].SpecMin = DateTime.Now.ToString("MM/dd/yyyy");       // set spec as date
                                    myTestInfo.ResultsParams[step].SpecMax = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                    myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                    myTestInfo.ResultsParams[step].Result = retVal[1];
                                }
                                else
                                {
                                    Trace.WriteLine(iteSlot + "Error when retrieving date from Leak Detector");
                                    myTestInfo.ResultsParams[step].SpecMin = DateTime.Now.ToString("MM/dd/yyyy");       // set spec as date
                                    myTestInfo.ResultsParams[step].SpecMax = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                    myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                }
                                
                                //status = Helper.DoThis(ref myTestInfo, "?CURRDATE", dateNow.ToString(format), step, "ok");
                                step++;


                                //@@ set service date @@//
                                Trace.WriteLine(iteSlot + "Set the service date of the UUT to current date...");
                                dateNow = DateTime.Now;
                                format = "M d yyyy";
                                status = Helper.DoThis(myLD, ref myTestInfo, dateNow.ToString(format) + " INIT-PUMPSERVICED", "ok", step, "ok");
                                step++;


                                //@@ verify service date @@//
                                Trace.WriteLine(iteSlot + "Verify the service date...");
                                format = "M d yyyy";
                                //myLD = new VSLeakDetector(comPort);
                                //myLD.iteSlot = iteSlot;
                                //myLD.Open();
                                retVal = myLD.Query("?PUMPSERVICED", ref status, "?PUMPSERVICED", "ok\r\n", " ");
                                //myLD.Close();
                                myTestInfo.ResultsParams[step].SpecMin = dateNow.ToString(format).Trim();                  // set spec as date
                                myTestInfo.ResultsParams[step].SpecMax = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMin;    // set spec as date
                                if (retVal.Length > 0)
                                {
                                    myTestInfo.ResultsParams[step].Result = string.Format("{0} {1} {2}", retVal[0], retVal[1], retVal[2]).Trim();
                                }
                                else
                                {
                                    Trace.WriteLine(iteSlot + "Error when retrieving date from Leak Detector");
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                }
                                //status = Helper.DoThis(ref myTestInfo, "?PUMPSERVICED", dateNow.ToString(format), step, "ok");
                                step++;


                                //@@ Set forepump type @@//
                                Trace.WriteLine(iteSlot + "Set forepump type..."); 
                                string pumptype = Helper.GetPumpType(myuutdata.Model);
                                if (pumptype == "Wet")
                                {
                                    Reading = "0";
                                    status = Helper.DoThis(myLD, ref myTestInfo, "0 INIT-FOREPUMP", "ok", step, Reading);

                                    Trace.WriteLine(iteSlot + "Forepump: WET");
                                }

                                else if (pumptype == "Dry")
                                {
                                    Reading = "1";
                                    status = Helper.DoThis(myLD, ref myTestInfo, "1 INIT-FOREPUMP", "ok", step, Reading);

                                    Trace.WriteLine(iteSlot + "Forepump: DRY");
                                }
                                myTestInfo.ResultsParams[step].Nominal = Reading;
                                myTestInfo.ResultsParams[step].SpecMin = Reading;
                                myTestInfo.ResultsParams[step].SpecMax = Reading;

                                step++;


                                //@@ Verify forepump type @@//
                                if (status == true)
                                {
                                    Trace.WriteLine(iteSlot + "Verify the forepump type...");

                                    status = Helper.DoThis(myLD, ref myTestInfo, "?FOREPUMP", Reading, step, Reading);

                                    myTestInfo.ResultsParams[step].Nominal = Reading;
                                    myTestInfo.ResultsParams[step].SpecMin = Reading;
                                    myTestInfo.ResultsParams[step].SpecMax = Reading;
                                    myTestInfo.ResultsParams[step].Result  = Reading;                                                               
                                }
                                else
                                {                             
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                                    //throw new Exception("Test point failed.");
                                }

                                step++;


                                //@@ Initiate the LD configuration @@//
                                // Check whether the LD w/ or wo/ diaphragm pump 
                                Trace.WriteLine(iteSlot + "Initaiate the LD configuration...");
                                var withDiaphragm = Helper.IsLDWithDiaphragmPump(myuutdata.Model);
                                if (!withDiaphragm)
                                {
                                    Reading = "0";
                                    status = Helper.DoThis(myLD, ref myTestInfo, "0 INIT-LDCONFIG", "ok", step, Reading);
                                }
                                
                                else 
                                {
                                    Reading = "1";
                                    status = Helper.DoThis(myLD, ref myTestInfo, "1 INIT-LDCONFIG", "ok", step, Reading);
                                }
                                
                                myTestInfo.ResultsParams[step].SpecMax = Reading;
                                myTestInfo.ResultsParams[step].SpecMin = Reading;
                                myTestInfo.ResultsParams[step].Nominal = Reading;

                                step++;


                                //@@ Set the date for the exhaust pump (Applicable for dry scroll pump type only) @@//

                                Trace.WriteLine(iteSlot + "Set the service date for the exhaust pump (Applicable for DRY SCROLL pump only).");

                                format = "MM dd yyyy";

                                if (pumptype == "Dry")
                                {
                                    status = Helper.DoThis(myLD, ref myTestInfo, dateNow.ToString(format) + " INIT-DPUMPDATE", "ok", step, "ok");
                                    myTestInfo.ResultsParams[step].SpecMin = "ok";
                                }

                                else
                                {
                                    //Skip the verification of pump service date if the pump type is WET

                                    Trace.WriteLine(iteSlot + "Test skipped due to forepump type: WET");

                                    myTestInfo.ResultsParams[step].Result  = "skip";
                                    myTestInfo.ResultsParams[step].SpecMax = "skip";
                                    myTestInfo.ResultsParams[step].Nominal = "skip";
                                    myTestInfo.ResultsParams[step].SpecMin = "skip";

                                    myTestInfo.ResultsParams[step + 1].Result  = "skip";
                                    myTestInfo.ResultsParams[step + 1].SpecMax = "skip";
                                    myTestInfo.ResultsParams[step + 1].Nominal = "skip";
                                    myTestInfo.ResultsParams[step + 1].SpecMin = "skip";

                                    goto skip;
                                }

                                step++;


                                //@@ Verify the date of the exhaust pump. *This test will be skipped if forepump = WET @@//
                                Trace.WriteLine(iteSlot + "Verify the date of the exhaust pump. (Applicable for LD with diaphragm pump only).");

                                format = "M d yyyy";
                                // since return value from LD for DPUMP service date is without 'ok' terminator, need to check if return contains '#?' char.
                                //myLD = new VSLeakDetector(comPort);
                                //myLD.iteSlot = iteSlot;
                                //myLD.Open();
                                var myRetVal = myLD.QueryWithoutTerminator("?DPUMPSERVICED", ref status, "?DPUMPSERVICED", 2000);
                                //myLD.Close();
                                //status = Helper.DoThis(ref myTestInfo, "?DPUMPSERVICED", dateNow.ToString(format), step, "ok", "\r\n");
                                myTestInfo.ResultsParams[step].SpecMin = dateNow.ToString(format);
                                myTestInfo.ResultsParams[step].SpecMax = dateNow.ToString(format);
                                myTestInfo.ResultsParams[step].Nominal = dateNow.ToString(format);
                                myTestInfo.ResultsParams[step].Result = myRetVal.Trim();   // set the result as Diaphragm pump stored service date from the LD

                                step++;
                            }

                         skip:
                            break;
                        }

                    case "5.3.7 No_Sniff":
                        {
                            step = 1;
                            //@@ Turns the HIGH PRESSURE TEST function OFF. @@//
                           
                            Trace.WriteLine(iteSlot + "Turns the HIGH PRESSURE TEST function OFF...");

                            status = Helper.DoThis(myLD, ref myTestInfo, "NOSNIFF", "ok", step, "ok");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //if (myLD != null)
                    //myLD.Close();
                Helper.Fail_Test(ref myTestInfo, ex.Message, step);
                throw;
            }

            return myTestInfo;
        }
    }
}