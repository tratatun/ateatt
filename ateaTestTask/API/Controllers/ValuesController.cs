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
        [HttpGet]
        //[AutoValidateAntiforgeryToken]
        public IEnumerable<Publisher> Get(string computerName)
        {
            ApplicationInfoContext dbContext = new ApplicationInfoContext();
            ClientComputer comp = dbContext.ClientComputers.FirstOrDefault(x => x.ComputerName == computerName);
            IEnumerable<Publisher> publishers = new List<Publisher>();
            if (comp != null)
            {
                dbContext.Entry(comp).Collection(c => c.ApplicationInfos).Load();
                publishers = dbContext.Publishers.Where(p => comp.ApplicationInfos.Any(ai => p.Id == ai.PublisherId));
            }
            return publishers;
        }
        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody] List<ApplicationInfoRequest> list)
        {
            ApplicationInfoContext dbContext = new ApplicationInfoContext();
            List<Publisher> publishers = new List<Publisher>();
            Response.ContentType = "application/json";
            if (list != null)
            {
                APIHelpers.UpdateApplicationInfo(list);
            }
            else
            {
                return Json("{error:'list is empty'}");
            }

            List<string> stringList = list.Select(app => app.Publisher).Distinct().OrderBy(x => x).ToList();

            publishers.AddRange(stringList.Select(pub => new Publisher {PublisherName = pub}));

            stringList.Clear();

            stringList = list.Select(app => app.PSComputerName).Distinct().OrderBy(x => x).ToList();
            string clientComputerName = stringList.Find(cc => !string.IsNullOrWhiteSpace(cc));

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // if Client Computer exists in DB than just change update date, if not htan create new
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

                    // if publisher doesn't exist than create new
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
            return Json("{response:'Ok " + (list != null ? list.Count.ToString() : "null") + "'}");
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
