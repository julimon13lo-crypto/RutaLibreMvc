using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Mantenimientos
    public class Mantenimiento
    {
        [Key]
        public int MantenimientoID { get; set; }

        public int VehiculoID { get; set; }
        public Vehiculo? Vehiculo { get; set; }

        public DateTime FechaMantenimiento { get; set; } = DateTime.Now;

        [Required]
        public string TipoMantenimiento { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public decimal Costo { get; set; }
    }
}
