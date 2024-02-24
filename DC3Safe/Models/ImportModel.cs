using System.ComponentModel.DataAnnotations;

namespace DC3Safe.Models
{
    public class ImportModel
    {
        [Required(ErrorMessage = "Seleccione la plantilla a importar")]
        public required IFormFile ImportFile { get; set; }
    }
}
