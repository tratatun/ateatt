using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Models;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value " + id;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]List<ApplicationInfoRequest> list)
        {
            ApplicationInfoContext dbContext = new ApplicationInfoContext();
            List<Publisher> publishers = new List<Publisher>();
            List<ClientComputer> clientComputers = new List<ClientComputer>();

            APIHelpers.UpdateApplicationInfo(list);

            List<string> stringList = list.Select(app => app.Publisher).Distinct().OrderBy(x => x).ToList();

            publishers.AddRange(stringList.Select(pub => new Publisher {PublisherName = pub}));

            foreach (ApplicationInfoRequest applicationInfoRequest in list)
            {

            }

            Console.WriteLine("Ok '" + (list != null ? list.Count.ToString() : "null") + "'");
            return "Ok '" + (list != null ? list.Count.ToString() : "null") + "'";
        }
    }
}
