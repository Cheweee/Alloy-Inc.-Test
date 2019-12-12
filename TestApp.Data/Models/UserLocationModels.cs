using System.Net;

namespace TestApp.Data.Models
{
    public class UserLocation
    {
        public int? Id { get; set; }
        public string IPAddress { get; set; }
        public string Location { get; set; }
    }

    public class UserLocationGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
    }
}