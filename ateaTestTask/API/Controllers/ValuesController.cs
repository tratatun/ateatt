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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public ApplicationInfoResponse Get(string computerName)
        {
            ApplicationInfoResponse response = new ApplicationInfoResponse
            {
                StatusCode = Constants.RESPONSE_CODE_OK,
                Description = Constants.RESPONSE_OK,
            };
            try
            {
                ApplicationInfoContext dbContext = new ApplicationInfoContext();
                ClientComputer comp = dbContext.ClientComputers.FirstOrDefault(x => x.ComputerName == computerName);
                if (comp != null)
                {
                    dbContext.Entry(comp).Collection(c => c.ApplicationInfos).Load();
                    IQueryable<Publisher> publs= dbContext.Publishers.Where(p => comp.ApplicationInfos.Any(ai => p.Id == ai.PublisherId));
                    List<ApplicationInfoResponseItem> ApplicationInfoList = new List<ApplicationInfoResponseItem>();

                    foreach (Publisher publisher in publs)
                    {
                        ApplicationInfoList.AddRange(publisher.ApplicationInfos.Select(
                            ai => new ApplicationInfoResponseItem
                            {
                                Id = ai.Id,
                                ClientComputerId = ai.ClientComputerId,
                                InstallDateString = ai.InstallDate == DateTime.MinValue
                                    ? string.Empty
                                    : ai.InstallDate.ToString(Constants.RETURN_DATE_FORMAT),
                                DisplayVersion = ai.DisplayVersion,
                                DisplayName = ai.DisplayName,
                                PublisherId = ai.PublisherId,
                                PublisherName = publisher.PublisherName
                            }));
                    }
                    response.ApplicationInfoList = ApplicationInfoList;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = ex.HResult;
                response.Description = ex.Message;
            }
            return response;
        }
        // POST api/values
        [HttpPost]
        public PostApplicationsResponse Post([FromBody] List<ApplicationInfoRequest> list)
        {
            PostApplicationsResponse response = new PostApplicationsResponse
            {
                Description = Constants.RESPONSE_OK,
                StatusCode = Constants.RESPONSE_CODE_OK,
                ApplicationInfosPostedCount = 0
            };

            try
            {
                ApplicationInfoContext dbContext = new ApplicationInfoContext();
                Response.ContentType = "application/json";

                if (list != null && list.Any())
                {
                    APIHelpers.UpdateApplicationInfo(list);
                }
                else
                {
                    response.Description = "List is empty";
                    return response;
                }

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        SaveToDbContext(list, dbContext, transaction);
                    }
                    catch (Exception e)
                    {
                        response.Description = "Saving data error. Transaction rollback.";
                        transaction.Rollback();
                        Console.WriteLine(e);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                response.StatusCode = e.HResult;
                response.Description = e.Message;
                return response;
            }

            response.ApplicationInfosPostedCount = list.Count;

            return response;
        }

        private static void SaveToDbContext(List<ApplicationInfoRequest> list, ApplicationInfoContext dbContext, IDbContextTransaction transaction)
        {
            List<Publisher> publishers = ExstractPublishersNames(list);
            string clientComputerName = ExtractClientComputerName(list);

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

        private static string ExtractClientComputerName(List<ApplicationInfoRequest> list)
        {
            List<string> clientComputerStringList = list.Select(app => app.PSComputerName).Distinct().OrderBy(x => x).ToList();
            string clientComputerName = clientComputerStringList.Find(cc => !string.IsNullOrWhiteSpace(cc));
            return clientComputerName;
        }

        private static List<Publisher> ExstractPublishersNames(List<ApplicationInfoRequest> list)
        {
            List<Publisher> publishers = new List<Publisher>();
            List<string> publishersStringList = list.Select(app => app.Publisher).Distinct().OrderBy(x => x).ToList();
            publishers.AddRange(publishersStringList.Select(pub => new Publisher {PublisherName = pub}));
            return publishers;
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
