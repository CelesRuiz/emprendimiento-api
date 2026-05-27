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
                    false // Calculamos el booleano abajo por limitaciones de LINQ sum
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
    }
}