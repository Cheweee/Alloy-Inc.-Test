using System;

namespace TestApp.Data.Models
{
    public class Report
    {

    }

    public class CartReport
    {
        public int Summary { get; set; }
    }

    public class ReportGetOptions
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}