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
    public class UserChangePwController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserChangePw
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserChangePw/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserChangePw
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserChangePw/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserChangePw/5
        public void Delete(int id)
        {
        }
        public IHttpActionResult Get(string UserMail, string UserNewPw)
        {
            SqlCommand sc1 = new SqlCommand();

            sc1.CommandText = "OPEN SYMMETRIC KEY AES256key DECRYPTION BY CERTIFICATE [Cert] update UserData set UserPw = EncryptByKey(Key_GUID('AES256key'),@UserNewPw)  where UserMail = @UserMail";

            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

            sc1.Parameters.Add("@UserNewPw", SqlDbType.NVarChar).Value = UserNewPw;

            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            sc1.Dispose();
            
            return Ok("Success");
        }
    }
}
