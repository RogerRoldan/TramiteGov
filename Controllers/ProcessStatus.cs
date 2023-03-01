using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TramiteGov.Environtment;

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
            BaseUrl = CamundaUrl ;
        }

        HttpClient client = new HttpClient();

        //Conocer el estado de una instancia de proceso en que parte se encuentra
        [HttpGet("{idInstanced}")]
        public IActionResult GetStatus(string idInstanced)
        {
            string Url = BaseUrl+ "/history/process-instance/" + idInstanced;
            var response = client.GetAsync(Url).Result;

            if (response.IsSuccessStatusCode)
            {
                var ResponseContent = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<dynamic>(ResponseContent);

                string status = json.state;

                switch (status)
                {
                    case "COMPLETED":
                        return Ok("Respuesta Final" + ",https://drive.google.com/file/d/1ZBc62dNh-jcwNu2tPDt7tM_3p95a7JK4/view?usp=sharing");
                    case "ACTIVE":
                        
                        string UrlActive = BaseUrl + "/process-instance/" + idInstanced + "/activity-instances";
                        var responseActive = client.GetAsync(UrlActive).Result.Content.ReadAsStringAsync().Result;
                        var jsonActive = JsonConvert.DeserializeObject<dynamic>(responseActive);
                        string activity = jsonActive.childActivityInstances[0].name;
                        return Ok(activity + ",null");
                        
                    case "SUSPENDED":
                        return Ok("Proceso suspendido temporalmente,null");
                    case "EXTERNALLY_TERMINATED":
                        return Ok("Proceso Cancelado,null");
                    default:
                        return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

           
        }
    }
}
