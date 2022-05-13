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
    public class UserSetSoilMoistureController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserSetSoilMoistureInput
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserSetSoilMoistureInput/5
        public string Get(string UserMail,int SetSoilMoisture)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"UPDATE ModulState SET SetSoilMoisture = @SetSoilMoisture  WHERE 
                                ModulName=(select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc )";

            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            sc1.Parameters.Add("@SetSoilMoisture", SqlDbType.NVarChar).Value = SetSoilMoisture;

            string strResult = DB.ConnectDB.ExecuteSQL(strConn, sc1);

            sc1.Dispose();

            return strResult;
        }

        // POST: api/UserSetSoilMoistureInput
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserSetSoilMoistureInput/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserSetSoilMoistureInput/5
        public void Delete(int id)
        {
        }
    }
}
