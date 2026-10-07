using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Empleados
    public class Empleado
    {
        [Key]
        public int EmpleadoID { get; set; }

        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        public string Cedula { get; set; } = string.Empty;

        public string Cargo { get; set; } = "Asesor de Ventas";

        public string? Telefono { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}
