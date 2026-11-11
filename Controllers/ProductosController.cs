using InventarioExpres.Data;
using InventarioExpres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventarioExpres.Controllers;

public class ProductosController : Controller
{
    private readonly AppDbContext _context;
    public ProductosController(AppDbContext context) => _context = context;

    private void CargarProveedores(int? seleccionado = null)
        => ViewBag.Proveedores = new SelectList(
            _context.Proveedores.OrderBy(p => p.Nombre), "Id", "Nombre", seleccionado);

    public async Task<IActionResult> Index()
        => View(await _context.Productos.Include(p => p.Proveedor).OrderBy(p => p.Nombre).ToListAsync());

    public IActionResult Create()
    {
        CargarProveedores();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Stock,Precio,ProveedorId")] Producto producto, bool desdeInicio = false)
    {
        if (!ModelState.IsValid)
        {
            CargarProveedores(producto.ProveedorId);
            return View(producto);
        }
        _context.Add(producto);
        await _context.SaveChangesAsync();
        TempData["Ok"] = "Producto guardado correctamente.";
        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return NotFound();
        CargarProveedores(producto.ProveedorId);
        return View(producto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Stock,Precio,ProveedorId")] Producto producto, bool desdeInicio = false)
    {
        if (id != producto.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            CargarProveedores(producto.ProveedorId);
            return View(producto);
        }
        _context.Update(producto);
        await _context.SaveChangesAsync();
        TempData["Ok"] = "Producto actualizado correctamente.";
        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, bool desdeInicio = false)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return NotFound();
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        TempData["Ok"] = "Producto eliminado correctamente.";
        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }
}