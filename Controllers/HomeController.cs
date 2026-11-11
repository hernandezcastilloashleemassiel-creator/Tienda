using InventarioExpres.Data;
using InventarioExpres.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tienda.Models;

namespace InventarioExpres.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var modelo = new InicioViewModel
        {
            TotalProductos = await _context.Productos.CountAsync(),
            TotalProveedores = await _context.Proveedores.CountAsync(),
            UltimosProductos = await _context.Productos
                .Include(p => p.Proveedor)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync(),
            UltimosProveedores = await _context.Proveedores
                .Include(p => p.Productos)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync(),
            TodosProveedores = await _context.Proveedores
                .OrderBy(p => p.Nombre)
                .ToListAsync()
        };
        return View(modelo);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}