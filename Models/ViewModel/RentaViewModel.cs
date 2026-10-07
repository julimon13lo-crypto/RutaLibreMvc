namespace RutaLibreMvc.Models.ViewModel
{
    public class RentaViewModel
    {
        // Listas que se cargan desde la base de datos
        public List<Vehiculo> Vehiculos { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public List<Empleado> Empleados { get; set; } = new();

        public string? TipoFiltro { get; set; }
        public string? CategoriaFiltro { get; set; }
        public string? TransmisionFiltro { get; set; }

        public int? VehiculoSeleccionadoId { get; set; }

        public DateTime FechaRecogida { get; set; } = DateTime.Today;
        public DateTime FechaDevolucion { get; set; } = DateTime.Today.AddDays(1);

        // Datos del cliente
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }

        // Datos de la renta / pago
        public int? EmpleadoId { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";

        public decimal TotalEstimado { get; set; }
        public string? Mensaje { get; set; }
        public string? TipoMensaje { get; set; }
    }
}
