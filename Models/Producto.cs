using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioExpres.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    public int Stock { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }
    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
}