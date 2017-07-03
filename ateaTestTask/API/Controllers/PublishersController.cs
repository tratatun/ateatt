using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Models;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class PublishersController : Controller
    {
        public IConfiguration _config;
        public PublishersController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public PublishersResponse Get()
        {
            PublishersResponse response = new PublishersResponse
            {
                StatusCode = Constants.RESPONSE_CODE_OK,
                Description = Constants.RESPONSE_OK,
            };
            try
            {
                //string computerName = _config.GetSection("CumputerName").Value;
                ApplicationInfoContext dbContext = new ApplicationInfoContext();
                ClientComputer comp = dbContext.ClientComputers.OrderBy(cc => cc.LastUpdated).LastOrDefault();

                if (comp != null)
                {
                    dbContext.Entry(comp).Collection(c => c.ApplicationInfos).Load();
                    IQueryable<Publisher> publs = dbContext.Publishers.Where(p => comp.ApplicationInfos.Any(ai => p.Id == ai.PublisherId));
                    List<PublishersResponseItem> publisherResponseItems = new List<PublishersResponseItem>();

                    foreach (Publisher publisher in publs)
                    {
                        publisherResponseItems.Add(new PublishersResponseItem
                        {
                            Id = publisher.Id,
                            PublisherName = publisher.PublisherName,
                            NumberOfApplications = publisher.ApplicationInfos.Count
                        });
                    }
                    response.Publishers = publisherResponseItems;
                    response.ClientComputerName = comp.ComputerName;
                    response.LastUpdatedString = comp.LastUpdated.ToString(Constants.RETURN_DATE_TIME_FORMAT);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = ex.HResult;
                response.Description = ex.Message;
            }
            return response;
        }
    }
}
