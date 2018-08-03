using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomFormLibrary;
using System.Windows.Forms;

namespace VSLDtest.SubGroupTest
{
    public class UUT_Parameters
    {
        private static string stdleak;
        private static string stdleak_SN;
        private static string stdleak_exp_date;
        private static string stdleak_temp;
        private static string stdleak_tempfactor;
        public static int locX { get; set; }
        public static int locY { get; set; }

        public static string Stdleak
        {
            get { return stdleak; }
            set { stdleak = value; }
        }

        public static string Stdleak_SN
        {
            get { return stdleak_SN; }
            set { stdleak_SN = value; }
        }

        public static string Stdleak_Exp_date
        {
            get { return stdleak_exp_date; }
            set { stdleak_exp_date = value; }
        }

        public static string Stdleak_temp
        {
            get { return stdleak_temp; }
            set { stdleak_temp = value; }
        }

        public static string stdleak_tempfactor2
        {
            get { return stdleak_tempfactor; }
            set { stdleak_tempfactor = value; }
        }

        public static string Stdleak1 { get; internal set; }
        public static string Stdleak_SN1 { get; internal set; }
        public static string Stdleak_Exp_date1 { get; internal set; }
        public static string Stdleak_temp1 { get; internal set; }
        public static string Stdleak2 { get; internal set; }
        public static string Stdleak_SN2 { get; internal set; }
        public static string Stdleak_Exp_date2 { get; internal set; }
        public static string Stdleak_temp2 { get; internal set; }
        public static string Stdleak_factor1 { get; internal set; }
        public static string Stdleak_factor2 { get; internal set; }

        public static Boolean DoUUT_Parameters()
        {
            resubmit:

            Boolean status = true;

            //@@ Standard leak rate @@//

            Stdleak = cal_leak_LabelDisplay();

            if (Stdleak == "Undefined")                      //If the entered value does not meet the requirements, the test process will be terminated 
            {
                status = false;
                goto Y;
            }

            //@@ Standard leak Serial Number @@//

            Stdleak_SN = stdleak_SN_display();
            
            if(Stdleak_SN == "Undefined")                   //If the entered value does not meet the requirements, the test process will be terminated 
            {
                status = false;
                goto Y;
            }

            //@@ Expiration date @@//

            Stdleak_Exp_date = cal_leak_expdate_LabelDisplay();

            if (Stdleak_Exp_date == "Undefined")            //If the entered value does not meet the requirements, the test process will be terminated
            {
                status = false;
                goto Y;
            }

            //@@ Stdleak calibration temperature @@//

            Stdleak_temp = cal_leak_temp_LabelDisplay();

            if (Stdleak_temp == "Undefined")                //If the entered value does not meet the requirements, the test process will be terminated
            {
                status = false;
                goto Y;
            }

            //@@ Stdleak temperature correction factor @@//

            Stdleak_factor1 = cal_leak_tempfactor_LabelDisplay();

            if (Stdleak_factor1 == "Undefined")          //If the entered value does not meet the requirements, the test process will be terminated
            {
                status = false;
                goto Y;
            }

            //@@ Second confirmation by user for the paramters entered above @@//

            DialogResult confirm_param = MessageBox.Show("Stdleak Rate:           " + Stdleak + " Std cc/s" + "\nStdleak Serial no. : " + Stdleak_SN + "\nExpiration Date:      " + Stdleak_Exp_date + "\nTemperature:         " + Stdleak_temp + " Celsius" + "\nTempfactor:          " + Stdleak_factor1 + " Celsius\n\nPlease confirm the parameters stated above before proceeding\n\nPress 'No' to resubmit.", "Finalization", MessageBoxButtons.YesNo);

            if (confirm_param == DialogResult.No)
            {
                MessageBox.Show("Please resubmit the paramters for the internal calibration leak.", "Notice");
                goto resubmit;
            }        

            Y:
            return status;
        }



        //@@ Prompt user to enter the stdleak rate of the internal cal leak @@//

        public static string cal_leak_LabelDisplay()
        {
            cal_leak_prompt my_cal_leak_prompt = new cal_leak_prompt();
            string display = "";

        X:                                                                              
            my_cal_leak_prompt.ShowDialog();

            if (my_cal_leak_prompt.DialogResult == DialogResult.OK)                 //Once user enter a value inside the textbox, the value will be returned and stored
            {
                display = my_cal_leak_prompt.Stdleak_value;
                my_cal_leak_prompt.Close();
            }

            else if (my_cal_leak_prompt.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_cal_leak_prompt.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }

        //@@ Prompt user to enter the internal cal leak serial number @@//

        public static string stdleak_SN_display()
        {
            stdleak_SN_prompt my_stdleak_SN_prompt = new stdleak_SN_prompt();
            string display = "Undefined";

        X:
            my_stdleak_SN_prompt.ShowDialog();

            if(my_stdleak_SN_prompt.DialogResult == DialogResult.OK)
            {
                display = stdleak_SN_prompt.Std_serialnum;
                my_stdleak_SN_prompt.Close();
            }
           
            else if(my_stdleak_SN_prompt.DialogResult == DialogResult.Abort)
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_stdleak_SN_prompt.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }

        //@@ Prompt user to enter the internal cal leak expiration date @@//

        public static string cal_leak_expdate_LabelDisplay()
        {
            string display = "";
            cal_leak_expdate my_cal_leak_expdate = new cal_leak_expdate();

        X:
            my_cal_leak_expdate.ShowDialog();

            if (my_cal_leak_expdate.DialogResult == DialogResult.OK)                 //Once user enter a value inside the textbox, the value will be returned and stored
            {
                display = my_cal_leak_expdate.Expdate;
                my_cal_leak_expdate.Close();
            }

            else if (my_cal_leak_expdate.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_cal_leak_expdate.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }

        //@@ Prompt user to enter the temperature of the internal cal leak @@//

        public static string cal_leak_temp_LabelDisplay()
        {
            cal_leak_temp my_cal_leak_temp = new cal_leak_temp();
            string display = "";

        X:
            my_cal_leak_temp.ShowDialog();

            if (my_cal_leak_temp.DialogResult == DialogResult.OK)                 //Once user enter a value inside the textbox, the value will be returned and stored
            {
                display = my_cal_leak_temp.Temp_value;
                my_cal_leak_temp.Close();
            }

            else if (my_cal_leak_temp.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_cal_leak_temp.Dispose();
                }

                else if (result == DialogResult.No)                            //If user reject the exit process, the user prompt will pop up again 
                {
                    goto X;
                }
            }
            return display;
        }

        //@@ Prompt user to enter the stdleak temperature correction factor @@//

        public static string cal_leak_tempfactor_LabelDisplay()
        {
            cal_leak_tempfactor my_cal_leak_tempfactor = new cal_leak_tempfactor();
            string display = "";

        X:
            my_cal_leak_tempfactor.ShowDialog();

            if (my_cal_leak_tempfactor.DialogResult == DialogResult.OK)                 //Once user enter a value inside the textbox, the value will be returned and stored
            {
                display = my_cal_leak_tempfactor.Tempfactor;
                my_cal_leak_tempfactor.Close();
            }

            else if (my_cal_leak_tempfactor.DialogResult == DialogResult.Abort)         //If the user abort the user prompt about entering the details of the UUT, the whole test process will be terminated
            {
                DialogResult result = MessageBox.Show("The following user prompts cannot be skipped. \nAre you sure you want to ABORT the test?", "WARNING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    display = "Undefined";
                    my_cal_leak_tempfactor.Dispose();
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
