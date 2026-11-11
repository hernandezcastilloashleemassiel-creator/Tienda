using InventarioExpres.Data;
using InventarioExpres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioExpres.Controllers;

public class ProveedoresController : Controller
{
    private readonly AppDbContext _context;
    public ProveedoresController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
        => View(await _context.Proveedores.OrderBy(p => p.Nombre).ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Telefono")] Proveedor proveedor, bool desdeInicio = false)
    {
        if (!ModelState.IsValid) return View(proveedor);
        _context.Add(proveedor);
        await _context.SaveChangesAsync();
        TempData["Ok"] = "Proveedor guardado correctamente.";
        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();
        return View(proveedor);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono")] Proveedor proveedor, bool desdeInicio = false)
    {
        if (id != proveedor.Id) return NotFound();
        if (!ModelState.IsValid) return View(proveedor);
        _context.Update(proveedor);
        await _context.SaveChangesAsync();
        TempData["Ok"] = "Proveedor actualizado correctamente.";
        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, bool desdeInicio = false)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor == null) return NotFound();

        if (await _context.Productos.AnyAsync(p => p.ProveedorId == id))
        {
            TempData["Error"] = "No se puede eliminar: el proveedor tiene productos asociados.";
        }
        else
        {
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Proveedor eliminado correctamente.";
        }

        return desdeInicio
            ? RedirectToAction("Index", "Home")
            : RedirectToAction(nameof(Index));
    }
}