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
        .Include(p => p.ProductoIngredientes)
        .ThenInclude(pi => pi.Ingrediente)
        .AsQueryable();

    if (!string.IsNullOrEmpty(nombre))
        query = query.Where(p => p.Nombre.Contains(nombre));

    if (stockBajo == true)
    {
        var stockPorProducto = await _context.MovimientosStock
            .GroupBy(m => m.ProductoId)
            .Select(g => new
            {
                ProductoId = g.Key,
                StockActual = g.Sum(m => m.Tipo == TipoMovimiento.Entrada ? m.Cantidad : -m.Cantidad)
            })
            .ToListAsync();

        var productosConStockBajo = stockPorProducto
            .Where(s => {
                var producto = _context.Productos.Find(s.ProductoId);
                return producto != null && s.StockActual < producto.StockMinimo;
            })
            .Select(s => s.ProductoId)
            .ToList();

        query = query.Where(p => productosConStockBajo.Contains(p.Id));
    }

    return await query.ToListAsync();
}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
{
            var producto = await _context.Productos
                .Include(p => p.ProductoIngredientes)
                .ThenInclude(pi => pi.Ingrediente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));

        return producto;
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