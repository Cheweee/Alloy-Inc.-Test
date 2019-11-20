namespace TestApp.Data.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public double DeliveryPrice { get; set; }
        public string Name { get; set; }
    }

    public class DeliveryGetOptions : BaseGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
        public string Name { get; set; }
    }
}