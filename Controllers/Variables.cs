using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TramiteGov.Environtment;
using TramiteGov.Models;

namespace TramiteGov.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class Variables : ControllerBase
    {
        string CamundaUrl;
        string BaseUrl;

        public Variables()
        {
            CamundaUrl = CredencialEnvironment.GetCamundaUrl();
            BaseUrl = CamundaUrl + "/process-instance/";
        }

        HttpClient client = new HttpClient();        
        
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
        //Upload File Camunda
        [HttpPost("uploadfile/{idInstanced}/{nameVariable}")]
        public IActionResult PostUploadFile(string idInstanced,string nameVariable ,[FromForm] IFormFile file)
        {
            string Url = BaseUrl + idInstanced + "/variables/" + nameVariable + "/data";
            client.BaseAddress = new Uri(Url);

            var streamContent = new StreamContent(file.OpenReadStream());
            string extension = Path.GetExtension(file.FileName);
            
            var content = new MultipartFormDataContent();
            content.Add(streamContent, "data", nameVariable + extension);
            content.Add(new StringContent("File"), "valueType");

            var response = client.PostAsync("", content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;


            return Ok(responseContent);
        }

        //obtener variable de archivo
        [HttpGet("getfile/{idInstanced}/{nameVariable}")]
        public IActionResult GetFile(string idInstanced, string nameVariable)
        {
            string Url = BaseUrl + idInstanced + "/variables/" + nameVariable + "/data";
            var Response = client.GetAsync(Url).Result;
            
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                var nameFile = Response.Content.Headers.ContentDisposition.FileNameStar;
                string extension = Path.GetExtension(nameFile);

                var ResponseContent = Response.Content.ReadAsStreamAsync().Result;

                return File(ResponseContent, "application/octet-stream", nameFile);
            }
            else
            {
                return BadRequest();
            }


        }
    }
}


//string path = @"C:\Users\Roger Roldan\Downloads\arc.txt";
//using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
//using var streamContent = new StreamContent(fileStream);