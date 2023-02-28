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
                int count = 1;
                foreach (var item in json)
                {
                    duration += float.Parse(item.durationInMillis.ToString());
                    count++;
                }
                duration = duration / 60000  /60 /(count-1);
                duration = (float)Math.Round(duration, 1);
            
                var  durationInHours = new
                    {
                        durationInHours = duration
                    };
                return Content(JsonConvert.SerializeObject(durationInHours), "application/json", Encoding.UTF8);
            }
            else
            {
                return NotFound();
            }

               
            
        }

    }
}
