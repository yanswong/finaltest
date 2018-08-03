using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomFormLibrary;
using System.Windows.Forms;

namespace VSLDtest
{
    public class External_leak_Parameters
    {
        private static string ext_leakrate;
        private static string ext_leakSN;

        public static string Ext_leakrate
        {
            get { return ext_leakrate; }
            set { ext_leakrate = value; }
        }

        public static string Ext_leakrate1 { get; internal set; }
        public static string Ext_leakrate2 { get; internal set; }

        public static string Ext_leakSN
        {
            get { return ext_leakSN; }
            set { ext_leakSN = value; }
        }

        public static Boolean DoExternal_leak_Parameters()
        {
            resubmit:
            
            Boolean status = true;

            //Enter the external leakrate
            Ext_leakrate = extleak_display();

            if (Ext_leakrate == "Undefined")                      //If the entered value does not meet the requirements, the test process will be terminated 
            {
                status = false;
                goto Y;
            }

            //Enter the external leak serial number
            Ext_leakSN = extleak_SN_display();

            if (Ext_leakSN == "Undefined")                      //If the entered value does not meet the requirements, the test process will be terminated 
            {
                status = false;
                goto Y;
            }

            //@@ Second confirmation by user for the paramters entered above @@//

            DialogResult confirm_param = MessageBox.Show("Extleak Rate:          " + Ext_leakrate + " Std cc/s" + "\nExtleak Serial no. : " + Ext_leakSN + "\n\nPlease confirm the parameters stated above before proceeding\n\nPress 'No' to resubmit.", "Finalization", MessageBoxButtons.YesNo);

            if (confirm_param == DialogResult.No)
            {
                MessageBox.Show("Please resubmit the paramters for the external calibration leak.", "Notice");
                goto resubmit;
            }

            Y:
            return status;
        }

        

        //@@ Prompt user to enter the external leak rate @@//

        public static string extleak_display()
        {
            string display = "";
            ext_leak_prompt myext_leak_prompt = new ext_leak_prompt();

        X:
            myext_leak_prompt.ShowDialog();

            if (myext_leak_prompt.DialogResult == DialogResult.OK)
            {
                display = myext_leak_prompt.Extleak_value;
                myext_leak_prompt.Close();
            }

            else if (myext_leak_prompt.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    myext_leak_prompt.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }


        //@@ Prompt user to enter external leak serial number @@//

        public static string extleak_SN_display()
        {
            string display = "";
            extleak_SN_prompt myextleak_SN_prompt = new extleak_SN_prompt();

        X:
            myextleak_SN_prompt.ShowDialog();

            if (myextleak_SN_prompt.DialogResult == DialogResult.OK)
            {
                display = extleak_SN_prompt.Extleak_SN;
                myextleak_SN_prompt.Close();
            }

            else if (myextleak_SN_prompt.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    myextleak_SN_prompt.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }
    }
}
