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
    public class UserUpdateCircleTimeController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserUpdateCircleTime
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserUpdateCircleTime/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserUpdateCircleTime
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserUpdateCircleTime/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserUpdateCircleTime/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail, string Name, string StartTime, string EndTime, string TemHumCode, string UVCode, string WaterFlowCode, string EmvCode, string SoilMoistureCode)
        {
            SqlCommand scData = new SqlCommand();

            scData.CommandText = @"
                                UPDATE  ModulState SET ModulName = @Name,StartTime =@StartTime,EndTime=@EndTime,TemHumCode = @TemHumCode,UVCode = @UVCode,WaterFlowCode = @WaterFlowCode,EmvCode = @EmvCode,SoilMoistureCode = @SoilMoistureCode Where  
                                UserMail = @UserMail AND 
                                ID=(select top 1 (ID) from ModulState  where UserMail = @UserMail order by ID desc)
                                ";

            scData.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;
            scData.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
            scData.Parameters.Add("@StartTime", SqlDbType.NVarChar).Value = StartTime;
            scData.Parameters.Add("@EndTime", SqlDbType.NVarChar).Value = EndTime;
            scData.Parameters.Add("@TemHumCode", SqlDbType.NVarChar).Value = TemHumCode;
            scData.Parameters.Add("@UVCode", SqlDbType.NVarChar).Value = UVCode;
            scData.Parameters.Add("@WaterFlowCode", SqlDbType.NVarChar).Value = WaterFlowCode;
            scData.Parameters.Add("@EmvCode", SqlDbType.NVarChar).Value = EmvCode;
            scData.Parameters.Add("@SoilMoistureCode", SqlDbType.NVarChar).Value = SoilMoistureCode;

            DataTable dtData = DB.ConnectDB.getDataTable(strConn, scData);

            scData.Dispose();

            return Ok("Success");
        }

    }
}
