using System.ComponentModel.DataAnnotations;

namespace DC3Safe.Models
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "El campo Nombre o razón social es requerido")]
        [MaxLength(100, ErrorMessage = "Máximo 100 carácteres")]
        [MinLength(10, ErrorMessage = "Mínimo 10 carácteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo SHCP es requerido")]
        [MinLength(10, ErrorMessage = "Mínimo 10 carácteres")]
        public string Shcp { get; set; }
    }
}
