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
        /// <param name="request">Rest Request</param>
        /// <returns></returns>
        public bool CreateRequest(RestRequest request)
        {
            bool success = false;
            DateTime date = DateTime.Now;

            string sql = string.Format(@" 
                    INSERT INTO Request (R_URL,R_METHOD, R_CONTROLLER, R_PARAMETERS, R_BODY, R_DATE, R_CTYPE)
                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}');   

                    SELECT CAST(scope_identity() AS int)
                     
                "
                , request.url, request.method,request.controller,"",request.json_data,date,request.type);

            string headers; 
               
            try
            {
                
                this.db.Open(); 
                SqlCommand cmd = MakeSQLCommand(sql);
              
                // make ExecuteScalar to return the id
                cmd.Parameters.AddWithValue("@Value", "bar");
                // get the id
                int r_id = (int)cmd.ExecuteScalar();

                // execute
                success = ExecuteActionQuery(sql);
                this.db.Close();
                   
                // go around add headers
                foreach (KeyValuePair<string,string> d in request.header)
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
                Debug.WriteLine("************** Exception In CreateRequest ******************");
                Debug.WriteLine(ex);
            }
            finally
            {
                db.Close();
    
            }
            return success;
        }//end



        /// <summary>
        /// GetRequests
        /// </summary>
        /// <returns>Number of Rows you need</returns>
        public List<RestRequest> GetRequests(int numOfRows = 30)
        {
            // list to collect the requests
            RestRequest r = null;
            List<RestRequest> restRequests = null;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            // some locals

            int prv_id = -1;
            int r_id = -1;
            string r_url = "";
            string r_method = "";
            string r_controller = "";
            string r_parameters = "";
            string r_body = "";
            string r_type = "";

            // query
            string sql = string.Format(@"
                    SELECT TOP {0} request.R_ID, R_URL, R_METHOD, R_DATE, R_CONTROLLER, R_PARAMETERS, R_BODY,R_CTYPE, H_KEY, H_VALUE
                    FROM request LEFT JOIN HEADER ON
                    request.R_ID = header.R_ID
					ORDER BY R_DATE;
            ", numOfRows);

            try
            {
                db.Open(); // open database
                SqlCommand cmd = MakeSQLCommand(sql);
                // execute
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds); // fill adapter

                // time object
                DateTime r_date;
                string date = "";

                // make new list
                restRequests = new List<RestRequest>();

                // collect Request attributes
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // get header
                    string key = Convert.ToString(row["H_KEY"]);
                    string value = Convert.ToString(row["H_VALUE"]);
                    // get the id
                    r_id = Convert.ToInt32(row["R_ID"]);

                    // if id is not equal to the previous id
                    // make new object and add it to the list
                    if (r_id != prv_id)
                    {
                        r_url = Convert.ToString(row["R_URL"]);
                        r_method = Convert.ToString(row["R_METHOD"]);
                        r_date = Convert.ToDateTime(row["R_DATE"]);
                        date = Convert.ToString(r_date);
                        r_controller = Convert.ToString(row["R_CONTROLLER"]);
                        r_parameters = Convert.ToString(row["R_PARAMETERS"]);
                        r_body = Convert.ToString(row["R_BODY"]);
                        r_type = Convert.ToString(row["R_CTYPE"]);
                        // make an object, add headers and add it to the list
                        r = new RestRequest(r_id, r_url, r_method, date, r_body, r_controller, r_parameters,r_type);

                        // TODO check for null/empty values for key:value
                        r.header.Add(key, value);
                        restRequests.Add(r);
                    }
                    // if the id is the same as the
                    // previous one do not make new 
                    // request object but add the key:value pair as headers
                    // to the last one in the list
                    else
                    {
                        // get the last element
                        var item = restRequests[restRequests.Count - 1];
                        // add it to the last element's headers
                        // TODO check for null/empty values for key:value
                        item.header.Add(key, value);
                    }
                    // set the id to be the same as the previous
                    // so this id is next previous
                    prv_id = r_id;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** Exception In GetRequests ******************");
                Debug.WriteLine(ex);
            }
            finally
            {
                db.Close();
            }

            return restRequests;
        }


        /// <summary>
        /// Delete Request by ID
        /// </summary>
        /// <returns></returns>
        public bool DeleteRequest(int id)
        {
            bool success = false;

            string sql = string.Format(@"
                    -- update Request  
                    DELETE FROM Request
                    WHERE R_ID = {0};
                     
                    -- update Header
                    DELETE FROM Request
                    WHERE R_ID = {0};

                "
                , id);

          
            try
            {
                db.Open(); // open database
                SqlCommand cmd = MakeSQLCommand(sql);
                // execute
                success = ExecuteActionQuery(sql);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** Exception In UpdateUser ******************");
                Debug.WriteLine(ex);
            }
            finally
            {
                db.Close(); // close Database
            }
            return success;
        }// end

    }
}
