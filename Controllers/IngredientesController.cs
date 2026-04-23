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
        public async Task<ActionResult<IEnumerable<Ingrediente>>> GetIngredientes()
        {
            return await _context.Ingredientes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Ingrediente>> PostIngrediente(Ingrediente ingrediente)
        {
            _context.Ingredientes.Add(ingrediente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIngredientes), new { id = ingrediente.Id }, ingrediente);
        }

        [HttpPut("{id}")]
public async Task<IActionResult> PutIngrediente(int id, Ingrediente ingrediente)
{
    if (id != ingrediente.Id)
        return BadRequest();

    _context.Entry(ingrediente).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteIngrediente(int id)
{
    var ingrediente = await _context.Ingredientes.FindAsync(id);
    if (ingrediente == null)
        return NotFound();

    _context.Ingredientes.Remove(ingrediente);
    await _context.SaveChangesAsync();
    return NoContent();
}
    }
    
}