using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TramiteGov.Models;

namespace TramiteGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessStatus : ControllerBase
    {
        static string CamundaUrl;
        static string BaseUrl;

        public ProcessStatus()
        {
            CamundaUrl = CredencialEnvironment.GetCamundaUrl();
            BaseUrl = CamundaUrl + "/process-instance/";
        }


        HttpClient client = new HttpClient();

        //Conocer el estado de una instancia de proceso en que parte se encuentra
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

            if(ProcessStatus == "Respuesta Final") {
                return Ok(ProcessStatusString + ",https://drive.google.com/file/d/1ZBc62dNh-jcwNu2tPDt7tM_3p95a7JK4/view?usp=sharing");
            }
            else { 
            return Ok(ProcessStatusString + ",null");            
            }
        }
    }
}
