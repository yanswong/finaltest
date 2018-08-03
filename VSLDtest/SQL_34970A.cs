using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;

namespace VSLDtest
{
    public class SQL_34970A
    {
        public static string cdd { get; set; }
        public static void Calibrated(string conn)
        {
            var myConStr = conn;
            using (SqlConnection my_sql = new SqlConnection(myConStr))
            {
                my_sql.Open();
                SqlCommand command = new SqlCommand("Select CalibrationDueDate from vpdof.dbo.EquipmentTracking where equipmentid ='5' and serialnumber = 'my44072866';", my_sql);
                SqlDataReader duedate = command.ExecuteReader();
                while (duedate.Read())
                {
                    cdd = duedate.GetDateTime(0).ToShortDateString();
                }
                DateTime date1 = DateTime.Parse(cdd);
                DateTime date2 = DateTime.Now.Date;
                int result = DateTime.Compare(date1, date2);
                string relationship;

                if (result < 0)
                {
                    relationship = "Today's date has over the calibration due date\n";
                    Trace.WriteLine(relationship+"CALIBRATION DUE DATE OVER STOP TESTING");
                    MessageBox.Show("Calibration due date OVER!!");
                    Thread.CurrentThread.Abort();
                }
                else if (result == 0)
                {
                    relationship = "Today is End of calibration date\n";
                    Trace.WriteLine(relationship + "CALIBRATION DUE DATE OVER STOP TESTING");
                    MessageBox.Show("Calibration due date OVER!!");
                }
                else
                {
                    TimeSpan diff = date1 - date2;
                    int days = (int)diff.TotalDays;
                    if (days < 30)
                    { MessageBox.Show("Calibration due date of 34970A end in " + days + " days"); }
                    else
                        Trace.WriteLine("34970A is calibrated");
                }
                  
                my_sql.Close();
            }
        }
        public static void Update_DMM_Status(string slotnum, string islocked, string conStr)
        {
            var myConStr = conStr;
            using (SqlConnection my_sql = new SqlConnection(myConStr))
            {
                try
                {
                    my_sql.Open();

                    // SqlCommand command = new SqlCommand("UPDATE [34970A_LockStatus]" + "SET [Slot No.] = " + slotnum +", IsLocked = '" + islocked + "';", my_sql);
                    // SqlCommand command = new SqlCommand("UPDATE [34970A_LockStatus]" + "SET  IsLocked = '" + islocked + "' where [Tester] = " + slotnum + "; ", my_sql);
                    SqlCommand command = new SqlCommand("UPDATE [34970A_LockStatus]" + "SET  IsLocked = '" + islocked + "' where [Tester] = '1'; ", my_sql);   // hardcode tester =1 
                    command.ExecuteNonQuery();

                    if (islocked == "Yes")
                        Trace.WriteLine("Locked successfully");

                    else if(islocked == "No")

                        Trace.WriteLine("Unlocked successfully");

                    my_sql.Close();
                }

                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public static string Read_DMM_Status(string conStr)
        {
            int slotnum = 0;
            string lockstatus = "";

            var myConStr = conStr;
            using (SqlConnection my_sql = new SqlConnection(myConStr))
            {
                my_sql.Open();

                try
                {
                    SqlCommand command = new SqlCommand("SELECT * from [34970A_LockStatus] where [Tester] = '1' and [IsLocked] like 'no'", my_sql);
                    SqlDataReader Reader = command.ExecuteReader();

                    while (Reader.Read())
                    {
                        slotnum = Reader.GetInt32(0);
                        lockstatus = Reader.GetString(1);
                    }
                }

                catch (Exception es)
                {
                    Trace.WriteLine(es.Message);
                    throw;
                }

                return lockstatus;
            }
        }
    }
}
