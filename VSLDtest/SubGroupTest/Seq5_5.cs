using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomFormLibrary;
using PluginSequence;
using SerialPortIO;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_5
    {
        private static string speed;

        public static string Speed
        {
            get { return speed; }

            set { speed = value; }
        }

        public static TestInfo DoSeq5_5(ref TestInfo myTestInfo, ref UUTData myuutdata)
        {
            VSLeakDetector myLD = new VSLeakDetector("COM4");
            Boolean status;
            string retval;

            try
            {
                switch (myTestInfo.TestLabel)
                {
                    case "5.5.1 turbo_pump":
                        {
                            int step = 1;

                            while (step <= myTestInfo.ResultsParams.NumResultParams)
                            {
                                //@@ Access full command @@//

                                status = Helper.DoThis(ref myTestInfo, "XYZZY", "ok", step, "ok");
                                step++;


                                //@@ Obtain the details about the state of the turbo pump @@//

                                myLD.Open();

                                myLD.Write("?TURBO");

                                retval = myLD.Read();

                                if (retval.Contains("Turbo Ready") && retval.Contains("Turbo No Fault"))
                                {
                                    myTestInfo.ResultsParams[step].Result = "ok";
                                    string[] response = retval.Split(new string[] { "(RPM): ", " \r\nTurbo Temp (Celsius): " }, StringSplitOptions.RemoveEmptyEntries);

                                    for (int j = 0; j < 2; j++)
                                    {
                                        Speed = response[j];
                                    }
                                    step++;
                                }
                                else
                                    myTestInfo.ResultsParams[step].Result = "FAILED";
                            }
                            myLD.Close();
                            break;
                        }

                    case "5.5.2 turbo_speed":
                        {
                            //@@ Retrieve the turbo pump speed in RPM @@//

                            int spd = Convert.ToInt32(Speed);

                            if (spd >= 69000 && spd <= 71000)
                            {
                                myTestInfo.ResultsParams[1].Result  = Convert.ToString(spd);
                                myTestInfo.ResultsParams[1].Nominal = Convert.ToString(spd);
                            }
                            else
                                myTestInfo.ResultsParams[1].Result = "FAILED";

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
    }
}
