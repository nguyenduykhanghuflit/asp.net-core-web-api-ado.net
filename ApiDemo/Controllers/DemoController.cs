using ApiDemo.Database;
using ApiDemo.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using Quartz.Util;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using ApiDemo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using SqlParameter = System.Data.SqlClient.SqlParameter;

namespace ApiDemo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly SqlHelper sqlHelper;
        private readonly TokenHelper tokenHelper;

        public DemoController(SqlHelper sqlHelper, TokenHelper tokenHelper)
        {
            this.sqlHelper = sqlHelper;
            this.tokenHelper = tokenHelper;
        }

        // TEST SERVER
        [HttpGet("/")]
        public IActionResult Ping()
        {
            return Ok("Ok server");
        }

        // LOGIN
        [HttpPost("/api/internship/Login")]
        public IActionResult Login(User user)
        {

            if (user.Username == "khang" && user.Password == "123") //không đúng
            {
                string token = tokenHelper.GenerateToken(user.Username);
                return Ok(new
                {
                    Success = true,
                    Message = "Authenticate success",
                    Token = token
                });

            }

            return Ok(new
            {
                Error = 1,
                Message = "Invalid username/password",

            });


        }

        // DEAL
        [HttpGet("/api/internship/getdeals")]

        public IActionResult GetDeals(int? pageNumber, string? filterType, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                int pageSize = 15;
                if (pageNumber == null || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                string sqlQuery = "khangGetDeal";
                List<SqlParameter> sqlParameters = new()
                    {
                        new SqlParameter() {ParameterName= "@PageNumber", Value =pageNumber},
                        new SqlParameter() {ParameterName= "@PageSize",Value =pageSize,},
                        new SqlParameter() {ParameterName= "@Type",Value =filterType,},
                        new SqlParameter() {ParameterName= "@StartDate",Value =startDate,},
                        new SqlParameter() {ParameterName= "@EndDate",Value =endDate,},
                    };

                var commandType = CommandType.StoredProcedure;


                var response = sqlHelper.HandleReadData(sqlQuery, commandType, sqlParameters.ToArray());

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


        //NHAN VIEN
        [HttpGet("/api/internship/employee")]
        // [Authorize]
        public IActionResult GetAllEmp()
        {
            try
            {
                string sqlQuery = "khangGetAllEmp";

                var commandType = CommandType.StoredProcedure;

                var response = sqlHelper.HandleReadData(sqlQuery, commandType);

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

        //NHAN VIEN - DEAL
        [HttpGet("/api/internship/employee/deal")]
        // [Authorize]
        public IActionResult GetEmpDeal()
        {
            try
            {

                string sqlQuery = "khangGetAllEmpDeal";

                var commandType = CommandType.StoredProcedure;

                var response = sqlHelper.HandleReadData(sqlQuery, commandType);

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


        //NHAN VIEN BY DEALID
        [HttpGet("/api/internship/employee/deal/{id}")]
        // [Authorize]
        public IActionResult GetEmpByDeal(int id)
        {
            try
            {

                string sqlQuery = "khangGetEmpByDeal";
                List<SqlParameter> sqlParameters = new()
                    {
                        new SqlParameter() {ParameterName= "@Id", Value =id},

                    };

                var commandType = CommandType.StoredProcedure;

                var response = sqlHelper.HandleReadData(sqlQuery, commandType, sqlParameters.ToArray());
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

    }



    /* if (1 == 1)
                 {
                     string query = "SELECT * FROM Branches";
     IDictionary<string, object> getData2 = sqlHelper.HandleReadData(query);
                     return StatusCode(200, getData2);
 }*/


}
