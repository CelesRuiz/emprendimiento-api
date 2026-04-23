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
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.Producto)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            var producto = await _context.Productos.FindAsync(venta.ProductoId);
            if (producto == null)
                return NotFound("Producto no encontrado");

            if (producto.StockActual < venta.Cantidad)
                return BadRequest("Stock insuficiente");

            producto.StockActual -= venta.Cantidad;
            venta.PrecioTotal = venta.Cantidad * producto.PrecioVenta;

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVentas), new { id = venta.Id }, venta);
        }
    }
}