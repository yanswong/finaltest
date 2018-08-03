using PluginSequence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VSLDtest.SubGroupTest
{
    public class Seq5_0
    {
        public static int locX { get; set; }
        public static int locY { get; set; }
        /*------------------------------------------------------------------------------------------------------------------------------------------
         * This test sequence is for UUT Information storage such as UUT Model Number, Serial Numbers, Cal Leak Information
         * Spectube, Valve Block and External Cal Leak Information
         -------------------------------------------------------------------------------------------------------------------------------------------*/
        public static void DoUUTInfoTest(ref TestInfo myTestInfo, UUTData myUUTData, CommonData myCommonData, string[] UUTInfos)
        {
            try
            {
                string intCalLeakSN = UUTInfos[0];
                string extCalLeakPN = UUTInfos[1];
                string extCalLeakSN = UUTInfos[2];
                recheck:
                string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                SQL_34970A.Calibrated(conStr);
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

                int step = 1;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin = 
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = myUUTData.Model;  // UUT model
                step++;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin =
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = myUUTData.SerNum; // UUT serial
                step++;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin =
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = intCalLeakSN;     // UUT Int Cal Leak Serial number
                step++;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin =
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = extCalLeakPN;     // Ext Cal Leak Part Number
                step++;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin =
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = extCalLeakSN;     // Ext Cal Leak Serial number
                step++;
                myTestInfo.ResultsParams[step].Result = myTestInfo.ResultsParams[step].SpecMin =
                myTestInfo.ResultsParams[step].Nominal = myTestInfo.ResultsParams[step].SpecMax = myUUTData.Options;    // slot number
                step++;
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
