using System;
using System.Collections.Generic;

namespace TestApp.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public int PaymentMethod { get; set; }
        public string FullName { get; set; }
        public string Addres { get; set; }
        public DateTime? SpecialDate { get; set; }
        public bool ToSpecialDate => SpecialDate.HasValue;

        public List<Cart> Products { get; set; }
    }

    public class OrderGetOptions : BaseGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
    }
}