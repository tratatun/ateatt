using System.Collections.Generic;
using Domain;

namespace API.Models
{
    public class PublishersResponse : BaseResponse
    {
        public IEnumerable<PublishersResponseItem> Publishers { get; set; }
        public string ClientComputerName { get; set; }
        public string LastUpdatedString { get; set; }
    }
}