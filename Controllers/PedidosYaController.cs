using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/pedidosya")]
    public class PedidosYaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosYaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("orden")]
        public async Task<IActionResult> RecibirOrden([FromBody] PedidosYaOrdenRequest request)
        {
            var pedido = new Pedido
            {
                FechaPedido = DateTime.UtcNow,
                OrigenPedido = "PedidosYa",
                Idexterno = request.OrderId,
                Estado = EstadoPedido.Pendiente
            };

            foreach (var item in request.Items)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Nombre.ToLower() == item.Name.ToLower());

                if (producto != null)
                {
                    pedido.Items.Add(new PedidoItem
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Quantity,
                        PrecioUnitario = item.Price
                    });
                }
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order received", pedidoId = pedido.Id });
        }

        [HttpPost("estado")]
        public async Task<IActionResult> ActualizarEstado([FromBody] PedidosYaEstadoRequest request)
        {
            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(p => p.Idexterno == request.OrderId && p.OrigenPedido == "PedidosYa");

            if (pedido == null)
                return NotFound();

            if (request.Status == "order_cancelled")
                pedido.Estado = EstadoPedido.Cancelado;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}