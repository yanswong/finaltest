using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSequence;
using CustomFormLibrary;
using System.Windows.Forms;
using VSLDtest.SubGroupTest;
using SerialPortIO;
using SystemIO;
using System.Diagnostics;
using Agilent.TMFramework.InstrumentIO;
using System.Data.SqlClient;
using System.Configuration;
using VSLDtest.TestForms;
using PreheatMeasure;
using System.Drawing;
using System.Threading;
using System.IO.Ports;


namespace VSLDtest
{
    public class PerformanceTestManager : ITest
    {
        private string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
        bool isLDWithIO = false;
        bool isLDWithWireless = false;
        string iteSlot = "";
        private object myLD;
        private VSLeakDetector myLD1;
        private VSLeakDetector myLD2;
        private string comPort1 = "COM4";
        private string comPort2 = "COM7";
        private string intCalLeakSN1 = "";
        private string intCalLeakSN2 = "";
        private string extCalLeakSN1 = "";
        private string extCalLeakSN2 = "";
        private string extCalLeakPN = "";
        private int screenWidth = 0;
        private int screenHeight = 0;
        private int startLocY = 0;
        private int startLocX = 0;

        //@@ ITest Exec Initial boot @@//
        public void TestExecLoaded(ref UUTData _uutData, ref CommonData _commonData)
        {
            Size area = Screen.PrimaryScreen.WorkingArea.Size;
            this.screenHeight = area.Height;
            this.screenWidth = area.Width;
            if (iteSlot.Contains("P1"))
            {
                this.startLocY = 10;
                this.startLocX = (this.screenHeight / 2) - 50;
            }
            else if (iteSlot.Contains("P2"))
            {
                this.startLocY = (this.screenWidth / 2) + 10;
                this.startLocX = (this.screenHeight / 2) - 50;
            }
            else
            {

            }
        }


