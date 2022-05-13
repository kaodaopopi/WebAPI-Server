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
    public class ESP8266AutoModeController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/ESP8266Auto
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESP8266Auto/5
        public int Get(int AutoState)
        {
            int AutoOpenOrNot;

            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"select  top 1* from SensorSoilMoistureData order by ID desc";

            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            sc1.Dispose();

            int NowSoilMoisture = int.Parse(dt1.Rows[0]["SoilMoistureData"].ToString());

            SqlCommand sc2 = new SqlCommand();

            sc2.CommandText = @"select SetSoilMoisture from ModulState";

            DataTable dt2 = DB.ConnectDB.getDataTable(strConn, sc2);

            sc2.Dispose();

            int SetSoilMoisture = int.Parse(dt2.Rows[0]["SetSoilMoisture"].ToString());

            if (NowSoilMoisture> SetSoilMoisture)
            {
                AutoOpenOrNot = 1;
            }
            else
            {
                AutoOpenOrNot = 0;
            }
            return AutoOpenOrNot;
        }

        // POST: api/ESP8266Auto
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ESP8266Auto/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESP8266Auto/5
        public void Delete(int id)
        {
        }
    }
}
