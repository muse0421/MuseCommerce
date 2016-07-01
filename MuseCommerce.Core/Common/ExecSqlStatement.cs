using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Common
{
    public class ExecSqlStatement
    {
        public static void Run(string CommandText)
        {
            string SqlConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["JobSqlConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(SqlConnectionString);
            con.Open();
            SqlCommand myCommand = new SqlCommand();
            SqlTransaction myTrans;
            myTrans = con.BeginTransaction();
            myCommand.Connection = con;
            myCommand.CommandType = CommandType.Text;
            myCommand.Transaction = myTrans;

            try
            {
                myCommand.CommandText = CommandText;
                myCommand.ExecuteNonQuery();
                myTrans.Commit();                
            }
            catch (Exception e)
            {
                myTrans.Rollback();
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                    con = null;
                }
            }
        }
    }
}
