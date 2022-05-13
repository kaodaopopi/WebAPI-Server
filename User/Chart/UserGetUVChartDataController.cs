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
    public class UserGetUVChartDataController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetUVChartData
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetUVChartData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetUVChartData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetUVChartData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetUVChartData/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail, string StartTime, string EndTime)
        {
            SqlCommand scChart = new SqlCommand();

            scChart.CommandText = @"
                                   select convert(varchar(16),Time,120) Time,UVData from SensorUVData Where Time BETWEEN @StartTime AND @EndTime AND DATEPART(MINUTE, Time )=0 
                                    AND UVCode = (select TemHumCode from ModulState where 
                                    ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc)) order by ID desc";

            scChart.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            scChart.Parameters.Add("@StartTime", SqlDbType.NVarChar).Value = StartTime;

            scChart.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndTime;

            DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scChart);

            scChart.Dispose();

            return Content(HttpStatusCode.OK, dtChart, Configuration.Formatters.JsonFormatter);
        }
    }
}
