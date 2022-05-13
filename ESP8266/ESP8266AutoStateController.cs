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
    public class ESP8266AutoStateController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/ESP8266AutoState
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESP8266AutoState/5
        public int Get(int AutoState)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"select AutoMode from ModulState";

            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            sc1.Dispose();

            int b = int.Parse(dt1.Rows[0]["AutoMode"].ToString());

            return b;
        }

        // POST: api/ESP8266AutoState
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ESP8266AutoState/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESP8266AutoState/5
        public void Delete(int id)
        {
        }
    }
}
