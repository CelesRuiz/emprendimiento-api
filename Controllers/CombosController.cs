using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CombosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Combo>>> GetCombos()
        {
            return await _context.Combos
                .Include(c => c.Opciones)
                .ThenInclude(o => o.Productos)
                .ThenInclude(p => p.Producto)
                .Include(c => c.Opciones)
                .ThenInclude(o => o.ProductoFijo)
                .Where(c => c.EsActivo)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Combo>> PostCombo(CrearComboRequest request)
        {
            var combo = new Combo
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Precio = request.Precio
            };

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCombos), new { id = combo.Id }, combo);
        }

        [HttpPost("opcion")]
        public async Task<IActionResult> AgregarOpcion(AgregarOpcionRequest request)
        {
            var combo = await _context.Combos.FindAsync(request.ComboId);
            if (combo == null)
                return NotFound("Combo no encontrado");

            var opcion = new ComboOpcion
            {
                ComboId = request.ComboId,
                Nombre = request.NombreOpcion,
                EsEleccion = request.EsEleccion,
                ProductoFijoId = request.ProductoFijoId
            };

            _context.ComboOpciones.Add(opcion);
            await _context.SaveChangesAsync();
            return Ok(opcion);
        }

        [HttpPost("opcion/producto")]
        public async Task<IActionResult> AgregarProductoAOpcion(AgregarProductoOpcionRequest request)
        {
            var opcion = await _context.ComboOpciones.FindAsync(request.ComboOpcionId);
            if (opcion == null)
                return NotFound("Opción no encontrada");

            var comboOpcionProducto = new ComboOpcionProducto
            {
                ComboOpcionId = request.ComboOpcionId,
                ProductoId = request.ProductoId
            };

            _context.ComboOpcionProductos.Add(comboOpcionProducto);
            await _context.SaveChangesAsync();
            return Ok(comboOpcionProducto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
                return NotFound("Combo no encontrado");

            combo.EsActivo = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}