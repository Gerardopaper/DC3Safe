using System.ComponentModel.DataAnnotations;

namespace DC3Safe.Models
{
    public class ProgramInformation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        public int DurationHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CategoryId { get; set; }
        public ProgramCategory Category { get; set; }
    }
}
