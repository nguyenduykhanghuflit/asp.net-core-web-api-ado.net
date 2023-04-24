using ApiDemo.Services;
using FirebaseAdmin.Messaging;
using Google.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealNotifyController : ControllerBase
    {

        public DealNotifyController()
        {

        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            try
            {

                SendNotify send = new SendNotify();
                var res = send.HandleSendNotify();


                return Ok(res);
            }
            catch (Exception exp)
            {
                return Ok(exp.Message);
            }
        }

    }



}
