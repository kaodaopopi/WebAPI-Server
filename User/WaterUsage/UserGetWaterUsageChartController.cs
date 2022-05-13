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
    public class UserGetWaterUsageChartController : ApiController
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;

        // GET: api/UserGetWaterUsageChart
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserGetWaterUsageChart/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserGetWaterUsageChart
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserGetWaterUsageChart/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserGetWaterUsageChart/5
        public void Delete(int id)
        {
        }
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetSensorData(string UserMail,int Num)
        {
            if (Num == 0)
            {
                SqlCommand scName = new SqlCommand();

                scName.CommandText = @"
                       
                                    DECLARE  
                                    @TotalNum INT, 
                                    @Num INT   ,   
                                    @All nvarchar(50)

                                    SET @TotalNum = 2 
                                    SET @Num =0        

                                    DECLARE  @Table_name AS TABLE
                                    (
                                    Name nvarchar(50)
                                    )
                                    WHILE @Num <= @TotalNum  
                                    BEGIN

                                    SET @All = 
                                    (
                                    select top 1 ModulName from (SELECT  * from ModulState where UserMail=@UserMail order by ModulName offset @Num row fetch next 1 rows only)  AS start order by ID desc
                                    )

                                    if @All is Null
	                                    SET @All = 'NULL'+CONVERT(varchar,@Num)

                                    INSERT INTO @Table_name(Name) VALUES (@All)

                                    SET @Num = @Num + 1
                                    END

                                    SELECT *
                                    FROM @Table_name

                                ";

                scName.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtName = DB.ConnectDB.getDataTable(strConn, scName);

                scName.Dispose();

                SqlCommand scSUM = new SqlCommand();

                scSUM.CommandText = @"
                       
                                DECLARE  
                                @TotalNum INT, 
                                @Num INT   ,   
                                @All FLOAT

                                SET @TotalNum = 2
                                SET @Num =0        

                                DECLARE  @Table_name AS TABLE
                                (
                                   Sum FLOAT
                                )
                                WHILE @Num <= @TotalNum  
                                BEGIN

                                SET @All = 
                                (

                                select cast(round(
                                (
								SELECT SUM (WaterFlowValues)/3600 FROM SensorWaterFlow where

                                Time BETWEEN 
                                (select top 1 (StartTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND 
                                (select top 1 (EndTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND

                                WaterFlowCode = (select WaterFlowCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc ))
								),3) as numeric(5,3)) 

                                )

                                if @All is Null
	                                SET @All = 0.0

                                INSERT INTO @Table_name(Sum) VALUES (@All)

	                                SET @Num = @Num + 1
                                END

                                SELECT *
                                FROM @Table_name


                                ";

                scSUM.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scSUM);

                scSUM.Dispose();

                List<Result> myResult1 = new List<Result>();
                for(int i =0;i<dtName.Rows.Count; i++)
                {
                    Result myResult2 = new Result();
                    myResult2.Name = dtName.Rows[i]["Name"].ToString();
                    myResult2.Sum = dtChart.Rows[i]["Sum"].ToString();
                    myResult1.Add(myResult2);
                }
                return Content(HttpStatusCode.OK, myResult1, Configuration.Formatters.JsonFormatter);
            }
            else if(Num == 1){
                SqlCommand scName = new SqlCommand();

                scName.CommandText = @"
                       
                                    DECLARE  
                                    @TotalNum INT, 
                                    @Num INT   ,   
                                    @All nvarchar(50)

                                    SET @TotalNum = 5 
                                    SET @Num =0        

                                    DECLARE  @Table_name AS TABLE
                                    (
                                    Name nvarchar(50)
                                    )
                                    WHILE @Num <= @TotalNum  
                                    BEGIN

                                    SET @All = 
                                    (
                                    select top 1 ModulName from (SELECT  * from ModulState where UserMail=@UserMail order by ModulName offset @Num row fetch next 1 rows only)  AS start order by ID desc
                                    )

                                    if @All is Null
	                                    SET @All = 'NULL'+CONVERT(varchar,@Num)

                                    INSERT INTO @Table_name(Name) VALUES (@All)

                                    SET @Num = @Num + 1
                                    END

                                    SELECT *
                                    FROM @Table_name

                                ";

                scName.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtName = DB.ConnectDB.getDataTable(strConn, scName);

                scName.Dispose();

                SqlCommand scSUM = new SqlCommand();

                scSUM.CommandText = @"
                       
                                  DECLARE  
                                @TotalNum INT, 
                                @Num INT   ,   
                                @All FLOAT

                                SET @TotalNum = 5
                                SET @Num =0        

                                DECLARE  @Table_name AS TABLE
                                (
                                   Sum FLOAT
                                )
                                WHILE @Num <= @TotalNum  
                                BEGIN

                                SET @All = 
                                (

                                select cast(round(
                                (
								SELECT SUM (WaterFlowValues)/3600 FROM SensorWaterFlow where

                                Time BETWEEN 
                                (select top 1 (StartTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND 
                                (select top 1 (EndTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND

                                WaterFlowCode = (select WaterFlowCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc ))
								),3) as numeric(5,3)) 

                                )

                                if @All is Null
	                                SET @All = 0.0

                                INSERT INTO @Table_name(Sum) VALUES (@All)

	                                SET @Num = @Num + 1
                                END

                                SELECT *
                                FROM @Table_name


                                ";

                scSUM.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scSUM);

                scSUM.Dispose();

                List<Result> myResult1 = new List<Result>();
                for (int i = 0; i < dtName.Rows.Count; i++)
                {
                    Result myResult2 = new Result();
                    myResult2.Name = dtName.Rows[i]["Name"].ToString();
                    myResult2.Sum = dtChart.Rows[i]["Sum"].ToString();
                    myResult1.Add(myResult2);
                }
                return Content(HttpStatusCode.OK, myResult1, Configuration.Formatters.JsonFormatter);
            }
            else if(Num == 2){
                SqlCommand scName = new SqlCommand();

                scName.CommandText = @"
                       
                                    DECLARE  
                                    @TotalNum INT, 
                                    @Num INT   ,   
                                    @All nvarchar(50)

                                    SET @TotalNum = 11 
                                    SET @Num =0        

                                    DECLARE  @Table_name AS TABLE
                                    (
                                    Name nvarchar(50)
                                    )
                                    WHILE @Num <= @TotalNum  
                                    BEGIN

                                    SET @All = 
                                    (
                                    select top 1 ModulName from (SELECT  * from ModulState where UserMail=@UserMail order by ModulName offset @Num row fetch next 1 rows only)  AS start order by ID desc
                                    )

                                    if @All is Null
	                                    SET @All = 'NULL'+CONVERT(varchar,@Num)

                                    INSERT INTO @Table_name(Name) VALUES (@All)

                                    SET @Num = @Num + 1
                                    END

                                    SELECT *
                                    FROM @Table_name

                                ";

                scName.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtName = DB.ConnectDB.getDataTable(strConn, scName);

                scName.Dispose();

                SqlCommand scSUM = new SqlCommand();

                scSUM.CommandText = @"
                       
                                  DECLARE  
                                @TotalNum INT, 
                                @Num INT   ,   
                                @All FLOAT

                                SET @TotalNum = 11 
                                SET @Num =0        

                                DECLARE  @Table_name AS TABLE
                                (
                                   Sum FLOAT
                                )
                                WHILE @Num <= @TotalNum  
                                BEGIN

                                SET @All = 
                                (

                                select cast(round(
                                (
								SELECT SUM (WaterFlowValues)/3600 FROM SensorWaterFlow where

                                Time BETWEEN 
                                (select top 1 (StartTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND 
                                (select top 1 (EndTime) from 
                                (SELECT  * from ModulState where UserMail=@UserMail order by StartTime offset @Num row fetch next 1 rows only)  AS start order by ID desc) AND

                                WaterFlowCode = (select WaterFlowCode from ModulState where 
                                ModulName = (select top 1 (ModulName) from ModulState where UserMail=@UserMail order by ID desc ))
								),3) as numeric(5,3)) 

                                )

                                if @All is Null
	                                SET @All = 0.0

                                INSERT INTO @Table_name(Sum) VALUES (@All)

	                                SET @Num = @Num + 1
                                END

                                SELECT *
                                FROM @Table_name


                                ";

                scSUM.Parameters.Add("@UserMail", SqlDbType.NVarChar).Value = UserMail;

                DataTable dtChart = DB.ConnectDB.getDataTable(strConn, scSUM);

                scSUM.Dispose();

                List<Result> myResult1 = new List<Result>();
                for (int i = 0; i < dtName.Rows.Count; i++)
                {
                    Result myResult2 = new Result();
                    myResult2.Name = dtName.Rows[i]["Name"].ToString();
                    myResult2.Sum = dtChart.Rows[i]["Sum"].ToString();
                    myResult1.Add(myResult2);
                }
                return Content(HttpStatusCode.OK, myResult1, Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Ok();
            }
        }
        public class Result
        {
            public string Name { get; set; }
            public string Sum { get; set; }
        }
    }
}
