using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace IOTFarmServer.Controllers
{
    public class UserForgetPwController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserForgetPw
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserForgetPw/5
        public string Get(String UserMail)
        {
            Random rnd = new Random();  //產生亂數初始值
            String RR = rnd.Next(100000, 999999).ToString();   //亂數產生

            SqlCommand sc1 = new SqlCommand();
            sc1.CommandText = "OPEN SYMMETRIC KEY AES256key DECRYPTION BY CERTIFICATE [Cert] update UserData set UserPw = EncryptByKey(Key_GUID('AES256key'),'" + RR + "')  where UserMail = @UserMail";
            sc1.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail; ;

            DataTable dt1 = DB.ConnectDB.getDataTable(strConn, sc1);

            sc1.Dispose();
            //設定smtp主機
            string smtpAddress = "smtp.gmail.com";

            //設定Port
            int portNumber = 587;

            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = "fcu.lab523@gmail.com";

            string password = "A11223344";

            //收信方email
            string emailTo = UserMail;
            //主旨
            string subject = "農園監測系統(此為自動發信，請勿回復)";
            //內容
            string body = "您的密碼暫時更改為" + RR + "，請登入後重新設定您的密碼";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            return "Success";

            //return dt1.Rows.Count.ToString();
        }

        // POST: api/UserForgetPw
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserForgetPw/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserForgetPw/5
        public void Delete(int id)
        {
        }
    }
}
