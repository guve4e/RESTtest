using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RESTtest.Databse
{
    /// <summary>
    /// Database class ADO.NET
    /// 
    /// Makes an SqlConnection and 
    /// connects to local database
    /// </summary>   
    class Database
    {
        public SqlConnection db { get; set; }
        public string version { get; set; }
        public string connectionInfo { get; set; }
        public string filename { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Database()
        {
            this.version = "MSSQLLocalDB";
            this.filename = "Database.mdf";

            this.connectionInfo = String.Format(@"Data Source = (LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", this.version, this.filename);

            try
            {
                db = new SqlConnection(connectionInfo);
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("Exception in Database Constructor: " + ex);
            }
        }

        /// <summary>
        /// Executes the given SQL string, which should be an "action" such as 
        /// create table, drop table, insert, update, or delete.  Returns 
        /// normally if successful, throws an exception if not.
        /// </summary>
        /// <param name="sql">Query</param>
        /// <exception cref="SqlException"></exception>
        public bool ExecuteActionQuery(string sql)
        {
            int obj = -1;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                //cmd.Transaction = t;
                obj = cmd.ExecuteNonQuery();
                Debug.WriteLine("AFFECTED ROWS -> " + obj);
            }
            catch (SqlException e)
            {
                Debug.WriteLine("************** Exception In ExecuteNonQuery ******************");
                Debug.WriteLine(e);
            }
            if (obj <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }// end 

        /// <summary>
        /// Makes an SQL Command
        /// 
        /// </summary>
        /// <param name="sql">Query</param>
        /// <returns>SQL Command</returns>
        public SqlCommand MakeSQLCommand(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            return cmd;
        }// end


    }// end class
}// end namespace
