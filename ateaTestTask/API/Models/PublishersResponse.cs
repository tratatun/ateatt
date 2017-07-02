using System.Collections.Generic;
using Domain;

namespace API.Models
{
    public class PublishersResponse : BaseResponse
    {
        public IEnumerable<PublishersResponseItem> Publishers { get; set; }
    }
}