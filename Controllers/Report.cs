using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TramiteGov.Environtment;

namespace TramiteGov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Report : ControllerBase
    {
        static string CamundaUrl;
        static string BaseUrl;
        HttpClient client = new HttpClient();

        public Report()
        {
            CamundaUrl = CredencialEnvironment.GetCamundaUrl();
            BaseUrl = CamundaUrl;
        }

        //Reporte de Actividades y el numero de instancias activas que tienen
        [HttpGet("AmountActivities/{nameProcess}")]
        public ActionResult GetStadistics(string nameProcess)
        {
            string Url = BaseUrl + "/process-definition/key/" + nameProcess + "/statistics";
            var response = client.GetAsync(Url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var ResponseContent = response.Content.ReadAsStringAsync().Result; 
                
                List<object> list = new List<object>();
                var json = JsonConvert.DeserializeObject<dynamic>(ResponseContent);
                foreach (var item in json)
                {
                    list.Add(new
                    {
                        id = item.id,
                        instances = item.instances,
                    });
                }
                return Content(JsonConvert.SerializeObject(list), "application/json", Encoding.UTF8);
            }
            else
            {
                return NotFound();

            }
        }

        //reporte de duracion promedio que tiene una instancia de proceso finalizada
        [HttpGet("AvgDuration/{nameProcess}/{amount}")]
        public ActionResult GetDuration(string nameProcess, int amount)
        {
            string Url = BaseUrl + "/history/process-instance?processDefinitionKey=" + nameProcess + "&finished=true&maxResults=" + amount;
            var response = client.GetAsync(Url).Result;
            if (response.IsSuccessStatusCode)
            {
                var ResponseContent = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<dynamic>(ResponseContent);
                float duration = 0;
                int count = duration >= 0 ? 1 : 0;
                foreach (var item in json)
                {
                    duration += float.Parse(item.durationInMillis.ToString());
                    count++;
                }
                duration = duration / count-1;
                var durationHours = TimeSpan.FromMilliseconds((double)duration).ToString(@"hh\:mm\:ss");

                var durationInHours = new
                {
                    durationInHours = durationHours
                };
                return Content(JsonConvert.SerializeObject(durationInHours), "application/json", Encoding.UTF8);
            }
            else
            {
                return NotFound();
            }     
        }

        //reporte de las ultimas actividades completadas y su duracion
        [HttpGet("LastActivities/{nameProcess}/{amount}")]
        public ActionResult GetLastActivities(string nameProcess, int amount)
        {
            string Url = BaseUrl + "/history/task?processDefinitionKey=" + nameProcess + "&finished=true&maxResults=" + amount + "&sortBy=startTime&sortOrder=desc";
            var response = client.GetAsync(Url).Result;
            if (response.IsSuccessStatusCode)
            {
                var ResponseContent = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<dynamic>(ResponseContent);
                List<object> list = new List<object>();
                foreach (var item in json)
                {
                    string status = "";
                    if (item.deleteReason == "completed")
                    {
                        status = "Completada";
                    }
                    else
                    {
                        status = "Cancelada";
                    }
                    
                    list.Add(new
                    {                      
                        name = item.name,
                        duration = TimeSpan.FromMilliseconds((double)item.duration).ToString(@"hh\:mm\:ss"),
                        assignee = item.assignee,
                        status = status

                    });
                }
                return Content(JsonConvert.SerializeObject(list), "application/json", Encoding.UTF8);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
