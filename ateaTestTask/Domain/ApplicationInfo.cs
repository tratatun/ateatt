﻿using System;

namespace Domain
{
    public class ApplicationInfo
    {
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public DateTime InstallDate { get; set; }
        public int PublisherId { get; set; }
        public int PSComputerId { get; set; }
    }
}