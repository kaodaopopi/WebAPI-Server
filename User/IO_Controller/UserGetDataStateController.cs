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
    public class UserGetDataStateController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetDataState
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetDataState/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetDataState
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetDataState/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetDataState/5
        public void Delete(int id)
        {
        }

        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail)
        {
       
            #region Emv
            SqlCommand scEmv = new SqlCommand();

            scEmv.CommandText = @"
                                select  top 1 * from SensorEmvData where 
                                EmvCode = (select EmvCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)) order by ID desc";

            scEmv.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtEmv = DB.ConnectDB.getDataTable(strConn, scEmv);

            scEmv.Dispose();
            #endregion
            #region SoilMoisture
            SqlCommand scSoilMoisture = new SqlCommand();

            scSoilMoisture.CommandText = @"
                                            select  top 1 * from SensorSoilMoistureData where 
                                            SoilMoistureCode = (select SoilMoistureCode from ModulState where 
                                            ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)) order by ID desc";


            scSoilMoisture.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtSoilMoisture = DB.ConnectDB.getDataTable(strConn, scSoilMoisture);

            scSoilMoisture.Dispose();

            #endregion
            #region TemHum
            SqlCommand scTemHum = new SqlCommand();

            scTemHum.CommandText = @"
                                    select  top 1 * from SensorTemHumData where 
                                    TemHumCode = (select TemHumCode from ModulState where 
                                    ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)) order by ID desc";

            scTemHum.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtTemHum = DB.ConnectDB.getDataTable(strConn, scTemHum);

            scTemHum.Dispose();
            #endregion
            #region UV
            SqlCommand scUV = new SqlCommand();

            scUV.CommandText = @"
                                select  top 1 * from SensorUVData where 
                                UVCode = (select UVCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)) order by ID desc";
            scUV.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtUV = DB.ConnectDB.getDataTable(strConn, scUV);

            scUV.Dispose();
            #endregion
            #region Auto
            SqlCommand scAuto = new SqlCommand();

            scAuto.CommandText = @"select AutoMode from ModulState where 
                                   ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)";
            scAuto.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtAuto = DB.ConnectDB.getDataTable(strConn, scAuto);

            scAuto.Dispose();
            #endregion
            #region Smart
            SqlCommand scSmart = new SqlCommand();

            scSmart.CommandText = @"select SmartMode from ModulState where 
                                   ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)";
            scSmart.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtSmart = DB.ConnectDB.getDataTable(strConn, scSmart);

            scSmart.Dispose();
            #endregion
            #region SetSoilMoisture
            SqlCommand scSetSoilMoisture = new SqlCommand();

            scSetSoilMoisture.CommandText = @"select SetSoilMoisture from ModulState where 
                                              ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)";
            scSetSoilMoisture.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtSetSoilMoisture = DB.ConnectDB.getDataTable(strConn, scSetSoilMoisture);

            scSetSoilMoisture.Dispose();
            #endregion

            #region Data Package 
            DataResult dataResults = new DataResult();

            SensorEmvData sensorEmvData = new SensorEmvData();
            sensorEmvData.EmvTime = dtEmv.Rows[0]["Time"].ToString();
            sensorEmvData.EmvState = int.Parse(dtEmv.Rows[0]["EmvState"].ToString());

            SensorSoilMoistureData sensorSoilMoistureData = new SensorSoilMoistureData();
            sensorSoilMoistureData.SoilMoistureTime = dtSoilMoisture.Rows[0]["Time"].ToString();
            sensorSoilMoistureData.SoilMoistureData = int.Parse(dtSoilMoisture.Rows[0]["SoilMoistureData"].ToString());

            SensorTemHumData sensorTemHumData = new SensorTemHumData();
            sensorTemHumData.TemHumTime = dtTemHum.Rows[0]["Time"].ToString();
            sensorTemHumData.Temperature = float.Parse(dtTemHum.Rows[0]["Temperature"].ToString());
            sensorTemHumData.Humidity = float.Parse(dtTemHum.Rows[0]["Humidity"].ToString());

            SensorUVData sensorUVData = new SensorUVData();
            sensorUVData.UVTime = dtUV.Rows[0]["Time"].ToString();
            sensorUVData.UVData = int.Parse(dtUV.Rows[0]["UVData"].ToString());

            ModulState modelState = new ModulState();
            modelState.SetSoilMoisture = int.Parse(dtSetSoilMoisture.Rows[0]["SetSoilMoisture"].ToString());
            modelState.AutoMode = int.Parse(dtAuto.Rows[0]["AutoMode"].ToString());
            modelState.SmartMode = int.Parse(dtSmart.Rows[0]["SmartMode"].ToString());

            dataResults.Emv = sensorEmvData;
            dataResults.SoilMoisture = sensorSoilMoistureData;
            dataResults.TemHum = sensorTemHumData;
            dataResults.UV = sensorUVData;
            dataResults.MS = modelState;
            #endregion

            return Content(HttpStatusCode.OK, dataResults, Configuration.Formatters.JsonFormatter);
        }
        public class DataResult
        {
            public SensorEmvData Emv { get; set; }
            public SensorSoilMoistureData SoilMoisture { get; set; }
            public SensorTemHumData TemHum { get; set; }
            public SensorUVData UV { get; set; }
            public ModulState MS { get; set; }
        }
        public class SensorEmvData
        {
            public string EmvTime { get; set; }
            public int EmvState { get; set; }
        }
        public class SensorSoilMoistureData
        {
            public string SoilMoistureTime { get; set; }
            public int SoilMoistureData { get; set; }
        }
        public class SensorTemHumData
        {
            public string TemHumTime { get; set; }
            public float Temperature { get; set; }
            public float Humidity { get; set; }
        }
        public class SensorUVData
        {
            public string UVTime { get; set; }
            public int UVData { get; set; }
        }
        public class ModulState
        {
            public int SetSoilMoisture { get; set; }
            public int AutoMode { get; set; }
            public int SmartMode { get; set; }
        }

    }
}
