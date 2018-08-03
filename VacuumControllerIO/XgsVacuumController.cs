// Type: VacuumControllerIO.XgsVacuumController
// Assembly: VacuumControllerIO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C1A7622-3733-44B0-841A-E16286973DCF
// Assembly location: Z:\VPD LDA\Z_General_Hairus\Projects\Intern Yan Han\IMG100\VacuumControllerIO.dll

using System;
using System.Diagnostics;
using System.IO.Ports;

namespace VacuumControllerIO
{
    public static class XgsVacuumController
    {
        private static XgsIO myIO;

        public static void Init()
        {
            myIO = new XgsIO();
        }

        public static void Init(string comPort)
        {
            XgsVacuumController.myIO = new XgsIO(comPort);
        }

        public static void Init(string comPort, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            XgsVacuumController.myIO = new XgsIO(comPort, baudRate, parity, dataBits, stopBits);
        }

        public static bool SetEmission(bool enable, string address, string sensorCode)
        {
            try
            {
                string str = enable ? "31" : "30";
                string command = string.Format("#{0}{1}U{2}", (object)address, (object)str, (object)sensorCode);
                XgsVacuumController.myIO.Write(command);
                XgsVacuumController.myIO.Read();
                return XgsVacuumController.GetEmissionStatus(address, sensorCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool GetEmissionStatus(string address, string sensorCode)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}32U{1}", (object)address, (object)sensorCode));
                return XgsVacuumController.myIO.Read() == "01";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static double ReadPressureAsDouble(string address, string sensorCode)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}02U{1}", (object)address, (object)sensorCode));
                string s = XgsVacuumController.myIO.Read();
                double result = -999.0;
                double.TryParse(s, out result);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string ReadPressureAsString(string address, string sensorCode)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}02U{1}", (object)address, (object)sensorCode));
                return XgsVacuumController.myIO.Read();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetPressureUnitAsTorr(string address)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}10", (object)address));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetPressureUnitAsMBar(string address)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}11", (object)address));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetPressureUnitAsPascal(string address)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}12", (object)address));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetPressureUnits(string address)
        {
            try
            {
                XgsVacuumController.myIO.Write(string.Format("#{0}13", (object)address));
                return XgsVacuumController.myIO.Read();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set Auto-ON feature for XGS600 gauge.
        /// </summary>
        /// <param name="address">RS232 address use 00, for RS485 use slave ID</param>
        /// <param name="sensorCode">User label for gauge, for example IMG1</param>
        /// <param name="gateSensorCode">User label gating gauge, for example CNV1</param>
        /// <param name="pressureGate">Pressure gate for example '5.0E-03'</param>
        /// <returns>true if no error</returns>
        public static bool SetGaugeAutoON(string address, string sensorCode, string gateSensorCode, string pressureGate)
        {
            try
            {
                myIO.Write(string.Format("#{0}B1U{1}U{2}{3}", address, sensorCode, gateSensorCode, pressureGate));

            }
            catch (Exception)
            {
                Trace.WriteLine("Error when setting up AUTO-ON feature.");
                return false;
            }
            return true;
        }

        public static void Close()
        {
            try
            {
                myIO.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
