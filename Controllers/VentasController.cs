using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniMarketWebApp.Data;
using MiniMarketWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace MiniMarketWebApp.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly MiniMarketContext _context;

        public VentasController(MiniMarketContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            return View(ventas);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["Productos"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] int[] IdProducto, [FromForm] int[] Cantidad)
        {
            if (IdProducto.Length == 0 || Cantidad.Length == 0)
            {
                TempData["error"] = "Debes agregar al menos un producto.";
                return RedirectToAction(nameof(Create));
            }

            decimal total = 0;
            var detalles = new List<VentaDetalle>();

            for (int i = 0; i < IdProducto.Length; i++)
            {
                var producto = await _context.Productos.FindAsync(IdProducto[i]);
                if (producto == null) continue;

                if (Cantidad[i] > producto.Stock)
                {
                    TempData["error"] = $"No hay suficiente stock para el producto {producto.Nombre}.";
                    return RedirectToAction(nameof(Create));
                }

                var subtotal = producto.Precio * Cantidad[i];
                total += subtotal;

                detalles.Add(new VentaDetalle
                {
                    IdProducto = producto.IdProducto,
                    Cantidad = Cantidad[i],
                    PrecioUnitario = producto.Precio,
                    SubTotal = subtotal
                });

                // Actualizar stock
                producto.Stock -= Cantidad[i];
                _context.Productos.Update(producto);
            }

            var venta = new Venta
            {
                Fecha = DateTime.Now,
                Total = total,
                Detalles = detalles
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            TempData["success"] = "Venta registrada correctamente ✅";
            return RedirectToAction(nameof(Index));
        }
        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

    }
}
