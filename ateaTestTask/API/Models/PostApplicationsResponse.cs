namespace API.Models
{
    public class PostApplicationsResponse : BaseResponse
    {
        public int ApplicationInfosPostedCount { get; set; }
        public string ResponseDateTime { get; set; }
    }
}