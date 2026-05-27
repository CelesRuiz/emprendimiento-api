using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lote>>> GetLotes([FromQuery] int? insumoId)
        {
            var query = _context.Lotes
                .Include(l => l.Insumo)
                .Where(l => l.CantidadActual > 0)
                .OrderBy(l => l.FechaVencimiento)
                .AsQueryable();

            if (insumoId.HasValue)
                query = query.Where(l => l.InsumoId == insumoId);

            return await query.ToListAsync();
        }

        [HttpGet("por-vencer")]
        public async Task<ActionResult<IEnumerable<Lote>>> GetLotesPorVencer([FromQuery] int dias = 7)
        {
            var fechaLimite = DateTime.UtcNow.AddDays(dias);

            var lotes = await _context.Lotes
                .Include(l => l.Insumo)
                .Where(l => l.CantidadActual > 0 && l.FechaVencimiento <= fechaLimite)
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();

            return Ok(lotes);
        }

        [HttpPost]
        public async Task<ActionResult<Lote>> PostLote(Lote lote)
        {
            lote.FechaIngreso = DateTime.UtcNow;
            lote.CantidadActual = lote.CantidadInicial;
            _context.Lotes.Add(lote);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLotes), new { id = lote.Id }, lote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLote(int id)
        {
            var lote = await _context.Lotes.FindAsync(id);
            if (lote == null)
                return NotFound();

            _context.Lotes.Remove(lote);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}