using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TramiteGov.Models;

namespace TramiteGov.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Variables : ControllerBase
    {
        HttpClient client = new HttpClient();        
        string BaseUrl = "http://localhost:8080/engine-rest/process-instance/";

        //Obtener variables de una instancia de proceso
        [HttpGet("{idInstanced}")]
        public dynamic GetVariablesId(string idInstanced)
        {
            string Url = BaseUrl + idInstanced + "/variables";
            var Response = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
            var type = JsonConvert.DeserializeObject<dynamic>(Response).type;
            
            if (type == "NullValueException")
            {
                return BadRequest();
            }

            return Content(Response, "application/json", Encoding.UTF8);
        }

        //Crear o modificar variables de una instancia de proceso
        [HttpPost("{idInstanced}")]
        public IActionResult PostNewVariable(string idInstanced, [FromBody] VariableModification variable)
        {
            string Url = BaseUrl + idInstanced + "/variables";
            var JsonContent = new StringContent(JsonConvert.SerializeObject(variable), Encoding.UTF8, "application/json");
            var Response = client.PostAsync(Url, JsonContent).Result.Content.ReadAsStringAsync().Result;
            var ResponseString = JsonConvert.DeserializeObject<dynamic>(Response);
            
            if (Response != "")
            {
                return BadRequest();
            }

            return Content(Response, "application/json", Encoding.UTF8);
        }
    }
}
