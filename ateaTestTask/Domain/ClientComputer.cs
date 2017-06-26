using System;
using System.Collections.Generic;

namespace Domain
{
    public class ClientComputer
    {
        public int Id { get; set; }
        public string ComputerName { get; set; }
        public DateTime LastUpdated { get; set; }
        public  List<ApplicationInfo> ApplicationInfos { get; set; }
    }
}