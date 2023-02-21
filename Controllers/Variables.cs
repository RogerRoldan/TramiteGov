using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TramiteGov.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Variables : ControllerBase
    {
        HttpClient client = new HttpClient();
        string BaseUrl = "http://localhost:8080/engine-rest/process-instance/";

        [HttpGet("{idInstanced}")]
        public IActionResult Get(string idInstanced)
        {
            string Url = BaseUrl + idInstanced + "/variables";
            var Response = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;


            return Ok(Response);
        }
    }
}
