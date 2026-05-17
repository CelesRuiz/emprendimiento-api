using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
public async Task<ActionResult<IEnumerable<Venta>>> GetVentas(
    [FromQuery] int? productoId,
    [FromQuery] DateTime? desde,
    [FromQuery] DateTime? hasta)
{
    var query = _context.Ventas
        .Include(v => v.Producto)
        .AsQueryable();

    if (productoId.HasValue)
        query = query.Where(v => v.ProductoId == productoId);

    if (desde.HasValue)
        query = query.Where(v => v.Fecha >= desde);

    if (hasta.HasValue)
        query = query.Where(v => v.Fecha <= hasta);

    return await query.OrderByDescending(v => v.Fecha).ToListAsync();
}
        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            var producto = await _context.Productos.FindAsync(venta.ProductoId);
            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));


            var stockActual = await _context.MovimientosStock
                .Where(m => m.ProductoId == venta.ProductoId)
                .SumAsync(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad);

            if (stockActual < venta.Cantidad)
                return BadRequest(MensajeErrorHelper.ObtenerMensaje(MensajeError.StockInsuficiente));

            venta.PrecioTotal = venta.Cantidad * producto.PrecioVenta;
            venta.Fecha = DateTime.Now;
            _context.Ventas.Add(venta);

            var movimiento = new MovimientoStock
            {
                ProductoId = venta.ProductoId,
                Tipo = TipoMovimiento.Salida,
                Cantidad = venta.Cantidad,
                Fecha = DateTime.Now
            };
            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVentas), new { id = venta.Id }, venta);
        }
    }
}