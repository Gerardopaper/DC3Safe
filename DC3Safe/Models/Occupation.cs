using System.ComponentModel.DataAnnotations;

namespace DC3Safe.Models
{
    public class Occupation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
    }
}
