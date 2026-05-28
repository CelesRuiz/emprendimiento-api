using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/mercadolibre")]
    public class MercadoLibreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MercadoLibreController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("orden")]
        public async Task<IActionResult> RecibirOrden([FromBody] MercadoLibreOrdenRequest request)
        {
            // Verificar que no sea un duplicado
            var existe = await _context.Pedidos
                .AnyAsync(p => p.Idexterno == request.OrderId.ToString() && p.OrigenPedido == "MercadoLibre");

            if (existe)
                return Ok(new { message = "Order already processed" });

            var pedido = new Pedido
            {
                FechaPedido = DateTime.UtcNow,
                OrigenPedido = "MercadoLibre",
                Idexterno = request.OrderId.ToString(),
                Estado = EstadoPedido.Pendiente
            };

            foreach (var item in request.Items)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Nombre.ToLower() == item.Title.ToLower());

                if (producto != null)
                {
                    pedido.Items.Add(new PedidoItem
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Quantity,
                        PrecioUnitario = item.UnitPrice
                    });
                }
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order received", pedidoId = pedido.Id });
        }

        [HttpPost("estado")]
        public async Task<IActionResult> ActualizarEstado([FromBody] MercadoLibreEstadoRequest request)
        {
            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(p => p.Idexterno == request.OrderId.ToString() && p.OrigenPedido == "MercadoLibre");

            if (pedido == null)
                return NotFound();

            pedido.Estado = request.Status switch
            {
                "paid" => EstadoPedido.Confirmado,
                "cancelled" => EstadoPedido.Cancelado,
                _ => pedido.Estado
            };

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}