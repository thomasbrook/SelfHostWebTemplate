﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostWeb.WebApi.Parts
{
    public class SimpleException
    {
        public List<string> Message { get; set; }
        public List<string> StackTrace { get; set; }
    }
}
