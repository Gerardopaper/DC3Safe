namespace DC3Safe.Models
{
    public class DC3Report
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Identifier { get; set; } = null!;

        public DateTime CreateTime { get; set; }
    }
}
