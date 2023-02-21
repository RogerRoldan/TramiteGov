﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TramiteGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessStatus : ControllerBase
    {
        HttpClient client = new HttpClient();
        string BaseUrl = "http://localhost:8080/engine-rest/process-instance/";
        

        [HttpGet("{idInstanced}")]
        public IActionResult GetStatus(string idInstanced)
        {
            string Url = BaseUrl + idInstanced + "/activity-instances";
            var response = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
            var ResponseRequest = JsonConvert.DeserializeObject<dynamic>(response);
            
            if (ResponseRequest.type == "InvalidRequestException")
            {
                return NotFound();
            }
            
            var ProcessStatus = ResponseRequest.childActivityInstances[0].name;

            string ProcessStatusString = Convert.ToString(ProcessStatus);

            return Ok(ProcessStatusString);
        }
    }
}
