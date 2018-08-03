using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agilent.TMFramework.InstrumentIO;
using System.Threading;
using System.Diagnostics;
using PluginSequence;

namespace SystemIO
{
    public static class InstrumentIO
    {
        public static string channel;
        private static DirectIO das;
        private static int res_channel;

        public static DirectIO DAS
        {
            get { return das; }
            set { das = value; }
        }

        public static string Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        public static int Res_channel
        {
            get { return res_channel; }
            set { res_channel = value; }
        }


        //@@ Initialize the channel number for data accquisition system 34970A @@//
        public static void DAS_initialize()
        {
            try
            {
                DAS = new DirectIO("ASRL5::INSTR");
                DAS.Timeout = 10000;
                //DAS.WriteLine("*RST");
                //DAS.WriteLine("*CLS");
            }
            catch (Exception)
            {

                throw;
            }
        }


        //@@ Identify the voltage requirement based on the UUT model number and set accordingly @@//

        public static void Init_UUT_Voltage_Switches(string modelnumber, string slotnum)
        {
            try
            {
                switch (modelnumber)           //MISSING * G8600-60000, G8600-60001, G8601-60000, G8601-60001, G8601-60002, G8602-60000, G8602-60001, G8602-60002
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
                            if (slotnum == "1")
                            {
                                Channel = "(@102,103)";
                                Res_channel = 201;
                            }

                            else if (slotnum == "2")
                            {
                                Channel = "(@106,107)";
                                Res_channel = 211;
                            }
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
                            if (slotnum == "1")
                            {
                                Channel = "(@101,102,104)";
                                Res_channel = 206;
                            }

                            else if (slotnum == "2")
                            {
                                Channel = "(@105,106,108)";
                                Res_channel = 216;
                            }
                        }
                        break;
                }
            }

