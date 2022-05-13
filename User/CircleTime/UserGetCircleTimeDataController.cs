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

namespace IOTFarmServer.Controllers.User.CircleTime
{
    public class UserGetCircleTimeDataController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetCircleTimeData
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetCircleTimeData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetCircleTimeData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetCircleTimeData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetCircleTimeData/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail)
        {
            SqlCommand scData = new SqlCommand();

            scData.CommandText = @"
                                    select ModulName,StartTime,EndTime,TemHumCode,UVCode,WaterFlowCode,EmvCode,SoilMoistureCode
                                    from ModulState 
                                    where ModulName = (select  Top 1 (ModulName) from ModulState where UserMail= @UserMail order by ID desc)";

            scData.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtData = DB.ConnectDB.getDataTable(strConn, scData);

            scData.Dispose();

            DataResult dataResults = new DataResult();
            dataResults.ModulName = dtData.Rows[0]["ModulName"].ToString();
            dataResults.StartTime = dtData.Rows[0]["StartTime"].ToString();
            dataResults.EndTime = dtData.Rows[0]["EndTime"].ToString();
            dataResults.TemHumCode = dtData.Rows[0]["TemHumCode"].ToString();
            dataResults.UVCode = dtData.Rows[0]["UVCode"].ToString();
            dataResults.WaterFlowCode = dtData.Rows[0]["WaterFlowCode"].ToString();
            dataResults.EmvCode = dtData.Rows[0]["EmvCode"].ToString();
            dataResults.SoilMoistureCode = dtData.Rows[0]["SoilMoistureCode"].ToString();

            return Content(HttpStatusCode.OK, dataResults, Configuration.Formatters.JsonFormatter);
        }
        public class DataResult
        {
            public string ModulName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string TemHumCode { get; set; }
            public string UVCode { get; set; }
            public string WaterFlowCode { get; set; }
            public string EmvCode { get; set; }
            public string SoilMoistureCode { get; set; }
        }

    }
}
