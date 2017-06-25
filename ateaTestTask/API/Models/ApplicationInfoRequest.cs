namespace API.Models
{
    public class ApplicationInfoRequest
    {
        public string DisplayName { get; set; }
        public string Publisher { get; set; }
        public string DisplayVersion { get; set; }
        public string InstallDate { get; set; }
        public string PSComputerName { get; set; }
    }
}