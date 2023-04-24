using ApiDemo.Helpers;
using ApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data.SqlClient;
using System.Data;
using ApiDemo.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealController : ControllerBase
    {

        private readonly SqlHelper sqlHelper;
        private readonly DatabaseManager database;

        public DealController(SqlHelper _sqlHelper, DatabaseManager _database)
        {

            sqlHelper = _sqlHelper;
            database = _database;
        }


        //get thông tin deal, nếu không có dealId sẽ get tất cả

        [HttpPost("/api/deal/note")]
        [Authorize]
        public ActionResult GetDealNote()
        {
            try
            {
                ResponseHelper responseHepler = new(-1, "Server error", null);
                var type = CommandType.StoredProcedure;

                var response = sqlHelper.HandleReadData("khangGetDealInfo", type);
                if (!response.ContainsKey("Error"))
                {
                    responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    responseHepler = new(-1, response["Error"], null);

                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }

        }

        //update deal notes

        [HttpPatch("/api/deal/note")]
        [Authorize]
        public ActionResult UpdateDealNote(DealNoteDTO dealNote)
        {
            try
            {
                ResponseHelper responseHepler = new(-1, "Server error", null);
                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
                 {
                    new   SqlParameter() { ParameterName = "@dealId", Value = dealNote.dealId },
                    new   SqlParameter() { ParameterName = "@notes", Value = dealNote.notes },
                 };

                var response = sqlHelper.HandleReadData("khangUpdateDealNote", type, sqlParameters);
                if (!response.ContainsKey("Error"))
                {
                    responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    responseHepler = new(-1, response["Error"], null);

                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }



        [HttpGet("/api/deal/getstatus")]
        [Authorize]
        public ActionResult GetAllStatus()
        {
            try
            {
                ResponseHelper responseHepler = new(-1, "Server error", null);
                var type = CommandType.StoredProcedure;

                var response = sqlHelper.HandleReadData("khangStatisticalDealByStatus", type);
                if (!response.ContainsKey("Error"))
                {
                    responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    responseHepler = new(-1, response["Error"], null);

                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }

        }

        [HttpPatch("/api/deal/status")]
        [Authorize]
        public ActionResult UpdateDealStatus([FromBody] DealStatusDTO dealStatus)
        {
            try
            {
                ResponseHelper responseHepler = new(-1, "Server error", null);
                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
                 {
                    new   SqlParameter() { ParameterName = "@dealId", Value = dealStatus.dealId },
                    new   SqlParameter() { ParameterName = "@statusId", Value = dealStatus.statusId },
                 };

                var response = sqlHelper.HandleReadData("khangUpdateDealStatus", type, sqlParameters);
                if (!response.ContainsKey("Error"))
                {
                    responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    responseHepler = new(-1, response["Error"], null);

                    return Ok(responseHepler.HandleResponse());
                }

            }
            catch (Exception ex)
            {
                ResponseHelper responseHepler = new(-1, "Server Error: " + ex.Message, null);
                return Ok(responseHepler.HandleResponse());
            }
        }



        //chưa sử dụng
        [HttpPost("/api/getdeal/status")]
        [Authorize]
        public ActionResult GetDealByStatus([FromBody] Status status)
        {
            try
            {
                ResponseHelper responseHepler = new(-1, "Server error", null);
                var type = CommandType.StoredProcedure;
                SqlParameter[] sqlParameters =
               {
                    new SqlParameter(){ParameterName= "@statusId", Value =status.StatusId },
                };
                var response = sqlHelper.HandleReadData("khangGetDealByStatus", type, sqlParameters);
                if (!response.ContainsKey("Error"))
                {
                    responseHepler = new(0, "Success", response["Data"]);
                    return Ok(responseHepler.HandleResponse());
                }
                else
                {
                    responseHepler = new(-1, response["Error"], null);

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

    //chưa sử dụng
    public class Query
    {

        private readonly SqlHelper sqlHelper;

        public Query(SqlHelper _sqlHelper)
        {

            sqlHelper = _sqlHelper;

        }
        public Dictionary<String, object> GetListDealByStatus(int? statusId)
        {

            var type = CommandType.StoredProcedure;
            SqlParameter[] sqlParameters =
             {
                    new   SqlParameter() { ParameterName = "@statusId", Value = statusId==null?null:statusId },
             };
            Dictionary<String, object> res = new Dictionary<String, object>();
            var response = sqlHelper.HandleReadData("khangGetDealByStatus", type, sqlParameters);
            if (!response.ContainsKey("Error"))
            {
                res.Add("Data", response["Data"]);
            }
            else
            {
                res.Add("Error", response["Error"]);

            }

            return res;

        }
    }
}
