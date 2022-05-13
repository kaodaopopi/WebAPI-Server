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

namespace IOTFarmServer.Controllers.User.WaterUsage
{
    public class UserGetWaterUsageOneDayController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetWaterUsageOneDay
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetWaterUsageOneDay/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetWaterUsageOneDay
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetWaterUsageOneDay/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetWaterUsageOneDay/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail)
        {
            SqlCommand scSUM = new SqlCommand();

            scSUM.CommandText = @"
                                select cast(round(

                                (SELECT SUM 

                                ((WaterFlowValues*24)/50000)/
                                (SELECT DATEDIFF(day, (SELECT TOP 1 (StartTime) from ModulState where UserMail=@UserMail order by ID desc), (SELECT TOP 1 (EndTime) from ModulState  where UserMail=@UserMail order by ID desc)) AS DiffDate) 
 
                                FROM SensorWaterFlow where WaterFlowValues !=8 and

                                Time BETWEEN (SELECT TOP 1 (StartTime) from ModulState where UserMail=@UserMail order by ID desc) AND 
                                (SELECT TOP 1 (EndTime) from ModulState  where UserMail=@UserMail order by ID desc) AND

                                WaterFlowCode = (select WaterFlowCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc ))
                                )
	
	                                ,3) as numeric(5,3)) AS OneDay
                                    ";

            scSUM.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scSUM);

            scSUM.Dispose();

            return Content(HttpStatusCode.OK, dtChart, Configuration.Formatters.JsonFormatter);
        }
    }
}
