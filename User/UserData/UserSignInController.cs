using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace IOTFarmServer.Controllers
{
    public class UserSignInController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserSignIn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserSignIn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserSignIn
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/UserSignIn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserSignIn/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult Post([FromBody]JObject value)
        {

            Input input = JsonConvert.DeserializeObject<Input>(value.ToString());

            string userMail = input.UserMail;
            string userPw = input.UserPw;

            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = @"
                                OPEN SYMMETRIC KEY AES256key DECRYPTION BY CERTIFICATE [Cert] 
                                select * from UserData 
                                WHERE UserMail = @UserMail and
                                CONVERT(varchar,DecryptByKey(UserPw)) = @UserPw ";

            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = userMail;

            sc1.Parameters.Add("@UserPw", SqlDbType.NVarChar).Value = userPw;

            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            sc1.Dispose();

            List<Output> dataResults = new List<Output>();

            Output dataResults2 = new Output();
            dataResults2.feedback = int.Parse(dt1.Rows.Count.ToString());
            
            dataResults.Add(dataResults2);

            return Content(HttpStatusCode.OK, dataResults, Configuration.Formatters.JsonFormatter);
            //return Ok(dt1.Rows.Count.ToString());


        }
        public class Input
        {
            public string UserMail { get; set; }
            public string UserPw { get; set; }
        }
        public class Output
        {
            public int feedback { get; set; }
        }
    }
}
