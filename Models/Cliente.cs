using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Clientes
    public class Cliente
    {
        [Key]
        public int Id { get; set; }                       // columna ClienteID

        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La cédula solo debe contener números")]
        public string Cedula { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "El teléfono solo debe contener números")]
        public string? Telefono { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
