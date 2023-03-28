using ApiDemo.Database;
using ApiDemo.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection.PortableExecutable;

namespace ApiDemo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly SqlHelper sqlHelper;

        public DemoController(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        [HttpGet("/api/demo/getdata")]
        public IActionResult Index()
        {
            try
            {
                string today = DateTime.Now.ToString("MM/dd/yyyy");

                //for stored proc
                string sqlStored = "GetAllRevenues";
                List<SqlParameter> sqlParameters = new()
                {
                    new SqlParameter() {ParameterName= "@StartDate", Value =today},
                    new SqlParameter() {ParameterName= "@EndDate",Value =today,},
                };
                var commandType = CommandType.StoredProcedure;


                IDictionary<string, object> getData = sqlHelper.HandleReadData(sqlStored, commandType, sqlParameters.ToArray());

                if ((int)getData["Error"] == 0)
                {
                    getData["Message"] = "Succes";
                    return StatusCode(200, getData);
                }
                else
                {
                    return BadRequest(getData);
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }


    }





}
