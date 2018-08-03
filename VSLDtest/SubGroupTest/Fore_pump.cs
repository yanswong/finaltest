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

namespace VSLDtest.SubGroupTest
{
    public class Fore_pump
    {
        public static TestInfo DoFore_pump(ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            VSLeakDetector myLD = new VSLeakDetector("COM4");

            string Reading = "";            //LD config

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    //Initiate the forepump type. 0 = WET, 1 = DRY
                    case "forepump_init":
                        {
                            myLD.Open();

                            string pumptype = Helper.GetPumpType(myuutdata.Model);

                            if(pumptype == "Wet")
                            {
                                myLD.Write("0 INIT-FOREPUMP");
                            }

                            else if(pumptype == "Dry")
                            {
                                myLD.Write("1 INIT-FOREPUMP");
                            }

                            ////WET
                            //if (myuutdata.Model.Contains("R") || myuutdata.Model == "G8600-60000" || myuutdata.Model == "G8601-60000" || myuutdata.Model == "G8602-60000")
                            //{
                            //    myLD.Write("0 INIT-FOREPUMP");
                            //    type = "0";
                            //}

                            ////DRY
                            //else if (myuutdata.Model.Contains("D") || myuutdata.Model == "G8600-60001" || myuutdata.Model == "G8601-60001" || myuutdata.Model == " G8601-60002" || myuutdata.Model == "G8602-60001" || myuutdata.Model == "G8602-60002")
                            //{
                            //    myLD.Write("1 INIT-FOREPUMP");
                            //    type = "1";
                            //}
                            
                            string retval = myLD.Read();
                            
                            if (retval.Contains("ok"))
                            {
                                myTestInfo.ResultsParams[1].Result = pumptype;
                                
                                myLD.Write("?FOREPUMP");
                                string retval2 = myLD.Read();

                                if (retval2.Contains("0") || retval2.Contains("1"))

                                    myTestInfo.ResultsParams[2].Result = "ok";
                                
                                else
                                    myTestInfo.ResultsParams[2].Result = "FAILED";
                            }
                            else
                                myTestInfo.ResultsParams[1].Result = "FAILED";

                            myLD.Close();
                            break;
                        }
                    
                    //Initiate the LD configuration     
                    case "init_ldconfig":
                        {
                            myLD.Open();

                            if (myuutdata.Model.Contains("PR02") || myuutdata.Model.Contains("MR15") || myuutdata.Model.Contains("BR15") || myuutdata.Model == "G8600-60000" || myuutdata.Model == "G8601-60000" || myuutdata.Model == "G8602-60000")
                            {
                                myLD.Write("0 INIT-LDCONFIG");
                                Reading = "0";
                            }

                            else if (myuutdata.Model.Contains("D") || myuutdata.Model == "G8600-60001" || myuutdata.Model == "G8601-60001" || myuutdata.Model == " G8601-60002" || myuutdata.Model == "G8602-60001" || myuutdata.Model == "G8602-60002")
                            {
                                myLD.Write("1 INIT-LDCONFIG");
                                Reading = "1";
                            }

                            string retval = myLD.Read();

                            if (retval.Contains("ok"))
                                myTestInfo.ResultsParams[1].Result = Reading;
                            
                            else
                                myTestInfo.ResultsParams[1].Result = "FAILED";

                            myLD.Close();
                            break;
                        }

                    //Set the date for the exhaust pump
                    case "set_exhdate":
                        {
                            myLD.Open();

                            string format = "MM dd yyyy";
                            myLD.Write(DateTime.Now.ToString(format) + " INIT-DPUMPDATE");
                         
                            string retval = myLD.Read();

                            if (retval.Contains("ok"))
                                Reading = "ok";
                            else
                                Reading = "FAILED";

                            myTestInfo.ResultsParams[1].Result = Reading;

                            myLD.Close();
                            break;
                        }

                    //Verify the date of the exhaust pump
                    case "verify_exhdate":
                        {
                            myLD.Open();

                            string format = "MM dd yyyy";

                            myLD.Write("?DPUMPSERVICED");
                            string retval = myLD.Read();                            
                            
                            if (retval.Contains(DateTime.Now.ToString(format)))
                                Reading = "ok";
                            else
                                Reading = "FAILED";

                            myTestInfo.ResultsParams[1].Result = Reading;

                            myLD.Close();
                            break;
                        }

                    //Turns the HIGH PRESSURE TEST function OFF
                    case "send_nosniff":
                        {
                            myLD.Open();

                            myLD.Write("NOSNIFF");
                            string retval = myLD.Read();

                            if (retval.Contains("ok"))
                                Reading = "ok";
                            else
                                Reading = "FAILED";

                            myTestInfo.ResultsParams[1].Result = Reading;

                            myLD.Close();
                            break;
                        }

                    default:
                        break;
                }
            }

            catch (Exception)
            {
                throw;
            }

            return myTestInfo;
        }

        public static string LabelDisplay(string display)
        {
            userprompt my_userprompt = new userprompt();
           
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
    }
}
