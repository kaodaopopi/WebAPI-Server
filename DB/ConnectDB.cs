using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IOTFarmServer.DB
{
    public class ConnectDB
    {
        public static DataTable getDataTable(string strConn0, SqlCommand sc0)
        {
            DataTable dtTemp = new DataTable();
            try
            {

                SqlCommand scTemp = sc0;

                SqlConnection objConnTemp = new SqlConnection(strConn0);

                scTemp.Connection = objConnTemp;

                scTemp.CommandType = CommandType.Text;

                SqlDataAdapter sdTemp = new SqlDataAdapter(scTemp);

                if (objConnTemp.State == ConnectionState.Closed) objConnTemp.Open();

                sdTemp.Fill(dtTemp);

                if (objConnTemp.State == ConnectionState.Open) objConnTemp.Close();

                sdTemp.Dispose();

                scTemp.Dispose();

                objConnTemp.Dispose();

            }
            catch (System.Exception e)
            {

                dtTemp.Columns.Add("ERROR");

                DataRow row = dtTemp.NewRow();

                row["ERROR"] = "Error：" + e.Message.ToString();

                dtTemp.Rows.Add(row);
            }
            return dtTemp;
        }
        public static string ExecuteSQL(string strConn0, SqlCommand sc0)
        {
            string strResult = "";
            try
            {
                SqlCommand scTemp = sc0;

                SqlConnection objConnTemp = new SqlConnection(strConn0);

                scTemp.Connection = objConnTemp;

                scTemp.CommandType = CommandType.Text;

                if (objConnTemp.State == ConnectionState.Closed) objConnTemp.Open();

                scTemp.ExecuteNonQuery();

                if (objConnTemp.State == ConnectionState.Open) objConnTemp.Close();

                objConnTemp.Dispose();

                scTemp.Dispose();

                strResult = "Success";
            }
            catch (System.Exception e)
            {
                strResult = "Error:" + e.Message.ToString();
            }
            return strResult;
        }
    }
}