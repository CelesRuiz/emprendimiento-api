using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insumo>>> GetInsumos([FromQuery] string? nombre)
        {
            var query = _context.Insumos.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(i => i.Nombre.Contains(nombre));

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Insumo>> GetInsumo(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.InsumoNoEncontrado));
            return insumo;
        }

        [HttpPost]
        public async Task<ActionResult<Insumo>> PostInsumo(Insumo insumo)
        {
            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInsumo), new { id = insumo.Id }, insumo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsumo(int id, Insumo insumo)
        {
            if (id != insumo.Id)
                return BadRequest(MensajeErrorHelper.ObtenerMensaje(MensajeError.IdNoCoincide));

            _context.Entry(insumo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Efecto inflación en cascada
            var productosAfectados = await _context.ProductoInsumos
                .Where(pi => pi.InsumoId == id)
                .Select(pi => pi.ProductoId)
                .Distinct()
                .ToListAsync();

            foreach (var productoId in productosAfectados)
            {
                var insumos = await _context.ProductoInsumos
                    .Where(pi => pi.ProductoId == productoId)
                    .Include(pi => pi.Insumo)
                    .ToListAsync();

                var nuevoCosto = insumos.Sum(pi => pi.Cantidad * pi.Insumo!.PrecioPorUnidad);

                var producto = await _context.Productos.FindAsync(productoId);
                if (producto != null)
                {
                    producto.CostoProduccion = nuevoCosto;
                    _context.Entry(producto).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.InsumoNoEncontrado));

            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}