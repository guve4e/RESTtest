using RESTtest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTtest.Databse
{
    class UpdateRequest : Database
    {
    
        /// <summary>
        /// Create Request 
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool CreateRequest(RestRequest u)
        {
            bool success = false;
            DateTime date = DateTime.Now;

            string sql = string.Format(@" 
                    INSERT INTO Request (R_URL,R_METHOD, R_CONTROLLER, R_PARAMETERS, R_BODY, R_DATE)
                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}');                        
                "
                , u.url, u.method,u.controller,"",u.json_data,date);

            string headers; 
               
            try
            {
                // update Request first
                this.db.Open(); // open database
                SqlCommand cmd = MakeSQLCommand(sql);
                // execute
                success = ExecuteActionQuery(sql);
                this.db.Close();

                // get the Id of the added Request
                // update Header
                RestRequest r = GetLastUpdatedRowIdRequest();
                int r_id = r.id;
                this.db.Open();
                foreach (KeyValuePair<string,string> d in u.header)
                {
                    headers  = string.Format(@" 
                    INSERT INTO Header(R_ID,H_KEY,H_VALUE)
                    VALUES('{0}','{1}','{2}');                        
                    "
                    ,r_id, d.Key,d.Value);

                    SqlCommand cmd1 = MakeSQLCommand(headers);

                    success = ExecuteActionQuery(headers);
                }

              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** Exception In CreateUser ******************");
                Debug.WriteLine(ex);
            }
            finally
            {
                db.Close(); // close Database
    
            }
            return success;
        }//end



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RestRequest GetLastUpdatedRowIdRequest()
        {
            RestRequest r = null;
            Dictionary<string, string> headers = new Dictionary<string, string>();

            int u_id = -1;
            string r_url = "";
            string r_method = "";
            string r_controller = "";
            string r_parameters = "";
            string r_body = "";

            // query
            string sql = string.Format(@"

                    -- Extract Request's ID from Request Table
                    DECLARE @ID INTEGER;
                    SELECT @ID = R_ID FROM Request
                    WHERE R_DATE=(SELECT max(R_DATE) FROM Request);
         
                    -- Get the Row            
                    SELECT * FROM Request
                    WHERE R_DATE=(SELECT max(R_DATE) FROM Request);
                
                    -- Get Headers
                    SELECT * FROM Header
                    WHERE R_ID = @ID;

            ");

            try
            {
                db.Open(); // open database
                SqlCommand cmd = MakeSQLCommand(sql);
                // execute
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds); // fill adapter

                DateTime r_date;
                string date = "";

                // Collect Request attributes
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    u_id = Convert.ToInt32(row["R_ID"]);
                    r_url = Convert.ToString(row["R_URL"]);
                    r_method = Convert.ToString(row["R_METHOD"]);
                    r_date = Convert.ToDateTime(row["R_DATE"]);
                    date = Convert.ToString(r_date);
                    r_body = Convert.ToString(row["R_BODY"]);
                    r_controller = Convert.ToString(row["R_CONTROLLER"]);
                    r_parameters = Convert.ToString(row["R_PARAMETERS"]);
                   
                }

                // Collect Headers attributes
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    string key = Convert.ToString(row["H_KEY"]);
                    string value = Convert.ToString(row["H_VALUE"]);
                    headers.Add(key, value);
                }

                // encapsulate request
                r = new RestRequest(u_id, r_url, r_method, date, r_body, r_controller, r_parameters);
                r.header = headers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** Exception In GetLastUpdatedRow ******************");
                Debug.WriteLine(ex);
            }
            finally
            {
                db.Close();
            }

            return r;
        }// end

    }
}
