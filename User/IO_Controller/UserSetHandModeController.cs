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
    public class UserSetHandModeController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserSetHandMode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserSetHandMode/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserSetHandMode
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserSetHandMode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserSetHandMode/5
        public void Delete(int id)
        {
        }
        public IHttpActionResult Get(string UserMail)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"
                                select EmvStateNOW from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )
";

            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;


            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            int s = int.Parse(dt1.Rows[0]["EmvStateNOW"].ToString());

            sc1.Dispose();

            SqlCommand sc12 = new SqlCommand();

            if (s == 0)
            {
                sc12.CommandText = @"
                                UPDATE ModulState SET EmvStateNOW = 1 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )

                                UPDATE ModulState SET AutoMode = 0 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )

                                UPDATE ModulState SET SmartMode = 0 where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

                sc12.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                string strResult2 = DB.ConnectDB.ExecuteSQL(strConn, sc12);
            }
            else
            {
                sc12.CommandText = @"
                                UPDATE ModulState SET EmvStateNOW = 0 where 
                                ModulName =(select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

                sc12.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                string strResult2 = DB.ConnectDB.ExecuteSQL(strConn, sc12);
            }
            sc12.Dispose();

            return Ok("Success");
        }
    }
}
