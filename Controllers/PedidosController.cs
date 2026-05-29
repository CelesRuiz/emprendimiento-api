using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos(
            [FromQuery] EstadoPedido? estado,
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] string? origen)
        {
            var query = _context.Pedidos
                .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
                .Include(p => p.Items)
                .ThenInclude(i => i.Combo)
                .Include(p => p.Items)
                .ThenInclude(i => i.OpcionesElegidas)
                .ThenInclude(o => o.ProductoElegido)
                .AsQueryable();

            if (estado.HasValue)
                query = query.Where(p => p.Estado == estado);

            if (desde.HasValue)
                query = query.Where(p => p.FechaPedido >= desde);

            if (hasta.HasValue)
                query = query.Where(p => p.FechaPedido <= hasta);

            if (!string.IsNullOrEmpty(origen))
                query = query.Where(p => p.OrigenPedido == origen);

            return await query.OrderByDescending(p => p.FechaPedido).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(CrearPedidoRequest request)
        {
            var pedido = new Pedido
            {
                FechaPedido = DateTime.UtcNow,
                OrigenPedido = request.OrigenPedido,
                Idexterno = request.IdExterno,
                Estado = EstadoPedido.Pendiente
            };

            foreach (var itemRequest in request.Items)
            {
                var item = new PedidoItem
                {
                    ProductoId = itemRequest.ProductoId,
                    ComboId = itemRequest.ComboId,
                    Cantidad = itemRequest.Cantidad,
                    PrecioUnitario = itemRequest.PrecioUnitario
                };

                if (itemRequest.OpcionesElegidas != null)
                {
                    foreach (var opcion in itemRequest.OpcionesElegidas)
                    {
                        item.OpcionesElegidas.Add(new PedidoItemOpcion
                        {
                            ComboOpcionId = opcion.ComboOpcionId,
                            ProductoElegidoId = opcion.ProductoElegidoId
                        });
                    }
                }

                pedido.Items.Add(item);
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPedidos), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, ActualizarEstadoPedidoRequest request)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Items)
                .ThenInclude(i => i.OpcionesElegidas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("Pedido no encontrado");

            pedido.Estado = request.NuevoEstado;


            if (request.NuevoEstado == EstadoPedido.Entregado)
            {
                foreach (var item in pedido.Items)
                {
                    if (item.ProductoId.HasValue)
                    {
                        var movimiento = new MovimientoStock
                        {
                            ProductoId = item.ProductoId.Value,
                            Tipo = TipoMovimiento.Salida,
                            Cantidad = item.Cantidad,
                            MotivoSalida = MotivoSalida.Venta,
                            Fecha = DateTime.UtcNow
                        };
                        _context.MovimientosStock.Add(movimiento);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}