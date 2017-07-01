using Domain;

namespace API.Models
{
    public class ApplicationInfoResponseItem : ApplicationInfo
    {
        public string PublisherName { get; set; }
        public string InstallDateString { get; set; }
    }
}