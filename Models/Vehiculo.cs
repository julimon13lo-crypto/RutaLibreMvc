using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RutaLibreMvc.Models
{
    // Tabla: Vehiculos
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }                       // columna VehiculoID

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(10)]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio")]
        public string TipoVehiculo { get; set; } = "Carro"; // columna agregada con el script de integración

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = string.Empty;

        public int Anio { get; set; }                     // columna [Año]

        public decimal PrecioDia { get; set; }

        public string Transmision { get; set; } = "Manual";

        public string Estado { get; set; } = "Disponible";

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría")]
        public int CategoriaId { get; set; }              // columna CategoriaID

        [ValidateNever]
        public Categoria? Categoria { get; set; }

        [NotMapped]
        public string NombreCategoria => Categoria?.NombreCategoria ?? string.Empty;
    }
}
