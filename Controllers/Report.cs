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
            BaseUrl = CamundaUrl + "/process-definition/key/";
        }

        [HttpGet("{nameProcess}")]
        public ActionResult GetStadistics(string nameProcess)
        {
            string Url = BaseUrl + nameProcess + "/statistics";
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

    }
}
