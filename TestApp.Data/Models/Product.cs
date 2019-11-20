using System.Collections.Generic;

namespace TestApp.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
    }

    public class Cart
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
    }

    public class ProductGetOptions : BaseGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
        public string Name { get; set; }
    }

    public class CartGetOptions : BaseGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
        public string Name { get; set; }
        public bool? Ordered { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public IReadOnlyList<int> OrderIds { get; set; }
    }
}