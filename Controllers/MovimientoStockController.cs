using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimientosStockController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoStock>>> GetMovimientos(
    [FromQuery] TipoMovimiento? tipo,
    [FromQuery] DateTime? desde,
    [FromQuery] DateTime? hasta)
        {
            var query = _context.MovimientosStock
                .Include(m => m.Producto)
                .AsQueryable();

            if (tipo.HasValue)
                query = query.Where(m => m.Tipo == tipo);

            if (desde.HasValue)
                query = query.Where(m => m.Fecha >= desde);

            if (hasta.HasValue)
                query = query.Where(m => m.Fecha <= hasta);

            return await query.OrderByDescending(m => m.Fecha).ToListAsync();
        }
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<MovimientoStock>>> GetMovimientosPorProducto(int productoId)
        {
            return await _context.MovimientosStock
                .Where(m => m.ProductoId == productoId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

        [HttpGet("stock/{productoId}")]
        public async Task<ActionResult<int>> GetStockActual(int productoId)
        {
            var stock = await _context.MovimientosStock
                .Where(m => m.ProductoId == productoId)
                .SumAsync(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad);

            return Ok(stock);
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoStock>> PostMovimiento(MovimientoStock movimiento)
        {
            _context.MovimientosStock.Add(movimiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovimientos), new { id = movimiento.Id }, movimiento);

        }
        [HttpGet("perdidas")]
        public async Task<ActionResult<IEnumerable<PerdidaResponse>>> GetPerdidas(
    [FromQuery] DateTime? desde,
    [FromQuery] DateTime? hasta)
        {
            var query = _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.MotivoSalida == MotivoSalida.Vencimiento ||
                            m.MotivoSalida == MotivoSalida.Descarte)
                .AsQueryable();

            if (desde.HasValue)
                query = query.Where(m => m.Fecha >= desde);

            if (hasta.HasValue)
                query = query.Where(m => m.Fecha <= hasta);

            var perdidas = await query
                .OrderByDescending(m => m.Fecha)
                .Select(m => new PerdidaResponse(
                    m.Producto!.Nombre,
                    m.Cantidad,
                    m.Producto.PrecioVenta,
                    m.Cantidad * m.Producto.PrecioVenta,
                    m.MotivoSalida!.Value,
                    m.Fecha
                ))
                .ToListAsync();

            return Ok(perdidas);
        }
    }



}