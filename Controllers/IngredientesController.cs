using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingrediente>>> GetIngredientes([FromQuery] string? nombre)
{
    var query = _context.Ingredientes.AsQueryable();

    if (!string.IsNullOrEmpty(nombre))
        query = query.Where(i => i.Nombre.Contains(nombre));

    return await query.ToListAsync();
}

[HttpGet("{id}")]
public async Task<ActionResult<Ingrediente>> GetIngrediente(int id)
{
    var ingrediente = await _context.Ingredientes.FindAsync(id);
    if (ingrediente == null)
        return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.IngredienteNoEncontrado));
    return ingrediente;
}
[HttpPut("{id}")]
public async Task<IActionResult> PutIngrediente(int id, Ingrediente ingrediente)
{
   if (id != ingrediente.Id)
    return BadRequest(MensajeErrorHelper.ObtenerMensaje(MensajeError.IdNoCoincide));

    _context.Entry(ingrediente).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteIngrediente(int id)
{
    var ingrediente = await _context.Ingredientes.FindAsync(id);
    if (ingrediente == null)
        return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.IngredienteNoEncontrado));

    _context.Ingredientes.Remove(ingrediente);
    await _context.SaveChangesAsync();
    return NoContent();
}
    }
    
}