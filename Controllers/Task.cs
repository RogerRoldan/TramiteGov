using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Nodes;
using TramiteGov.Models;

namespace TramiteGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Task : ControllerBase
    {
        HttpClient client = new HttpClient();
        string BaseUrl = "http://localhost:8080/engine-rest/task";
        
        //Obtener informacion de una instancia de proceso
        [HttpGet("{idInstanced}")]
        public IActionResult GetTaskId(string idInstanced)
        {
            string Url = BaseUrl + "?processInstanceId=" + idInstanced;
            var Response = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;

            if (Response == "[]")
            {
                return NotFound();
            }
            return Content(Response, "application/json", Encoding.UTF8);
        }



        //Obtener tareas de un Rol o Usuario
        [HttpGet("role/{role}")]
        public IActionResult GetTaskRole(string role)
        {

            string JsonString = "{\"assigneeIn\":[\"" + role + "\"]}";

            var JsonContent = new StringContent(JsonString, Encoding.UTF8, "application/json");
            var Response = client.PostAsync(BaseUrl, JsonContent).Result.Content.ReadAsStringAsync().Result;

            if(Response == "[]")
            {
                return NotFound();
            }

            List<TaskCamunda> tasks = JsonConvert.DeserializeObject<List<TaskCamunda>>(Response);

            List<object> taskList = new List<object>();
            foreach (TaskCamunda task in tasks)
            {
                taskList.Add(new { Id = task.id, Name = task.name, InstacedId = task.processInstanceId });
            }
            
            return Content(JsonConvert.SerializeObject(taskList), "application/json", Encoding.UTF8);
        }

        //Completar una tarea, tambien se pueden agregar o modificar variables
        [HttpPost("complete/{idTask}")]
        public IActionResult PostCompleteTask(string idTask, [FromBody] VariableComplete variable)
        {
            string Url = BaseUrl + "/" + idTask + "/complete";
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
