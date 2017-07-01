using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace WebPortal.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDataAsync(string computerName)
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET core");
            string uri = _config.GetSection("RESTService").Value + "?computerName=" + computerName;

            Console.WriteLine("URI: " + uri);

            Task<string> task = client.GetStringAsync(uri);
            string result = await task;

            return Json(JsonConvert.DeserializeObject(result));
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
