using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text;
using TramiteGov.Models;
using TramitesGov.Models;

namespace TramitesGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartProcess : ControllerBase
    {
        string CamundaUrl;
        string BaseUrl;
        string NameProcess;

        public StartProcess()
        {
            CamundaUrl = CredencialEnvironment.GetCamundaUrl();
            NameProcess = CredencialEnvironment.GetNameProcess();
            BaseUrl = CamundaUrl + "/process-definition/key/";
        }
        HttpClient client = new HttpClient();
        
        //Iniciar una instancia de proceso
        [HttpPost]
        public IActionResult Post([FromBody] VariableInitial variable)
        {          
            string Url = BaseUrl + NameProcess + "/start";

            var json = JsonConvert.SerializeObject(variable);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Url , data).Result.Content.ReadAsStringAsync().Result;

            var ResponseRequest = JsonConvert.DeserializeObject<dynamic>(response);
            return Ok(Convert.ToString(ResponseRequest.id));
        }
    }
}
