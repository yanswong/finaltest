using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSLDtest
{
    class SqlHelper
    {
        public static CalLeakData GetCalLeakData(SqlConnection conn, string sn, bool useTrackingNumber)
        {
            try
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                // CalLeakData instance
                CalLeakData cld = new CalLeakData();

                if (!useTrackingNumber)
                {
                    if (sn.ToUpper().Contains("MY"))    // if want to search by using serial number, get the tracking number first
                    {
                        string cmdText1 = string.Format("Select TOP (1) asn.SerialNumberDummy " +
                                                    "From [VpdOF].[dbo].CalLeakAutoSN as asn " +
                                                    "Where asn.SerialNumber = '{0}' " +
                                                    "Order By asn.LogDate Desc", sn);
                        SqlCommand cmd1 = new SqlCommand(cmdText1, conn);
                        SqlDataReader rdr1 = cmd1.ExecuteReader();
                        if (rdr1.HasRows)
                        {
                            rdr1.Read();
                            sn = rdr1[0].ToString();
                            rdr1.Close();
                        }
                        else
                        {
                            rdr1.Close();
                            cld = null;
                            return cld;
                        }
                    }
                }

                string cmdText = string.Format("SELECT TOP(1) cld.Id,cld.PartNumber,cld.SerialNumber AS TrackingNumber,asn.SerialNumber AS SerialNumber,cld.TestPort,cld.LeakRate," +
                                             "cld.UUTTemp,cld.BoardTemp,cld.Factor,cld.TestDate,cld.TestedBy,cld.NISTSerialNumber,cld.NISTDescription,cld.NISTPartNumber," +
                                             "cld.NISTReportNumber,cld.NISTCalDate,cld.NISTCalDueDate,cld.StationSerialNumber,cld.StationDescription,cld.StationModelNumber," +
                                             "cld.StationReportNumber,cld.StationCalDate,cld.StationCalDueDate " +
                                             "FROM VpdOF.dbo.CalLeakData AS cld " +
                                             "INNER JOIN VpdOF.dbo.CalLeakAutoSN AS asn " +
                                             "ON asn.SerialNumberDummy = cld.SerialNumber " +
                                             "WHERE cld.IsPass = 1 AND cld.SerialNumber = '{0}' " +
                                             "ORDER BY TestDate DESC, SerialNumber DESC", sn);




                SqlCommand cmd = new SqlCommand(cmdText, conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        cld.Id = rdr[0] as int? ?? default(int);
                        cld.PartNumber = rdr[1].ToString();
                        cld.TrackingNumber = rdr[2].ToString();
                        cld.SerialNumber = rdr[3].ToString();
                        cld.TestPortId = rdr[4] as int? ?? default(int);
                        cld.LeakRate = rdr[5] as double? ?? default(double);
                        cld.UUTTemp = rdr[6] as double? ?? default(double);
                        cld.BoardTemp = rdr[7] as double? ?? default(double);
                        cld.Factor = rdr[8] as double? ?? default(double);
                        cld.TestDate = rdr[9] as DateTime? ?? default(DateTime);
                        cld.TestedBy = rdr[10].ToString();
                        cld.NISTSerialNumber = rdr[11].ToString();
                        cld.NISTDescription = rdr[12].ToString();
                        cld.NISTPartNumber = rdr[13].ToString();
                        cld.NISTReportNumber = rdr[14].ToString();
                        cld.NISTCalDate = rdr[15] as DateTime? ?? default(DateTime);
                        cld.NISTCalDueDate = rdr[16] as DateTime? ?? default(DateTime);
                        cld.StationSerialNumber = rdr[17].ToString();
                        cld.StationDescription = rdr[18].ToString();
                        cld.StationModelNumber = rdr[19].ToString();
                        cld.StationReportNumber = rdr[20].ToString();
                        cld.StationCalDate = rdr[21] as DateTime? ?? default(DateTime);
                        cld.StationCalDueDate = rdr[22] as DateTime? ?? default(DateTime);
                    }
                    else
                    {
                        cld = null;
                    }
                }

                return cld;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static ExtCalLeakData GetExtCalLeakData(SqlConnection conn, string partNumber, string serialNumber)
        {
            try
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                // ext cal leak data
                var cld = new ExtCalLeakData();

                string cmdText = string.Format("SELECT TOP(1) * FROM VpdOF.dbo.LDFT_ExtCalLeak AS ecld " +
                                               "WHERE ecld.PartNumber = '{0}' AND ecld.SerialNumber = '{1}'", partNumber, serialNumber);

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        cld.Id = rdr[0] as int? ?? default(int);
                        cld.PartNumber = rdr[1].ToString();
                        cld.SerialNumber = rdr[2].ToString();
                        cld.LeakRate = rdr[3] as double? ?? default(double);
                        cld.Temperature = rdr[4] as double? ?? default(double);
                        cld.CalDate = rdr[5] as DateTime? ?? default(DateTime);
                        cld.CalDueDate = rdr[6] as DateTime? ?? default(DateTime);
                    }
                    else
                    {
                        cld = null;
                    }
                }

                return cld;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
