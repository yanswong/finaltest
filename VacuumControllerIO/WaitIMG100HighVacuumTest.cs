// Type: VacuumControllerIO.WaitIMG100HighVacuumTest
// Assembly: VacuumControllerIO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C1A7622-3733-44B0-841A-E16286973DCF
// Assembly location: Z:\VPD LDA\Z_General_Hairus\Projects\Intern Yan Han\IMG100\VacuumControllerIO.dll

using System;
using System.Windows.Forms;

namespace VacuumControllerIO
{
  public class WaitIMG100HighVacuumTest
  {
    public double GetHighVacuumReading(string comPort, string address, string sensorCodeImg, string sensorCodeTc)
    {
      try
      {
        FormHiVacTest formHiVacTest = new FormHiVacTest();
        formHiVacTest.ComPortStr = comPort;
        formHiVacTest.Address = address;
        formHiVacTest.SensorCodeImg = sensorCodeImg;
        formHiVacTest.SensorCodeTc = sensorCodeTc;
        formHiVacTest.DialogFlag = DialogResult.No;
        int num = (int) formHiVacTest.ShowDialog();
        return formHiVacTest.PressureReading;
      }
      catch (Exception ex)
      {
        return -1.0;
      }
    }
  }
}
