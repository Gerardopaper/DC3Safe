using Microsoft.AspNetCore.Mvc;

namespace DC3Safe.Models
{
    public class DataTableRequest
    {
        [FromQuery(Name = "draw")]
        public int draw { get; set; }

        [FromQuery(Name = "start")]
        public int start { get; set; }

        [FromQuery(Name = "length")]
        public int length { get; set; }

        [FromQuery(Name = "search[value]")]
        public string? search { get; set; }

        [FromQuery(Name = "order[0][dir]")]
        public string dir { get; set; } = string.Empty;

        [FromQuery(Name = "order[0][column]")]
        public int column { get; set; }
    }
}
