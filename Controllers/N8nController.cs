using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/n8n")]
    public class N8nController : ControllerBase
    {
        private readonly AppDbContext _context;

        public N8nController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            var stock = await _context.Productos
                .Select(p => new
                {
                    p.Nombre,
                    StockActual = _context.MovimientosStock
                        .Where(m => m.ProductoId == p.Id)
                        .Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad),
                    p.StockMinimo
                })
                .ToListAsync();

            return Ok(stock);
        }

        [HttpGet("vencimientos")]
        public async Task<IActionResult> GetVencimientos([FromQuery] int dias = 7)
        {
            var fechaLimite = DateTime.UtcNow.AddDays(dias);

            var lotes = await _context.Lotes
                .Include(l => l.Insumo)
                .Where(l => l.CantidadActual > 0 && !l.Cerrado && l.FechaVencimiento <= fechaLimite)
                .OrderBy(l => l.FechaVencimiento)
                .Select(l => new
                {
                    Insumo = l.Insumo!.Nombre,
                    l.CantidadActual,
                    l.FechaVencimiento
                })
                .ToListAsync();

            return Ok(lotes);
        }

        [HttpGet("perdidas")]
        public async Task<IActionResult> GetPerdidas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            var query = _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.MotivoSalida == MotivoSalida.Vencimiento ||
                            m.MotivoSalida == MotivoSalida.Descarte)
                .AsQueryable();

            if (desde.HasValue) query = query.Where(m => m.Fecha >= desde);
            if (hasta.HasValue) query = query.Where(m => m.Fecha <= hasta);

            var perdidas = await query
                .Select(m => new
                {
                    Producto = m.Producto!.Nombre,
                    m.Cantidad,
                    m.Producto.PrecioVenta,
                    TotalPerdido = m.Cantidad * m.Producto.PrecioVenta,
                    Motivo = m.MotivoSalida.ToString(),
                    m.Fecha
                })
                .ToListAsync();

            return Ok(perdidas);
        }

        [HttpGet("ventas")]
        public async Task<IActionResult> GetVentas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            var query = _context.Ventas
                .Include(v => v.Producto)
                .AsQueryable();

            if (desde.HasValue) query = query.Where(v => v.Fecha >= desde);
            if (hasta.HasValue) query = query.Where(v => v.Fecha <= hasta);

            var ventas = await query
                .Select(v => new
                {
                    Producto = v.Producto!.Nombre,
                    v.Cantidad,
                    v.PrecioTotal,
                    v.Fecha
                })
                .ToListAsync();

            return Ok(ventas);
        }

        [HttpGet("costos")]
        public async Task<IActionResult> GetCostos()
        {
            var costos = await _context.Productos
                .Select(p => new
                {
                    p.Nombre,
                    p.CostoProduccion,
                    p.PrecioVenta,
                    Margen = p.PrecioVenta - p.CostoProduccion
                })
                .ToListAsync();

            return Ok(costos);
        }

        [HttpGet("precio-sugerido/{productoId}")]
        public async Task<IActionResult> GetPrecioSugerido(int productoId, [FromQuery] decimal margenDeseado = 0.30m)
        {
            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null)
                return NotFound();

            var precioSugerido = producto.CostoProduccion * (1 + margenDeseado);

            return Ok(new
            {
                producto.Nombre,
                producto.CostoProduccion,
                producto.PrecioVenta,
                PrecioSugerido = precioSugerido,
                MargenActual = producto.PrecioVenta - producto.CostoProduccion
            });
        }

        [HttpGet("insumos")]
        public async Task<IActionResult> GetInsumos()
        {
            var insumos = await _context.Insumos
                .Select(i => new
                {
                    i.Nombre,
                    i.PrecioPorUnidad,
                    i.UnidadMedida,
                    i.StockMinimo
                })
                .ToListAsync();

            return Ok(insumos);
        }

        [HttpGet("receta/{productoId}")]
        public async Task<IActionResult> GetReceta(int productoId)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductoInsumos)
                .ThenInclude(pi => pi.Insumo)
                .FirstOrDefaultAsync(p => p.Id == productoId);

            if (producto == null)
                return NotFound();

            return Ok(new
            {
                producto.Nombre,
                producto.CostoProduccion,
                Insumos = producto.ProductoInsumos.Select(pi => new
                {
                    Insumo = pi.Insumo!.Nombre,
                    pi.Cantidad,
                    pi.Insumo.UnidadMedida,
                    CostoInsumo = pi.Cantidad * pi.Insumo.PrecioPorUnidad
                })
            });
        }
    }
}