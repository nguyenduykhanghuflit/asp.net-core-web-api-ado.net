using ApiDemo.Database;
using ApiDemo.Helpers;
using ApiDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Xml;


namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly SqlHelper _db;
        private readonly DatabaseManager _database;

        public StatisticalController(SqlHelper db, DatabaseManager database)
        {
            _db = db;
            _database = database;
        }

        //Tổng số deal của tất cả nhân viên từ ngày đến ngày

        [HttpPost("/api/statistical/Deal/Employee")]
        [Authorize]
        public ActionResult StatisticalDealEmployee(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                List<SqlParameter> sqlParameters = new()
                 {
                        new SqlParameter() {ParameterName= "@startDate", Value =startDate==null?today:startDate},
                        new SqlParameter() {ParameterName= "@endDate", Value =endDate==null?today:endDate}
                 };
                var type = System.Data.CommandType.StoredProcedure;
                List<StatisticalVM> statisticalVMs = new();

                _database.OpenConnection();

                using (var command = _database.CreateCommand("khangStatisticalDeal", type, sqlParameters.ToArray()))
                {
                    using SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        StatisticalVM statisticalVM = new();

                        string xmlDeals = reader["deals"] != DBNull.Value ? (string)reader["deals"] : "";

                        string jsonString = "";
                        if (!string.IsNullOrEmpty(xmlDeals))
                        {

                            Utils utils = new();
                            jsonString = utils.XmlToJsonString(xmlDeals);


                        }

                        statisticalVM.OBJ_AUTOID = (int)reader["OBJ_AUTOID"];
                        statisticalVM.OBJ_NAME = reader["OBJ_NAME"] != DBNull.Value ? (string)reader["OBJ_NAME"] : null;
                        statisticalVM.DealTotal = (int)reader["DealTotal"];
                        statisticalVM.Deals = jsonString;
                        statisticalVM.DealDate = reader["DealDate"] != DBNull.Value ? (DateTime)reader["DealDate"] : null;
                        statisticalVM.OBJ_PHONE = reader["OBJ_PHONE"] != DBNull.Value ? (string)reader["OBJ_PHONE"] : null;
                        statisticalVM.OBJ_ADDRESS = reader["OBJ_ADDRESS"] != DBNull.Value ? (string)reader["OBJ_ADDRESS"] : null;
                        statisticalVM.OBJ_EMAIL = reader["OBJ_EMAIL"] != DBNull.Value ? (string)reader["OBJ_EMAIL"] : null;
                        statisticalVM.OBJ_ISACTIVE = (bool)reader["OBJ_ISACTIVE"];

                        statisticalVMs.Add(statisticalVM);
                    }

                }

                _database.CloseConnection();


                ResponseHelper responseHepler = new(0, "Success", statisticalVMs);
                return Ok(responseHepler.HandleResponse());


            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Erorr: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }


        }


        //Thống kê trạng thái theo tháng
        [HttpPost("/api/statistical/Deal/Status")]
        // [Authorize]
        public ActionResult StatisticalDealStatus([FromBody] StatusDTO status)
        {

            try
            {

                int currentYear = DateTime.Now.Year;
                int year = status.Year;
                int statusId = status.StatusId;

                if (year.ToString().Length < 4 || year < 2000 || year > currentYear || statusId < 0)
                {
                    ResponseHelper responseHepler = new(-1, "Data invalid", null);
                    return Ok(responseHepler.HandleResponse());
                }



                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter() { ParameterName = "@stt", Value = status.StatusId },
                    new SqlParameter() { ParameterName = "@year", Value =status.Year }
                };


                var response = _db.HandleReadData("khangStatisticalStatusDeal", type, sqlParameters);

                if (!response.ContainsKey("Error"))
                {
                    ResponseHelper responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    ResponseHelper responseHepler = new(-1, response["Error"], null);
                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }


        //thống kê doanh thu của deal đã hoàn thành
        [HttpPost("/api/statistical/Deal/Revenues/Success")]
        //[Authorize]
        public ActionResult StatisticalDealSuccess(DateTime? startDate, DateTime? endDate)
        {

            try
            {

                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter() { ParameterName = "@startDate", Value = startDate },
                    new SqlParameter() { ParameterName = "@endDate", Value =endDate }
                };


                var response = _db.HandleReadData("khangStatisticalDealSuccess", type, sqlParameters);

                if (!response.ContainsKey("Error"))
                {
                    ResponseHelper responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    ResponseHelper responseHepler = new(-1, response["Error"], null);
                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }

        //thống kê top 10 sảnh của deal được chọn nhiều nhất theo tháng
        [HttpPost("/api/statistical/Deal/Hall/top10")]
        // [Authorize]
        public ActionResult StatisticalTopHall(DateTime startDate, DateTime endDate)
        {

            try
            {
                if (startDate.Year != endDate.Year)
                {
                    ResponseHelper responseHepler = new(-1, "Data invalid", null);
                    return Ok(responseHepler.HandleResponse());
                }
                int year = endDate.Year;
                int start = startDate.Month;
                int end = endDate.Month;

                List<dynamic> data = new();
                for (int month = start; month <= end; month++)
                {
                    Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
                    var type = CommandType.StoredProcedure;
                    SqlParameter[] sqlParameters =
                    {
                    new SqlParameter() { ParameterName = "@year", Value = year },
                    new SqlParameter() { ParameterName = "@month", Value =month }
                    };

                    var response = _db.HandleReadData("khangStatisticalTop10Hall", type, sqlParameters);

                    if (!response.ContainsKey("Error"))
                    {
                        item.Add("Month", month);
                        item.Add("top10", response["Data"]);
                        data.Add(item);
                    }
                    else
                    {
                        ResponseHelper responseHepler = new(-220, response["Error"], null);
                        return Ok(responseHepler.HandleResponse());
                    }

                }


                if (data.Count > 0)
                {
                    ResponseHelper responseHepler = new(0, "Success", data);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    ResponseHelper responseHepler = new(-1, "Server error at line 234", null);
                    return Ok(responseHepler.HandleResponse());
                }



            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }

        [HttpPost("/api/statistical/Deal/Hall/top10/v2")]
        // [Authorize]
        public ActionResult StatisticalTopHall2(DateTime startDate, DateTime endDate)
        {

            try
            {
                if (startDate.Year != endDate.Year)
                {
                    return Ok(new ResponseHelper(-1, "Data invalid", null).HandleResponse());
                }

                _database.OpenConnection();
                CommandType type = CommandType.StoredProcedure;
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(){ParameterName="@startDate",Value=startDate},
                    new SqlParameter(){ParameterName="@endDate",Value=endDate},
                };

                List<dynamic> data = new List<dynamic>();
                using (var command = _database.CreateCommand("khangStatisticalTop10Hall2", type, parameters))
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Dictionary<string, dynamic> item = new();
                            Utils utils = new();
                            string xml = (string)reader["top10"];

                            string jsonString = utils.XmlToJsonString(xml);
                            TopHallVM? topHall = JsonConvert.DeserializeObject<TopHallVM>(jsonString);

                            item.Add("Month", reader["month"]);
                            item.Add("Top10", topHall);
                            data.Add(item);
                        }

                    }
                }

                _database.CloseConnection();


                ResponseHelper responseHepler = new(0, "Success", data);
                return Ok(responseHepler.HandleResponse());
            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }

        [HttpPut("/api/deal/update")]
        // [Authorize]
        public ActionResult UpdateDeal(DealDTO dealDTO)
        {
            try
            {


                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
                {
                    new SqlParameter() { ParameterName = "@Id", Value = dealDTO.Id },
                    new SqlParameter() { ParameterName = "@Title", Value =dealDTO.Title },
                    new SqlParameter() { ParameterName = "@SaleMenId", Value =dealDTO.SaleMenId },
                    new SqlParameter() { ParameterName = "@Status", Value =dealDTO.Status },
                    new SqlParameter() { ParameterName = "@CustName", Value =dealDTO.CustName },
                    new SqlParameter() { ParameterName = "@Phone", Value =dealDTO.Phone },
                    new SqlParameter() { ParameterName = "@Email", Value =dealDTO.Email },
                    new SqlParameter() { ParameterName = "@Notes", Value =dealDTO.Notes },
                    };

                var response = _db.HandleReadData("khangUpdateDeal", type, sqlParameters);

                if (!response.ContainsKey("Error"))
                {
                    ResponseHelper responseHepler = new(0, "Update success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    ResponseHelper responseHepler = new(-1, response["Error"], null);
                    return Ok(responseHepler.HandleResponse());
                }




            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }

    }





}
