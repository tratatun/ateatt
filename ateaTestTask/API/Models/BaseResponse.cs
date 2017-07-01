namespace API.Models
{
    public abstract class BaseResponse
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
    }
}