using System.ComponentModel.DataAnnotations;

namespace DC3Safe.Models
{
    public class WorkerInformation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string Curp { get; set; }
        public string OccupationId { get; set; }
        public Occupation Occupation { get; set; }
        public string Position { get; set; }
    }
}
