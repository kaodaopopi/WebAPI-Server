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
    public class ESP8266EmvInputController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/ESP8266EmvInput
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESP8266EmvInput/5
        public string Get(string CodeName, int EmvState)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"UPDATE ModulState SET EmvStateNOW = @EmvState 
                                INSERT INTO SensorEmvData(EmvCode,EmvState)VALUES (@CodeName,@EmvState)";

            sc1.Parameters.Add("@CodeName", SqlDbType.NVarChar).Value = CodeName;

            sc1.Parameters.Add("@EmvState", SqlDbType.NVarChar).Value = EmvState;

            string strResult = DB.ConnectDB.ExecuteSQL(strConn, sc1);

            sc1.Dispose();

            return strResult;
        }

        // POST: api/ESP8266EmvInput
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ESP8266EmvInput/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESP8266EmvInput/5
        public void Delete(int id)
        {
        }
    }
}
