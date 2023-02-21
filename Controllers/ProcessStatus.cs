using Microsoft.AspNetCore.Http;
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
            var ResponseRequest = JsonConvert.DeserializeObject<dynamic>(response).childActivityInstances[0].name;
            
            string ResponseRequestString = Convert.ToString(ResponseRequest);

            return Ok(ResponseRequestString);
        }
    }
}
