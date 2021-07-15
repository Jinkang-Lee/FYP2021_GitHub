using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Collections.Generic;

namespace FYP2021.Models
{
    public class ReportChart
    {

        public string ReportChartGUID { get; set; }

        public int PendingForTransitLink { get; set; }

        public int ReadyForApplication { get; set; }

        public int CardReady { get; set; }

        public int CardDispatched { get; set; }

    }

}
