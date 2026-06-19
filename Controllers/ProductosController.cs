using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
    [FromQuery] string? nombre,
    [FromQuery] bool? stockBajo)
        {
            var query = _context.Productos
                .Include(p => p.ProductoInsumos)
                .ThenInclude(pi => pi.Insumo)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (stockBajo == true)
            {
                query = query.Where(p => _context.MovimientosStock
                    .Where(m => m.ProductoId == p.Id)
                    .Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad) < p.StockMinimo);
            }

            return await query.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductoInsumos)
                .ThenInclude(pi => pi.Insumo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));

            return producto;
        }

        [HttpGet("stock")]
        public async Task<ActionResult<IEnumerable<ProductoStockResponse>>> GetStockProductos()
        {
            var stocks = await _context.Productos
                .Select(p => new ProductoStockResponse(
                    p.Id,
                    p.Nombre,
                    p.StockMinimo,
                    _context.MovimientosStock
                        .Where(m => m.ProductoId == p.Id)
                        .Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad),
                    false,
                    p.DiasMaxFrescura,
                    _context.MovimientosStock
                        .Where(m => m.ProductoId == p.Id && m.Tipo == TipoMovimiento.Entrada)
                        .OrderByDescending(m => m.Fecha)
                        .Select(m => (DateTime?)m.Fecha)
                        .FirstOrDefault()
                ))
                .ToListAsync();

            var resultado = stocks.Select(s => s with { EsStockBajo = s.StockActual < s.StockMinimo });

            return Ok(resultado);
        }
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest(MensajeErrorHelper.ObtenerMensaje(MensajeError.IdNoCoincide));

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("{id}/lotes")]
        public async Task<ActionResult<IEnumerable<LoteProductoResponse>>> GetLotesProducto(int id, [FromQuery] bool historial = false)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));

            // IDs de movimientos anulados
            var idsAnulados = await _context.MovimientosStock
                .Where(m => m.MovimientoAnuladoId != null)
                .Select(m => m.MovimientoAnuladoId!.Value)
                .ToListAsync();

            // Lotes = movimientos de Entrada
            var query = _context.MovimientosStock
                .Where(m => m.ProductoId == id && m.Tipo == TipoMovimiento.Entrada)
                .AsQueryable();

            // Si NO es historial, solo los activos (cantidad > 0 y no anulados)
            if (!historial)
            {
                query = query.Where(m => m.CantidadActual > 0 && !idsAnulados.Contains(m.Id));
            }

            var lotes = await query.OrderBy(m => m.Fecha).ToListAsync();

            var hoy = DateTime.UtcNow;
            var resultado = lotes.Select(m =>
            {
                string estado;
                if (m.FechaVencimiento == null)
                    estado = "Activo";
                else if (m.FechaVencimiento < hoy)
                    estado = "Vencido";
                else if ((m.FechaVencimiento.Value - hoy).TotalDays <= 2)
                    estado = "PorVencer";
                else
                    estado = "Activo";

                return new LoteProductoResponse(
                    m.Id,
                    m.Fecha,
                    m.Cantidad,
                    m.CantidadActual,
                    m.FechaVencimiento,
                    estado,
                    idsAnulados.Contains(m.Id)
                );
            });

            return Ok(resultado);
        }
    }

}