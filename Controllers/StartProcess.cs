using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text;
using TramitesGov.Models;

namespace TramitesGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartProcess : ControllerBase
    {
            HttpClient client = new HttpClient();
            string BaseUrl = "http://localhost:8080/engine-rest/process-definition/key/";
            string NameProcess = "TramiteGov";

        [HttpPost]
        public IActionResult Post([FromBody] VariableInitial variable)
        {          
            string Url = BaseUrl + NameProcess + "/start";

            //VariableInitial variableInitial = new VariableInitial();
            //variableInitial.variables = new Dictionary<string, AtributeInitial>();
            //variableInitial.Add("Identificacion", "", "string");
            //variableInitial.Add("Nombre", "", "string");
            //variableInitial.Add("Apellido", "", "string");
            //variableInitial.Add("Correo", "", "string");

            var json = JsonConvert.SerializeObject(variable);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Url , data).Result.Content.ReadAsStringAsync().Result;

            var ResponseRequest = JsonConvert.DeserializeObject<dynamic>(response);
            return Ok(Convert.ToString(ResponseRequest.id));
        }
    }
}