            catch (Exception)
            {

                throw;
            }
        }
     

        //@@ Power ON the UUT @@//
        public static void DAS_Power_ON(string slotNum)
        {
            try
            {
                // before power ON make sure that all 4 relays is open
                DAS_Power_OFF(slotNum);
                Thread.Sleep(2000);
                DAS.WriteLine("ROUTe:CLOSE " + Channel);
                slotNum = null;
            }
            catch (Exception)
            {

                throw;
            }         
        }


        //@@ Power OFF the UUT @@//
        public static void DAS_Power_OFF(string slotNum)
        {
            try
            {
                string channel = string.Empty;
                if (slotNum == "1")
                {
                    channel = "(@101,102,103,104)";
                    DAS.WriteLine("ROUTe:OPEN " + channel);
                }
                else
                {
                    channel = "(@105,106,107,108)";
                    DAS.WriteLine("ROUTe:OPEN " + channel);
                }
                //DAS.WriteLine("ROUTe:OPEN " + Channel);
                slotNum = null;
            }
            catch (Exception)
            {

                throw;
            }          
        }


        // Hairus added to get more accurate resistance measurement.
        /// <summary>
        /// To configure resistance measurement range and resolution for specific channel
        /// </summary>
        /// <param name="channel">Channel to be configured</param>
        /// <param name="range">Resistance range</param>
        /// <param name="resolution">Resolution</param>
        public static double Measure_Resistance(int channel, string range, string resolution)
        {
            try
            {
                int finalChannel = Res_channel + channel;
                //DAS.WriteLine("ROUTE:SCAN (@)");
                DAS.WriteLine("CONF:RES {0},{1},(@{2})", range, resolution, finalChannel);
                //DAS.WriteLine("ROUT:SCAN (@{0})", channel);
                //Thread.Sleep(3000);
                DAS.WriteLine("ROUT:CHAN:DELAY 5");
                DAS.WriteLine("INIT");
                DAS.WriteLine("FETCH?");
                var reading = DAS.ReadNumberAsDouble();

                return reading;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //@@ Measure Resistance between ground blade and test port metal @@//
        public static string Measure_Resistance(int switching, string res_range)
        {
            try
            {           
                DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + switching) + ")");
                Thread.Sleep(2000);

                string reading = DAS.Read();

                return reading;
            }

            catch (Exception)
            {

                throw;
            }
        }


        //@@ Measure resistance between line blade and neutral blade. The acceptable range is based on model number of the UUT. @@//
        public static Boolean Measure_Res_LB_NB_ON(string modelnumber, string res_range)
        {
            Boolean status = false;
            DAS.Timeout = 10000;
            try
            {
                //115V, PR02 and PD03
                if (modelnumber == "VSPR021" || modelnumber == "VSPD030")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 2.0 && Convert.ToDouble(reading) <= 15.0)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                //230V, PR02 and PD03
                else if (modelnumber == "VSPR022" || modelnumber == "VSPD032")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 9.2 && Convert.ToDouble(reading) <= 60.0)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                //115V, MR15 and BR15
                else if (modelnumber == "VSMR151" || modelnumber == "VSBR151")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 1.0 && Convert.ToDouble(reading) <= 5.0)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                //230V, MR15 and BR15
                else if (modelnumber == "VSMR152" || modelnumber == "VSBR152")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 3.2 && Convert.ToDouble(reading) <= 15.0)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                //115V, MD15, MD30, BD15 and BD30
                else if (modelnumber == "VSMD301" || modelnumber == "VSBD301" || modelnumber == "G8601-64004" || modelnumber == "G8602-64004")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 0.5 && Convert.ToDouble(reading) <= 1.2)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                //230V, MD15, MD30, BD15 and BD30
                else if (modelnumber == "VSMD302" || modelnumber == "VSBD302" || modelnumber == "G8601-64005" || modelnumber == "G8602-64005")
                {
                    DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    Thread.Sleep(2000);

                    string reading = DAS.Read();

                    if (Convert.ToDouble(reading) >= 1.0 && Convert.ToDouble(reading) <= 10.0)
                    {
                        status = true;
                    }
                    else
                        throw new Exception("Measured value out of acceptable range.");
                }

                else
                {
                    throw new Exception("Unknown model number.");
                }
            }

            catch (Exception)
            {

                throw;
            }

            return status;
        }

        // get reading instead of status, do limit checking at test executive side
        public static string Measure_Res_LB_NB_ON(string modelnumber, string res_range, string[] limitsArray, ref TestInfo myTestInfo, int step)
        {
            try
            {
                //115V, PR02 and PD03
                if (modelnumber == "VSPR021" || modelnumber == "VSPD030"
                    || modelnumber == "G8610-64000" || modelnumber == "G8610-64002"
                    || modelnumber == "G8610-64003")
                {
                    //DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    //Thread.Sleep(2000);

                    //string reading = DAS.Read();
                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 0;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                //230V, PR02 and PD03
                else if (modelnumber == "VSPR022" || modelnumber == "VSPD032"
                    || modelnumber == "G8610-64001" || modelnumber == "G8610-64004")
                {
                    //DAS.WriteLine("MEASure:RES? " + res_range + ", (@" + (Res_channel + 4) + ")");
                    //Thread.Sleep(2000);

                    //string reading = DAS.Read();

                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 1;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                //115V, MR15 and BR15
                else if (modelnumber == "VSMR151" || modelnumber == "VSBR151" 
                    || modelnumber == "G8611-64000" || modelnumber == "G8612-64000")
                {
                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 2;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                //230V, MR15 and BR15
                else if (modelnumber == "VSMR152" || modelnumber == "VSBR152" 
                    || modelnumber == "G8611-64001" || modelnumber == "G8612-64001"
                    || modelnumber == "G8611-64006" || modelnumber == "G8612-64006" || modelnumber=="MSPLL10779" || modelnumber== "MSPLL10767")
                {
                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 3;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                //115V, MD15, MD30, BD15 and BD30
                else if (modelnumber == "VSMD301" || modelnumber == "VSBD301" || modelnumber == "G8601-64004" || modelnumber == "G8602-64004"
                     || modelnumber == "G8611-64002" || modelnumber == "G8611-64004" || modelnumber == "G8612-64002" || modelnumber == "G8612-64004")
                {
                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 4;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                //230V, MD15, MD30, BD15 and BD30
                else if (modelnumber == "VSMD302" || modelnumber == "VSBD302" || modelnumber == "G8601-64005" || modelnumber == "G8602-64005"
                    || modelnumber == "G8611-64003" || modelnumber == "G8611-64005" || modelnumber == "G8612-64003" || modelnumber == "G8612-64005")
                {
                    double res = Measure_Resistance(4, "10", "0.001");
                    string reading = res.ToString();

                    int id = 5;
                    double specMin = Convert.ToDouble(limitsArray[id].Split(';').FirstOrDefault());
                    double specMax = Convert.ToDouble(limitsArray[id].Split(';').LastOrDefault());
                    myTestInfo.ResultsParams[step].SpecMin = specMin.ToString();
                    myTestInfo.ResultsParams[step].SpecMax = specMax.ToString();
                    myTestInfo.ResultsParams[step].Nominal = Convert.ToString(specMin + ((specMax - specMin) / 2));
                    return reading.ToString();
                }

                else
                {
                    throw new Exception("Unknown model number.");
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        public static string Measure_Res_LB_NB_OFF()
        {
            try
            {
                DAS.Timeout = 10000;
                var res = Measure_Resistance(4, "1000000", "1");
                return res.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // @@ Measure the DC voltage drop between the black and red wire of the chasis fan connector. The acceptable range is 22.8V ~ 25.2V @@ //
        public static string Measure_DCV(string channel)
        {
            try
            {
                DAS.WriteLine("MEASure:VOLT:DC? (@ " + channel + ")");
                Thread.Sleep(2000);

                string retval = DAS.Read();

                return retval;
            }

            catch (Exception)
            {

                throw;
            }
        } 
        
        //@@ Open the external calibrated leak @@//
        public static void Open_Extleak(int slotNum)
        {
            try
            {
                DAS.WriteLine("ROUTE:CLOSE (@11{0})", slotNum);
            }
            catch (Exception)
            {

                throw;
            }           
        }

        //@@ Close External Calibrated leak @@//
        public static void Close_Extleak(int slotNum)
        {
            try
            {
                DAS.WriteLine("ROUTE:OPEN (@11{0})", slotNum);
            }
            catch (Exception)
            {

                throw;
            }          
        }      





        public static void ConfigureThermocouple(DirectIO mySw)
        {
            try
            {
                mySw.WriteLine("ROUTE:SCAN (@)");
                mySw.WriteLine("CONF:TEMP TC,J,(@1001:1005)");
                mySw.WriteLine("TEMP:TRAN:TC:RJUN:TYPE INT,(@1001:1005)");
                mySw.WriteLine("ROUTE:SCAN (@1001:1005)");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static double MeasureCalLeakTemp(DirectIO mySw, int port)
        {
            try
            {
                mySw.WriteLine("INIT");
                mySw.WriteLine("FETCH?");
                var tempReadings = mySw.ReadList().Cast<double>().ToList();
                var UUTTemp = tempReadings[port];
                return UUTTemp;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to open electromagnetic valve.
        /// </summary>
        /// <param name="port">Integer 0 for NIST test valve. Integer 1,2,3 and 4 for Port1,Port2,Port3 and Port4 test valve respectively</param>
        public static void OpenTestValve(DirectIO mySw, int port)
        {
            try
            {
                string switchChannel = string.Empty;
                switch (port)
                {
                    case 0: switchChannel = "2001"; break;
                    case 1: switchChannel = "2021"; break;
                    case 2: switchChannel = "2022"; break;
                    case 3: switchChannel = "2023"; break;
                    case 4: switchChannel = "2024"; break;
                    default:
                        break;
                }

                mySw.WriteLine("ROUTE:CLOSE (@{0})", switchChannel);    // close means activate the 34937A SPDT Switch. Normally open port will now closed
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to close electromagnetic valve.
        /// </summary>
        /// <param name="port">Integer 0 for NIST test valve. Integer 1,2,3 and 4 for Port1,Port2,Port3 and Port4 test valve respectively</param>
        public static void CloseTestValve(DirectIO mySw, int port)
        {
            try
            {
                string switchChannel = string.Empty;
                switch (port)
                {
                    case 0: switchChannel = "2001"; break;
                    case 1: switchChannel = "2021"; break;
                    case 2: switchChannel = "2022"; break;
                    case 3: switchChannel = "2023"; break;
                    case 4: switchChannel = "2024"; break;
                    default:
                        break;
                }

                mySw.WriteLine("ROUTE:OPEN (@{0})", switchChannel);     // open means de-activate the 34937A SPDT Switch. Normally open port will go back to open state.

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void CloseAllTestValve(DirectIO mySw)
        {
            try
            {
                mySw.WriteLine("ROUTE:OPEN (@2001,2021:2024)");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static void SetupBoardTempMeasRoute(DirectIO mySw, DirectIO myScope, int port)
        {
            try
            {
                // Disconnect 5v supply from the temp-board, and the load.
                mySw.WriteLine("ROUTE:OPEN (@5001:5040)");
                // then connect to the desired port to measure board temperature
                mySw.WriteLine("ROUTE:CLOSE (@500{0},502{0})", port);
                // autoscale Oscilloscope to get measurement
                myScope.WriteLine(":AUToscale CHANNEL{0}", port);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void DisconnectTempBoardMeasRoute(DirectIO mySw, int port)
        {
            try
            {
                // Disconnect switches
                mySw.WriteLine("ROUTE:OPEN (@500{0},502{0})", port);
                Thread.Sleep(500);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mySw"></param>
        /// <param name="myScope"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static double MeasureBoardTemperature(DirectIO mySw, DirectIO myScope, int port)
        {
            try
            {
                // Now measure the period T1 and T2 to calculate the temperature
                //myScope.WriteLine(":AUToscale CHANNEL{0}", port);
                myScope.WriteLine(":MEASure:DUTYcycle? CHANNEL{0}", port);
                double dutyCycle = myScope.ReadNumberAsDouble();
                myScope.WriteLine(":MEASure:PERiod? CHANNEL{0}", port);
                double period = myScope.ReadNumberAsDouble();       // T1 + T2
                myScope.WriteLine(":MARKer:MODE MEASurement");

                double t1 = dutyCycle / 100 * period;
                double t2 = period - t1;

                // formula for temp board.
                double temperature = 235 - (400 * t1 / t2);

                return temperature;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
