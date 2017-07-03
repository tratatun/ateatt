using System.Collections.Generic;
using Domain;

namespace API.Models
{
    public class ApplicationInfoResponse : BaseResponse
    {
        public IEnumerable<ApplicationInfoResponseItem> ApplicationInfoList { get; set; }
        public string ClientComputerName { get; set; }
        public string LastUpdatedString { get; set; }
    }
}