using System.ComponentModel.DataAnnotations;

namespace InventarioExpres.Models;

public class Proveedor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone, StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    public List<Producto> Productos { get; set; } = new();
}