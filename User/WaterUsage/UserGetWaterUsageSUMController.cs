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
    public class UserGetWaterSUMController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetWaterSUM
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetWaterSUM/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetWaterSUM
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetWaterSUM/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetWaterSUM/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail)
        {
            SqlCommand scSUM = new SqlCommand();

            scSUM.CommandText = @"
                                select cast(round(

                                (SELECT SUM ((WaterFlowValues*24)/30000) FROM SensorWaterFlow where WaterFlowValues !=8 and

                                Time BETWEEN (SELECT TOP 1 (StartTime) from ModulState where UserMail=@UserMail order by ID desc) AND 
                                (SELECT TOP 1 (EndTime) from ModulState  where UserMail=@UserMail order by ID desc) AND

                                WaterFlowCode = (select WaterFlowCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc ))
                                ) 

                                ,3) as numeric(18,3))AS Sum
                                    ";

            scSUM.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scSUM);

            scSUM.Dispose();

            return Content(HttpStatusCode.OK, dtChart, Configuration.Formatters.JsonFormatter);
        }
    }
}
