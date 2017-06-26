using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Models;
using DataAccess;
using Domain;
using EFLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value " + id;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] List<ApplicationInfoRequest> list)
        {
            ApplicationInfoContext dbContext = new ApplicationInfoContext();
            dbContext.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            List<Publisher> publishers = new List<Publisher>();

            APIHelpers.UpdateApplicationInfo(list);

            List<string> stringList = list.Select(app => app.Publisher).Distinct().OrderBy(x => x).ToList();

            publishers.AddRange(stringList.Select(pub => new Publisher {PublisherName = pub}));

            stringList = list.Select(app => app.PSComputerName).Distinct().OrderBy(x => x).ToList();
            string clientComputerName = stringList.Find(cc => !string.IsNullOrWhiteSpace(cc));

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {

                    ClientComputer clientComputer =
                        dbContext.ClientComputers.FirstOrDefault(c => c.ComputerName == clientComputerName);
                    if (clientComputer == null)
                    {
                        clientComputer = new ClientComputer
                        {
                            ComputerName = clientComputerName,
                            LastUpdated = DateTime.Now,
                        };
                        dbContext.ClientComputers.Add(clientComputer);
                    }
                    else
                    {
                        clientComputer.LastUpdated = DateTime.Now;
                        dbContext.ApplicationsInfos.RemoveRange(
                            dbContext.ApplicationsInfos.Where(ai => ai.ClientComputerId == clientComputer.Id));
                    }

                    foreach (Publisher publisher in publishers)
                    {
                        Publisher existedPublisher =
                            dbContext.Publishers.FirstOrDefault(p => p.PublisherName == publisher.PublisherName);
                        if (existedPublisher == null)
                        {
                            publisher.ApplicationInfos = GetApplicationInfoLists(list, publisher, clientComputer);
                            dbContext.Publishers.Add(publisher);
                            dbContext.ApplicationsInfos.AddRange(publisher.ApplicationInfos);
                        }
                        else
                        {
                            existedPublisher.ApplicationInfos =
                                GetApplicationInfoLists(list, existedPublisher, clientComputer);
                            dbContext.Publishers.Update(existedPublisher);
                            dbContext.ApplicationsInfos.AddRange(existedPublisher.ApplicationInfos);
                        }
                    }
                    dbContext.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("Ok '" + (list != null ? list.Count.ToString() : "null") + "'");
            return "Ok '" + (list != null ? list.Count.ToString() : "null") + "'";
        }

        private static List<ApplicationInfo> GetApplicationInfoLists(List<ApplicationInfoRequest> list, Publisher publisher, ClientComputer clientComputer)
        {
            return list.Where(app => app.Publisher == publisher.PublisherName).Select(
                app => new ApplicationInfo
                {
                    DisplayName = app.DisplayName,
                    InstallDate = app.InstallDateParsed,
                    DisplayVersion = app.DisplayVersion,
                    ClientComputerId = clientComputer.Id,

                }).ToList();
        }
    }
}