        //@@ When 'New UUT' button is pressed @@//
        public void UserBegins(ref UUTData _uutData, ref CommonData _commonData)
        {
            iteSlot = _commonData.TestStation + ": "; //P1 or P2

           

            //StartupForm startupform = new StartupForm();
            
            _commonData = new CommonData();
            try
            {
            X:
                //startupform.ShowDialog();                                             
                _uutData = new UUTData();

                //if (startupform.DialogResult == DialogResult.OK)                    //When button apply is entered
                //{
                FormModelNumber formModel = new FormModelNumber();
                formModel.StartPosition = FormStartPosition.Manual;
                formModel.Location = new Point(this.startLocX, this.startLocY);
                formModel.ShowDialog();
                if (formModel.DialogResult == DialogResult.OK)
                {
                    _uutData.Model = formModel.ModelNumber;
                }
                else
                {
                    _commonData.Mode = "Abort";
                    return;
                }
                FormSerialNumber formSerial = new FormSerialNumber();
                formSerial.StartPosition = FormStartPosition.Manual;
                formSerial.Location = new Point(this.startLocX, this.startLocY);
                formSerial.ShowDialog();
                if (formSerial.DialogResult == DialogResult.OK)
                {
                    _uutData.SerNum = formSerial.SerialNumber;
                }
                else
                {
                    _commonData.Mode = "Abort";
                    return;
                }
                //_uutData.Model = startupform.Model;                             //Save the model number
                //_uutData.SerNum = startupform.Serial;                           //Save the serial number
                //_uutData.Options = "1";                            // Yan han use uutData.option as slot number, maybe need to change later
                
                _uutData.Options = iteSlot.Contains("P1") ? "1" : "2";
                _commonData.Slot = iteSlot.Contains("P1") ? "1" : "2";
                // Call Internal Cal Leak Forms
                FormIntCalLeak formIntCalLeak = new FormIntCalLeak();
                formIntCalLeak.ShowDialog();
                if (formIntCalLeak.DialogResult == DialogResult.OK)
                {
                    if (iteSlot.Contains("P1"))
                        intCalLeakSN1 = formIntCalLeak.SerialNumber;
                    else
                        intCalLeakSN2 = formIntCalLeak.SerialNumber;
                }
                else
                {
                    _commonData.Mode = "Abort";
                    return;
                }

                FormExtCalLeak formExtCalLeak = new FormExtCalLeak();
                formExtCalLeak.StartPosition = FormStartPosition.Manual;
                formExtCalLeak.Location = new Point(this.startLocX, this.startLocY);
                formExtCalLeak.ShowDialog();
                if (formExtCalLeak.DialogResult == DialogResult.OK)
                {
                    if (iteSlot.Contains("P1"))
                        extCalLeakSN1 = formExtCalLeak.SerialNumber;
                    else
                        extCalLeakSN2 = formExtCalLeak.SerialNumber;

                    extCalLeakPN = formExtCalLeak.ModelNumber;
                    
                }
                else
                {
                    _commonData.Mode = "Abort";
                    return;
                }

// YS Wong remove as there's no setting on the test station.   13 Jul 2018

/*                FormLD_Options formOptions = new FormLD_Options();
                formOptions.StartPosition = FormStartPosition.Manual;
                formOptions.Location = new Point(this.startLocX, this.startLocY);
                formOptions.ShowDialog();
                if (formOptions.DialogResult == DialogResult.OK)
                {
                    isLDWithIO = formOptions.isIoChecked;
                    isLDWithWireless = formOptions.isWirelessChecked;
                }
                else
                {
                    _commonData.Mode = "Abort";
                    return;
                }
*/
                

                //_uutData.PCASerNum = startupform.SpectroSerial;
               
                _commonData.Testplan = @"C:\VSLDtest.PRJ\Sequences\VSLD.xml";   //Obtain the neccesary info about the UUT from sequence file XML created in sequential editor
               
                //startupform.Dispose();

                // 25-May-17: commented out the windows to get all cal-leak information, instead use database to get all data by providing serial numbers of cal-leak
                //Boolean status = UUT_Parameters.DoUUT_Parameters();             //Prompt user to enter the details about the UUT initially (One time prompt only)
                bool status = false;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    CalLeakData intCalLeakData;
                    if (iteSlot.Contains("P1"))
                    {
                        _commonData.Slot = "1";
                        intCalLeakData = SqlHelper.GetCalLeakData(conn, intCalLeakSN1, false);
                        if (intCalLeakData != null)
                        {
                            UUT_Parameters.Stdleak1 = Math.Round(intCalLeakData.LeakRate, 8).ToString();
                            UUT_Parameters.Stdleak_SN1 = intCalLeakData.SerialNumber;
                            UUT_Parameters.Stdleak_Exp_date1 = intCalLeakData.TestDate.AddMonths(18).ToString("MM dd yyyy");
                            UUT_Parameters.Stdleak_temp1 = intCalLeakData.UUTTemp.ToString();
                            UUT_Parameters.Stdleak_factor1 = intCalLeakData.Factor.ToString();
                            status = true;
                            CalleakinfoDB.iteSlot = iteSlot;
                            CalleakinfoDB calleak_infoDB = new CalleakinfoDB();
                            
                            calleak_infoDB.ShowDialog();
                        }
                    }
                        
                    else
                    {
                        intCalLeakData = SqlHelper.GetCalLeakData(conn, intCalLeakSN2, false);
                        if (intCalLeakData != null)
                        {
                            _commonData.Slot = "2";
                            UUT_Parameters.Stdleak2 = Math.Round(intCalLeakData.LeakRate, 8).ToString();
                            UUT_Parameters.Stdleak_SN2 = intCalLeakData.SerialNumber;
                            UUT_Parameters.Stdleak_Exp_date2 = intCalLeakData.TestDate.AddMonths(18).ToString("MM dd yyyy");
                            UUT_Parameters.Stdleak_temp2 = intCalLeakData.UUTTemp.ToString();
                            UUT_Parameters.Stdleak_factor2 = intCalLeakData.Factor.ToString();
                            status = true;
                            CalleakinfoDB.iteSlot = iteSlot;
                            CalleakinfoDB calleak_infoDB = new CalleakinfoDB();
                            
                            calleak_infoDB.ShowDialog();
                        }
                    }
                        
                    if (status == false)                                            //When user press 'Yes' during the prompt on whether to abort the test
                    {
                        if (iteSlot.Contains("P1"))
                            MessageBox.Show("Internal Calibrated Leak with SN:" + intCalLeakSN1+ " data wasn't found from database. Make sure that the calibrated leak was tested and pass.","INT CAL LEAK Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        else
                            MessageBox.Show("Internal Calibrated Leak with SN:" + intCalLeakSN2+" data wasn't found from database. Make sure that the calibrated leak was tested and pass.", "INT CAL LEAK Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        _commonData.Mode = "Abort";                                 //Abort the test
                    }
                    else
                    {
                        // get data for ext leak 
                        //status = External_leak_Parameters.DoExternal_leak_Parameters(); //Prompt user to enter the details about the external calibrated leak initially (One time prompt only)
                        ExtCalLeakData extCalLeakData;
                        if (iteSlot.Contains("P1"))
                        {
                            extCalLeakData = SqlHelper.GetExtCalLeakData(conn, extCalLeakPN, extCalLeakSN1);
                            if (extCalLeakData != null)
                            {
                                External_leak_Parameters.Ext_leakrate1 = extCalLeakData.LeakRate.ToString("0.0E00");


                                //YS Wong add in ext cal due date notify 23 Jul 
                                
                                //string extcalleakduedate = extCalLeakData.CalDueDate
                                //cdd = duedate.GetDateTime(0).ToShortDateString();
                                
                                DateTime excaldate1 = extCalLeakData.CalDueDate;
                                DateTime nowdate2 = DateTime.Now.Date;
                                int result = DateTime.Compare(excaldate1, nowdate2);
                                string relationship;

                                if (result < 0)
                                {
                                    relationship = "Today's date has over the calibration due date\n";
                                    Trace.WriteLine(relationship + "CALIBRATION DUE DATE OVER STOP TESTING");
                                    MessageBox.Show("Calibration due date OVER!! the test will stop here");
                                    _commonData.Mode = "Abort";
                                }
                                else if (result == 0)
                                {
                                    relationship = "Today is End of calibration date\n";
                                    Trace.WriteLine(relationship + "CALIBRATION DUE DATE OVER STOP TESTING");
                                    MessageBox.Show("Calibration due date OVER!! the test will stop here");
                                    _commonData.Mode = "Abort";
                                }
                                else
                                {
                                    TimeSpan diff = excaldate1 - nowdate2;
                                    int days = (int)diff.TotalDays;
                                    if (days < 30)
                                    { MessageBox.Show("Calibration due date of "+ extCalLeakSN1 +" end in " + days + " days"); }
                                    else
                                        Trace.WriteLine(extCalLeakSN1+" is calibrated");
                                }
                                
                                status = true;
                            }
                            else
                            {
                                status = false;
                                MessageBox.Show("Enternal Calibrated Leak with SN:" + extCalLeakSN1 + " data wasn't found from database.", "Ext CAL LEAK Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _commonData.Mode = "Abort";
                                                                  
                            }
                        }
                        else
                        {
                            extCalLeakData = SqlHelper.GetExtCalLeakData(conn, extCalLeakPN, extCalLeakSN2);
                            if (extCalLeakData != null)
                            {
                                External_leak_Parameters.Ext_leakrate2 = extCalLeakData.LeakRate.ToString("0.0E00");
                                
                                DateTime excaldate1 = extCalLeakData.CalDueDate;
                                DateTime nowdate2 = DateTime.Now.Date;
                                int result = DateTime.Compare(excaldate1, nowdate2);
                                string relationship;

                                if (result < 0)
                                {
                                    relationship = "Today's date has over the calibration due date\n";
                                    Trace.WriteLine(relationship + "CALIBRATION DUE DATE OVER STOP TESTING");
                                    MessageBox.Show("Calibration due date OVER!! the test will stop here");
                                    _commonData.Mode = "Abort";
                                }
                                else if (result == 0)
                                {
                                    relationship = "Today is End of calibration date\n";
                                    Trace.WriteLine(relationship + "CALIBRATION DUE DATE OVER STOP TESTING");
                                    MessageBox.Show("Calibration due date OVER!! the test will stop here");
                                    _commonData.Mode = "Abort";
                                }
                                else
                                {
                                    TimeSpan diff = excaldate1 - nowdate2;
                                    int days = (int)diff.TotalDays;
                                    if (days < 30)
                                    { MessageBox.Show("Calibration due date of " + extCalLeakSN2 + " end in " + days + " days"); }
                                    else
                                        Trace.WriteLine(extCalLeakSN2 + " is calibrated");
                                }

                                status = true;
                            }
                            else
                            {
                                status = false;
                                MessageBox.Show("Enternal Calibrated Leak with SN:" + extCalLeakSN2 + " data wasn't found from database.", "Ext CAL LEAK Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _commonData.Mode = "Abort";

                            }
                        }
                    }


                }

                

                // }

                //else if (startupform.DialogResult == DialogResult.Cancel)           //When button cancel is entered
                //{
                //    DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo);

                //    if (result == DialogResult.Yes)                                 //When Yes is entered
                //    {
                //        startupform.Dispose();
                //    }

                //    else if (result == DialogResult.No)                             //When No is entered
                //    {
                //        goto X;
                //    }
                //}
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    

        //@@ When the 'START' button is pressed @@//
        public void UserSetups(ref UUTData _uutData, ref CommonData _commonData)
        {
            
        recheck_connection:

            try
            {
                if (iteSlot.Contains("P1"))
                {
                    myLD1 = new VSLeakDetector(comPort1);
                    myLD1.iteSlot = iteSlot;
                    myLD1.Open();
                    //Helper.comPort = comPort1;
                }
                else if (iteSlot.Contains("P2"))
                {
                    myLD2 = new VSLeakDetector(comPort2);
                    myLD2.iteSlot = iteSlot;
                    myLD2.Open();
                    //Helper.comPort = comPort2;
                }
                else
                {

                }

                //Initialize data acquisition system

                recheck:
                string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                string DMMstatus = SQL_34970A.Read_DMM_Status(conStr);

                if (DMMstatus != "No")
                {
                    MessageBox.Show("DMM locked", "warning", MessageBoxButtons.OK);
                    Thread.Sleep(5000);
                    goto recheck;
                }

                InstrumentIO.DAS_initialize();

                //Accquire the voltage requirement based on the model number and channel number based on the slot number of the tester
                InstrumentIO.Init_UUT_Voltage_Switches(_uutData.Model, _uutData.Options);
            }

            catch (Agilent.TMFramework.InstrumentIO.VisaException ex)               //When there is connection error between the 34970A and the PC
            {
                DialogResult connection = MessageBox.Show(ex.Message + "\n\nPlease check the connection between the 34970A data accquisition system and the PC.", "WARNING", MessageBoxButtons.RetryCancel);

                if (connection == DialogResult.Retry)
                {
                    goto recheck_connection;
                }

                else
                {
                    Application.Exit();
                }
            }
        }


        //@@ This method runs in between every test group @@//
        public void TestSetup(ref TestInfo myTestInfo, ref UUTData _uutData, ref CommonData _commonData)
        {
            //_commonData.
            //if (_commonData.Mode.Contains("Abort"))
            //    _commonData.Mode = "Abort";
        }


        //@@ Test process starts here @@//
        public TestInfo DoTests(TestInfo myTestInfo, ref UUTData myUUTData, ref CommonData myCommonData)
        {
            try
            {
                switch (myTestInfo.GroupLabel)
                {
                    case "Seq 5.0":
                        {
                            switch (myTestInfo.TestLabel)
                            {
                                case "5.0.1 RecordUUTInfo":
                                    {
                                        var uutInfoList = new List<string>();
                                        if (iteSlot.Contains("P1"))
                                        {
                                            uutInfoList.Add(intCalLeakSN1);  // Serial number internal cal leak
                                            uutInfoList.Add(extCalLeakPN);  // Ext cal leak part number
                                            uutInfoList.Add(extCalLeakSN1);  // Ext cal leak serial number
                                        }
                                        else
                                        {
                                            uutInfoList.Add(intCalLeakSN2);  // Serial number internal cal leak
                                            uutInfoList.Add(extCalLeakPN);  // Ext cal leak part number
                                            uutInfoList.Add(extCalLeakSN2);  // Ext cal leak serial number
                                        }
                                        
                                        Seq5_0.DoUUTInfoTest(ref myTestInfo, myUUTData, myCommonData, uutInfoList.ToArray());
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "Seq 5.1":
                        {
                            Trace.WriteLine(iteSlot + "Model Number Selected:  " + myUUTData.Model);
                            Trace.WriteLine(iteSlot + "Serial Number:          " + myUUTData.SerNum);
                            Trace.WriteLine(iteSlot + "Selected slot #:        " + myUUTData.Options);

                            Seq5_1.iteSlot = iteSlot;
                            Seq5_1.locX = this.startLocX;
                            Seq5_1.locY = this.startLocY;
                            if (iteSlot.Contains("P1"))
                                Seq5_1.DoSeq5_1(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_1.DoSeq5_1(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.2":
                        {
                            Seq5_2.iteSlot = iteSlot;
                            Seq5_2.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            Seq5_2.locX = this.startLocX;
                            Seq5_2.locY = this.startLocY;
                            
                            if (iteSlot.Contains("P1"))
                                Seq5_2.DoSeq5_2(myLD1, ref myTestInfo, ref myUUTData, ref myCommonData);
                            else
                                Seq5_2.DoSeq5_2(myLD2, ref myTestInfo, ref myUUTData, ref myCommonData);
                        }
                        break;
                    case "Seq 5.3":                                    
                        {
                            Seq5_3.iteSlot = iteSlot;
                            Seq5_3.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            Seq5_3.locX = this.startLocX;
                            Seq5_3.locY = this.startLocY;
                            if (iteSlot.Contains("P1"))
                                Seq5_3.DoSeq5_3(myLD1, ref myTestInfo, ref myUUTData, ref myCommonData);
                            else
                                Seq5_3.DoSeq5_3(myLD2, ref myTestInfo, ref myUUTData, ref myCommonData);
                        }
                        break;
                    case "Seq 5.4":                                  
                        {
                            Seq5_4.iteSlot = iteSlot;
                            Seq5_4.locX = this.startLocX;
                            Seq5_4.locY = this.startLocY;
                            Seq5_4.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_4.DoSeq5_4(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_4.DoSeq5_4(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.6":
                        {
                            Seq5_6.iteSlot = iteSlot;
                           // Seq5_6.locX = this.startLocX;
                           // Seq5_6.locY = this.startLocY;
                            Seq5_6.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_6.DoSeq5_6(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_6.DoSeq5_6(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.7":
                        {
                            Seq5_7.iteSlot = iteSlot;
                            Seq5_7.locX = this.startLocX;
                            Seq5_7.locY = this.startLocY;
                            Seq5_7.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_7.DoSeq5_7(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_7.DoSeq5_7(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.8":
                        {
                            Seq5_8.iteSlot = iteSlot;
                            Seq5_8.locX = this.startLocX;
                            Seq5_8.locY = this.startLocY;
                            Seq5_8.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_8.DoSeq5_8(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_8.DoSeq5_8(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.9":
                        {
                            //retest:
                            Seq5_9.iteSlot = iteSlot;
                            Seq5_9.locX = this.startLocX;
                            Seq5_9.locY = this.startLocY;
                            Seq5_9.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            Seq5_9.SlotNum = Convert.ToInt32(myUUTData.Options);
                            if (iteSlot.Contains("P1"))
                            {
                                Seq5_9.DoSeq5_9(myLD1, ref myTestInfo, ref myUUTData);
                                if (myCommonData.IsFailureExist)
                                {
                                    MessageBox.Show("fail", "retest?", MessageBoxButtons.OK);
                                    myCommonData.Mode="Abort";
                                    //goto retest;
                                }
                            }
                            else
                                Seq5_9.DoSeq5_9(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.11":
                        {
                            Seq5_11.iteSlot = iteSlot;
                            Seq5_11.locX = this.startLocX;
                            Seq5_11.locY = this.startLocY;
                            Seq5_11.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_11.DoSeq5_11(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_11.DoSeq5_11(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.12":
                        {
                            Seq5_12.iteSlot = iteSlot;
                            Seq5_12.locX = this.startLocX;
                            Seq5_12.locY = this.startLocY;
                            //Seq5_12.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            Seq5_12.IsWirelessEnabled = this.isLDWithWireless;
                            Seq5_12.IsIOEnabled = this.isLDWithIO;
                            if (iteSlot.Contains("P1"))
                                Seq5_12.DoSeq5_12(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_12.DoSeq5_12(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    case "Seq 5.13":
                        {
                            Seq5_13.iteSlot = iteSlot;
                            Seq5_13.locX = this.startLocX;
                            Seq5_13.locY = this.startLocY;
                            //Seq5_13.comPort = this.iteSlot.Contains("P1") ? this.comPort1 : this.comPort2;
                            if (iteSlot.Contains("P1"))
                                Seq5_13.DoSeq5_13(myLD1, ref myTestInfo, ref myUUTData);
                            else
                                Seq5_13.DoSeq5_13(myLD2, ref myTestInfo, ref myUUTData);
                        }
                        break;
                    default:
                            break;
                }
            }

            catch(Exception ex)
            {
                // DO Logging here for exception
                Trace.WriteLine(ex.Message);
                return myTestInfo;
            }

            return myTestInfo;
        }

        public void TestCleanup(ref TestInfo myTestInfo, ref UUTData _uutData, ref CommonData _commonData)
        {
            
        }

        public SerialError EventType { get; }
        public string SN { get; set; }

        public void UserCleanup(ref UUTData _uutData, ref CommonData _commonData)
        {
            
                string conStr = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
                if (iteSlot.Contains("P1"))
                {
                    SQL_XGS.Update_XGS("1", "No");
                    SQL_34970A.Update_DMM_Status("1", "No", conStr);
                    InstrumentIO.Close_Extleak(1);
                
                 myLD1.Close();
               //   myLD1 = null;
                   
                   
                }
            else if (iteSlot.Contains("P2"))
             {
                    SQL_XGS.Update_XGS("2", "No");
                    SQL_34970A.Update_DMM_Status("1", "No", conStr);
                InstrumentIO.Close_Extleak(2);
              
                    myLD2.Close();
                    myLD2 = null;
             }
                else
                {

                }
            
          
        }
    
    }
}
