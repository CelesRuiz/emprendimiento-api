using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduccionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProduccionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarProduccion([FromBody] ProduccionRequest request)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductoInsumos)
                .ThenInclude(pi => pi.Insumo)
                .FirstOrDefaultAsync(p => p.Id == request.ProductoId);

            if (producto == null)
                return NotFound(MensajeErrorHelper.ObtenerMensaje(MensajeError.ProductoNoEncontrado));

            // Para cada insumo de la receta, descontar usando PEPS
            foreach (var productoInsumo in producto.ProductoInsumos)
            {
                var cantidadNecesaria = productoInsumo.Cantidad * request.Cantidad;

                // Traer lotes disponibles ordenados por fecha de vencimiento (PEPS)
                var lotes = await _context.Lotes
                    .Where(l => l.InsumoId == productoInsumo.InsumoId
                             && l.CantidadActual > 0
                             && !l.Cerrado)
                    .OrderBy(l => l.FechaVencimiento)
                    .ToListAsync();

                var totalDisponible = lotes.Sum(l => l.CantidadActual);
                if (totalDisponible < cantidadNecesaria)
                    return BadRequest($"Stock insuficiente de {productoInsumo.Insumo!.Nombre}. Necesario: {cantidadNecesaria}, Disponible: {totalDisponible}");

                // Descontar de los lotes uno por uno (PEPS)
                foreach (var lote in lotes)
                {
                    if (cantidadNecesaria <= 0) break;

                    if (lote.CantidadActual >= cantidadNecesaria)
                    {
                        lote.CantidadActual -= cantidadNecesaria;
                        cantidadNecesaria = 0;
                    }
                    else
                    {
                        cantidadNecesaria -= lote.CantidadActual;
                        lote.CantidadActual = 0;
                        lote.Cerrado = true;
                        lote.MotivoCierre = "Agotado";
                    }
                }
            }

            // Registrar el movimiento de entrada del producto terminado
            var movimiento = new MovimientoStock
            {
                ProductoId = request.ProductoId,
                Tipo = TipoMovimiento.Entrada,
                Cantidad = request.Cantidad,
                Fecha = DateTime.UtcNow
            };

            _context.MovimientosStock.Add(movimiento);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producción registrada correctamente", movimientoId = movimiento.Id });
        }
    }
}