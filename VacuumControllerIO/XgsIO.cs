// Type: VacuumControllerIO.XgsIO
// Assembly: VacuumControllerIO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C1A7622-3733-44B0-841A-E16286973DCF
// Assembly location: Z:\VPD LDA\Z_General_Hairus\Projects\Intern Yan Han\IMG100\VacuumControllerIO.dll

using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace VacuumControllerIO
{
  public class XgsIO
  {
    private string _Terminator = "\r";
    private int _Timeout = 10;
    private bool Success = false;
    private bool _enableTraceLog = false;
    private int _Delay = 10;
    private SerialPort mySerialPort;

    public StringBuilder Message { get; set; }

    public int Timeout
    {
      get
      {
        return this._Timeout;
      }
      set
      {
        this._Timeout = value;
      }
    }

    public string Terminator
    {
      get
      {
        return this._Terminator;
      }
      set
      {
        this._Terminator = value;
      }
    }

    public bool EnableTraceLog
    {
      get
      {
        return this._enableTraceLog;
      }
      set
      {
        this._enableTraceLog = value;
      }
    }

    public XgsIO(string portName)
      : this(portName, 9600, Parity.None, 8, StopBits.One)
    {
    }

    public XgsIO(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    {
      this.mySerialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
      this.Message = new StringBuilder();
    }

    public XgsIO()
      : this("COM6", 9600, Parity.None, 8, StopBits.One)
    {
    }

    public void Open()
    {
      try
      {
        if (this.mySerialPort == null)
          throw new Exception("Serial Port Instance must be created before opening the COM port");
        this.mySerialPort.Open();
      }
      catch (Exception ex)
      {
        if (this._enableTraceLog)
          Trace.WriteLine(ex.Message);
        throw ex;
      }
    }

    public void Close()
    {
      try
      {
        if (mySerialPort == null)
          return;
        if (mySerialPort.IsOpen)
          mySerialPort.Close();
        
                mySerialPort.Close();
                mySerialPort = (SerialPort)null;
            }
      catch (Exception ex)
      {
        if (this._enableTraceLog)
          Trace.WriteLine(ex.Message);
        throw ex;
      }
    }

    public void Write(string command)
    {
      try
      {
        this.Write(command, true);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public void Write(string command, bool clearPrevMsg)
    {
      try
      {
        if (this.mySerialPort == null)
          throw new Exception("RS232IO wasn't set to an instance of an object");
        if (!this.mySerialPort.IsOpen)
          this.Open();
        if (clearPrevMsg)
          this.Message = new StringBuilder();
        this.mySerialPort.Write(command + "\r");
        Thread.Sleep(this._Delay);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string Read()
    {
      try
      {
        DateTime now1 = DateTime.Now;
        Thread.Sleep(10);
        DateTime now2 = DateTime.Now;
        StringBuilder stringBuilder;
        for (stringBuilder = new StringBuilder(); (now2 - now1).TotalSeconds < (double) this.Timeout && !((object) stringBuilder).ToString().Contains(this._Terminator) && !((object) stringBuilder).ToString().Contains(">"); now2 = DateTime.Now)
        {
          Thread.Sleep(100);
          stringBuilder.Append(this.mySerialPort.ReadExisting());
        }
        if ((now2 - now1).TotalSeconds > (double) this.Timeout)
          throw new Exception(string.Format("Timeout attempting to read serial port. Message :\r\n {0}", (object) ((object) stringBuilder).ToString()));
        return ((object) stringBuilder).ToString().Split(new char[1]
        {
          '>'
        })[1].Split(new char[1]
        {
          '\r'
        })[0];
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
