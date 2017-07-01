﻿using System.Collections.Generic;
using Domain;

namespace API.Models
{
    public class ApplicationInfoResponse : BaseResponse
    {
        public IEnumerable<Publisher> Publishers { get; set; }
    }
}