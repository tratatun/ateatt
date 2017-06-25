using System.Collections.Generic;

namespace Domain
{
    public class Publisher
    {
        public int Id { get; set; }
        public string PublisherName { get; set; }
        public  List<ApplicationInfo> ApplicationInfos { get; set; }
    }
}