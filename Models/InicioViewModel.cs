namespace InventarioExpres.Models;

public class InicioViewModel
{
    public int TotalProductos { get; set; }
    public int TotalProveedores { get; set; }
    public List<Producto> UltimosProductos { get; set; } = new();
    public List<Proveedor> UltimosProveedores { get; set; } = new();
    public List<Proveedor> TodosProveedores { get; set; } = new();
}