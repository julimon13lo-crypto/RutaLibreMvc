using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Pagos
    public class Pago
    {
        [Key]
        public int PagoID { get; set; }

        public int RentaID { get; set; }
        public Renta? Renta { get; set; }

        public DateTime? FechaPago { get; set; } = DateTime.Now;

        public decimal Monto { get; set; }

        [Required]
        public string MetodoPago { get; set; } = "Efectivo";
    }
}
