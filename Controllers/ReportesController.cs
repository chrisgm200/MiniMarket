using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarketWebApp.Data;
using MiniMarketWebApp.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace MiniMarketWebApp.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly MiniMarketContext _context;

        public ReportesController(MiniMarketContext context)
        {
            _context = context;
        }

        // GET: Reportes
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idProducto)
        {
            ViewData["Categorias"] = await _context.Categorias.ToListAsync();
            ViewData["Productos"] = await _context.Productos.ToListAsync();

            var query = _context.VentaDetalles
                .Include(d => d.Producto)
                .ThenInclude(p => p.Categoria)
                .Include(d => d.Venta)
                .AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(d => d.Venta.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(d => d.Venta.Fecha <= fechaFin.Value);

            if (idCategoria.HasValue && idCategoria.Value > 0)
                query = query.Where(d => d.Producto.IdCategoria == idCategoria.Value);

            if (idProducto.HasValue && idProducto.Value > 0)
                query = query.Where(d => d.Producto.IdProducto == idProducto.Value);

            var resultados = await query
                .OrderByDescending(d => d.Venta.Fecha)
                .ToListAsync();

            ViewData["FechaInicio"] = fechaInicio?.ToString("yyyy-MM-dd");
            ViewData["FechaFin"] = fechaFin?.ToString("yyyy-MM-dd");
            ViewData["IdCategoria"] = idCategoria;
            ViewData["IdProducto"] = idProducto;

            return View(resultados);
        }

        // GET: Exportar a Excel
        [HttpGet]
        public async Task<IActionResult> ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idProducto)
        {
            var query = _context.VentaDetalles
                .Include(d => d.Producto)
                .ThenInclude(p => p.Categoria)
                .Include(d => d.Venta)
                .AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(d => d.Venta.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(d => d.Venta.Fecha <= fechaFin.Value);

            if (idCategoria.HasValue && idCategoria.Value > 0)
                query = query.Where(d => d.Producto.IdCategoria == idCategoria.Value);

            if (idProducto.HasValue && idProducto.Value > 0)
                query = query.Where(d => d.Producto.IdProducto == idProducto.Value);

            var data = await query.OrderByDescending(d => d.Venta.Fecha).ToListAsync();

            // 🧾 Crear libro de Excel con ClosedXML
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("ReporteVentas");

            // Título
            ws.Cell("A1").Value = "Reporte de Ventas";
            ws.Range("A1:E1").Merge()
                .Style.Font.SetBold()
                .Font.SetFontSize(14)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Encabezados
            ws.Cell("A3").Value = "Fecha";
            ws.Cell("B3").Value = "Producto";
            ws.Cell("C3").Value = "Categoría";
            ws.Cell("D3").Value = "Cantidad";
            ws.Cell("E3").Value = "Subtotal (S/)";
            ws.Range("A3:E3").Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightGray)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            int row = 4;
            decimal total = 0;

            foreach (var d in data)
            {
                ws.Cell(row, 1).Value = d.Venta.Fecha.ToString("dd/MM/yyyy");
                ws.Cell(row, 2).Value = d.Producto.Nombre;
                ws.Cell(row, 3).Value = d.Producto.Categoria.Nombre;
                ws.Cell(row, 4).Value = d.Cantidad;
                ws.Cell(row, 5).Value = d.SubTotal;
                ws.Cell(row, 5).Style.NumberFormat.Format = "S/ #,##0.00";
                total += d.SubTotal;
                row++;
            }

            // Total general
            ws.Cell(row, 4).Value = "TOTAL:";
            ws.Cell(row, 4).Style.Font.SetBold();
            ws.Cell(row, 5).Value = total;
            ws.Cell(row, 5).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.LightYellow)
                .NumberFormat.SetFormat("S/ #,##0.00");

            // Ajustar ancho de columnas
            ws.Columns().AdjustToContents();

            // Aplicar auto-filtro
            ws.Range("A3:E3").SetAutoFilter();

            // Descargar archivo
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string excelName = $"Reporte_Ventas_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                excelName);
        }
    }
}
