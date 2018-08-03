using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSLDtest
{
    class SQL_XGS
    {
        public static void Update_XGS(string slotnum, string locked)
        {
            var src = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
            using (SqlConnection ys_sql = new SqlConnection(src))
            {
                try
                {
                    ys_sql.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE [_XGS600]"+"SET XGS600 ='"+locked+"'where[Tester]='1';",ys_sql);
                    cmd.ExecuteNonQuery();
                    ys_sql.Close();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public static string Read_XGS_status()
        {
            string lockstatus = ""; 
            var src = "Data Source=wpsqldb21;Initial Catalog=VpdOF;Persist Security Info=True;User ID=VpdTest;Password=Vpd@123";
            using (SqlConnection ys_sql = new SqlConnection(src))
            {
                try
                {
                    ys_sql.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * from [_XGS600] where [Tester] = '1' and [XGS600] like 'no'",ys_sql);
                    SqlDataReader Reader = cmd.ExecuteReader();
                    while (Reader.Read())
                    {
                       lockstatus = Reader.GetString(1);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    throw;
                }
                return lockstatus;
            }
        }
    }
}
