using System.Collections.Generic;

namespace TestApp.Data.Models
{
    public abstract class BaseGetOptions
    {
        public int? Id { get; set; }
        public IReadOnlyList<int> Ids { get; set; }
        public IReadOnlyList<int> ExcludeIds { get; set; }
    }
}