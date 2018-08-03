using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginSequence;

namespace Demo2
{
    public class PerformanceTestManager : ITest
    {

        /// When the iTestExec loaded this method will be invoked
        public void TestExecLoaded(ref UUTData _uutData, ref CommonData _commonData)
        {

        }

        /// When we click 'New DUT" button from the iTestExec, it will refer to the sequence file XML 
        /// (Future dev: Show an interface where user can select the right model for testing.)
        public void UserBegins(ref UUTData _uutData, ref CommonData _commonData)
        {

            uut_form uut = new uut_form();
            uut.TopMost = true;

            X:
            uut.ShowDialog();
              
            _uutData = new UUTData();
            
            if(uut.DialogResult == DialogResult.OK)
            {
                _uutData.Model = Model;
                _uutData.SerNum = Serial;

                _commonData.Testplan = @"C:\VSLDtest.PRJ\Sequences\VSLD.xml"; //Obtain the neccesary info about the UUT from sequence file XML created in sequential editor
                uut.Dispose();
            }
                
            else if(uut.DialogResult == DialogResult.Cancel)
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation" , MessageBoxButtons.YesNo);

                if(result == DialogResult.Yes)
                {
                    uut.Dispose();
                }

                else if(result == DialogResult.No)
                {
                    goto X;           
                }
            }
        }

        static string model;
        static string serial;
        static string option;
        static string spec;

        public string Model                 //Update and retrieve model name
        {
            get
            { return model; }

            set
            { model = value; }
        } 

        public string Serial                //Update and retrieve serial number
        {
            get { return serial; }

            set { serial = value; }
        }

        public string Option
        {
            get { return option; }

            set { option = value; }
        }

        public string Spec
        {
            get { return spec; }

            set { spec = value; }
        }

        public void UserSetups(ref UUTData _uutData, ref CommonData _commonData)
        {

        }

        ///To set constant variable for testing purpose
        public void TestSetup(ref TestInfo myTestInfo, ref UUTData _uutData, ref CommonData _commonData)
        {
            
        }

        /// This is the main interaction between iTestExec software with this project, and when user press "start" button
        public TestInfo DoTests(TestInfo myTestInfo, ref UUTData myUUTData, ref CommonData myCommonData)
        {
            try
            {
                switch (myTestInfo.GroupLabel)                      //Swtich between testing group enlisted for the UUT
                {
                    case "DcvGroup":
                        {
                            
                        }
                        break;

                    case "DcrGroup":
                        {
                            
                        }
                        break;

                    case "CurrentGroup":
                        {

                        }
                        break;

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
        /// These two function below, TestCleanup and UserCleanup happens after the DoTests process is completed. 
        public void TestCleanup(ref TestInfo myTestInfo, ref UUTData _uutData, ref CommonData _commonData)
        {
            
        }

        public void UserCleanup(ref UUTData _uutData, ref CommonData _commonData)
        {
            
        }

        
    }
}
