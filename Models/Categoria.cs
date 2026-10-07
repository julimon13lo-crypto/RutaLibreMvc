using System.ComponentModel.DataAnnotations;

namespace RutaLibreMvc.Models
{
    // Tabla: Categorias
    public class Categoria
    {
        [Key]
        public int CategoriaID { get; set; }

        [Required]
        public string NombreCategoria { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
