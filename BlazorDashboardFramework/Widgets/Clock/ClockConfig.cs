﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Widgets.Clock
{
    public class ClockConfig
    {
        public string? Format { get; set; }
        public bool HideHeader { get; set; }
    }
}
