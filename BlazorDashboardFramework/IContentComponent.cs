﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public interface IContentComponent
    {
        public WidgetInstance Widget { get; set; }
    }
}
