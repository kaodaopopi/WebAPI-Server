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
    public class ESP8266TemHumUvController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/ESP8266TemHumUv
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ESP8266TemHumUv/5
        public string Get(int TemHumCode, float Temperature, float Humidity, int UVCode,int UVData)
        {

            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"INSERT INTO SensorTemHumData(TemHumCode,Temperature,Humidity)VALUES(@TemHumCode,@Temperature,@Humidity)
                                INSERT INTO SensorUVData(UVCode,UVData)VALUES(@UVCode,@UVData)";

            sc1.Parameters.Add("@TemHumCode", SqlDbType.NVarChar).Value = TemHumCode;

            sc1.Parameters.Add("@Temperature", SqlDbType.NVarChar).Value = Temperature;

            sc1.Parameters.Add("@Humidity", SqlDbType.NVarChar).Value = Humidity;

            sc1.Parameters.Add("@UVCode", SqlDbType.NVarChar).Value = UVCode;

            sc1.Parameters.Add("@UVData", SqlDbType.NVarChar).Value = UVData;

            string strResult = DB.ConnectDB.ExecuteSQL(strConn, sc1);

            sc1.Dispose();

            return strResult;
        }

        // POST: api/ESP8266TemHumUv
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ESP8266TemHumUv/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ESP8266TemHumUv/5
        public void Delete(int id)
        {
        }
    }
}
