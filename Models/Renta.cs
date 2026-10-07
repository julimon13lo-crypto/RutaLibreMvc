using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Rentas
    public class Renta
    {
        [Key]
        public int Id { get; set; }                       // columna RentaID

        public int VehiculoId { get; set; }               // columna VehiculoID
        public Vehiculo? Vehiculo { get; set; }

        public int ClienteId { get; set; }                // columna ClienteID
        public Cliente? Cliente { get; set; }

        public int? EmpleadoId { get; set; }              // columna EmpleadoID (opcional)
        public Empleado? Empleado { get; set; }

        public DateTime FechaRecogida { get; set; }
        public DateTime FechaDevolucion { get; set; }

        public decimal TotalEstimado { get; set; }

        public string EstadoRenta { get; set; } = "Activa";

        public DateTime? FechaRegistro { get; set; } = DateTime.Now;

        public List<Pago> Pagos { get; set; } = new();
    }
}
