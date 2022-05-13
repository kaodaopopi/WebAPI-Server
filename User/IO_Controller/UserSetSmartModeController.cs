using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;


namespace IOTFarmServer.Controllers
{
    public class UserSetSmartModeController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserSetSmartMode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetDataState/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserSetSmartMode
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserSetSmartMode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserSetSmartMode/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult Get(string UserMail)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"select SmartMode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;


            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            int s = int.Parse(dt1.Rows[0]["SmartMode"].ToString());

            sc1.Dispose();

            SqlCommand sc12 = new SqlCommand();

            if (s == 0)
            {
                sc12.CommandText = @"UPDATE ModulState SET SmartMode = 1 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )

                                UPDATE ModulState SET AutoMode = 0 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

                sc12.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                string strResult2 = DB.ConnectDB.ExecuteSQL(strConn, sc12);
            }
            else
            {
                sc12.CommandText = @"UPDATE ModulState SET SmartMode = 0 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

                sc12.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                string strResult2 = DB.ConnectDB.ExecuteSQL(strConn, sc12);
            }
            sc12.Dispose();

            return Ok("Success");
        }
    }
}
