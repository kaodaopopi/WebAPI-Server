using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IOTFarmServer.Controllers
{
    public class ESP8266SoilMoistureController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/ESP8266SoilMoisture
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESP8266SoilMoisture/5
        public string Get(int SoilMoistureCode, int SoilMoistureData)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"INSERT INTO SensorSoilMoistureData(SoilMoistureCode,SoilMoistureData)VALUES(@SoilMoistureCode,@SoilMoistureData)";

            sc1.Parameters.Add("@SoilMoistureCode", SqlDbType.NVarChar).Value = SoilMoistureCode;

            sc1.Parameters.Add("@SoilMoistureData", SqlDbType.NVarChar).Value = SoilMoistureData;

            string strResult = DB.ConnectDB.ExecuteSQL(strConn, sc1);

            sc1.Dispose();

            return strResult;
        }

        // POST: api/ESP8266SoilMoisture
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ESP8266SoilMoisture/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESP8266SoilMoisture/5
        public void Delete(int id)
        {
        }
    }
}
