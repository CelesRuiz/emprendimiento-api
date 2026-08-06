using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AjustesInventarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AjustesInventarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarInventario([FromBody] ConfirmarInventarioRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var ajuste in request.Ajustes)
                {
                    if (ajuste.Tipo == "Insumo" && ajuste.InsumoId.HasValue)
                    {
                        await ProcesarAjusteInsumo(ajuste, request.Motivo);
                    }
                    else if (ajuste.Tipo == "Producto" && ajuste.ProductoId.HasValue)
                    {
                        await ProcesarAjusteProducto(ajuste, request.Motivo);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Inventario confirmado correctamente" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Error al confirmar inventario: {ex.Message}");
            }
        }

        private async Task ProcesarAjusteInsumo(AjusteInventarioItem ajuste, string motivo)
        {
            var insumo = await _context.Insumos.FindAsync(ajuste.InsumoId!.Value);
            if (insumo == null)
                throw new Exception($"Insumo {ajuste.InsumoId} no encontrado");

            var diferenciaDinero = ajuste.CantidadAjuste * insumo.PrecioPorUnidad;

            _context.AjustesStock.Add(new AjusteStock
            {
                InsumoId = ajuste.InsumoId,
                CantidadAjuste = ajuste.CantidadAjuste,
                DiferenciaDinero = diferenciaDinero,
                Fecha = DateTime.UtcNow,
                Motivo = motivo
            });

            if (ajuste.CantidadAjuste < 0)
            {
                // Falta stock: descontar de lotes vía PEPS
                var cantidadAFaltar = Math.Abs(ajuste.CantidadAjuste);
                var lotes = await _context.Lotes
                    .Where(l => l.InsumoId == ajuste.InsumoId && !l.Cerrado && l.CantidadActual > 0)
                    .OrderBy(l => l.FechaVencimiento)
                    .ToListAsync();

                foreach (var lote in lotes)
                {
                    if (cantidadAFaltar <= 0) break;

                    if (lote.CantidadActual >= cantidadAFaltar)
                    {
                        lote.CantidadActual -= cantidadAFaltar;
                        cantidadAFaltar = 0;
                    }
                    else
                    {
                        cantidadAFaltar -= lote.CantidadActual;
                        lote.CantidadActual = 0;
                        lote.Cerrado = true;
                        lote.MotivoCierre = "AjusteInventario";
                    }
                }
            }
            else if (ajuste.CantidadAjuste > 0)
            {
                // Sobra stock: crear lote nuevo
                _context.Lotes.Add(new Lote
                {
                    InsumoId = ajuste.InsumoId!.Value,
                    CantidadInicial = ajuste.CantidadAjuste,
                    CantidadActual = ajuste.CantidadAjuste,
                    FechaIngreso = DateTime.UtcNow,
                    FechaVencimiento = ajuste.FechaVencimiento ?? DateTime.MaxValue,
                    Cerrado = false
                });
            }
        }

        private async Task ProcesarAjusteProducto(AjusteInventarioItem ajuste, string motivo)
        {
            var producto = await _context.Productos.FindAsync(ajuste.ProductoId!.Value);
            if (producto == null)
                throw new Exception($"Producto {ajuste.ProductoId} no encontrado");

            var diferenciaDinero = ajuste.CantidadAjuste * producto.CostoProduccion;

            _context.AjustesStock.Add(new AjusteStock
            {
                ProductoId = ajuste.ProductoId,
                CantidadAjuste = ajuste.CantidadAjuste,
                DiferenciaDinero = diferenciaDinero,
                Fecha = DateTime.UtcNow,
                Motivo = motivo
            });

            if (ajuste.CantidadAjuste < 0)
            {
                // Falta stock: crear movimiento de salida
                _context.MovimientosStock.Add(new MovimientoStock
                {
                    ProductoId = ajuste.ProductoId!.Value,
                    Tipo = TipoMovimiento.Salida,
                    Cantidad = (int)Math.Abs(ajuste.CantidadAjuste),
                    Fecha = DateTime.UtcNow,
                    MotivoSalida = MotivoSalida.AjusteInventario
                });
            }
            else if (ajuste.CantidadAjuste > 0)
            {
                // Sobra stock: crear movimiento de entrada
                _context.MovimientosStock.Add(new MovimientoStock
                {
                    ProductoId = ajuste.ProductoId!.Value,
                    Tipo = TipoMovimiento.Entrada,
                    Cantidad = (int)ajuste.CantidadAjuste,
                    CantidadActual = ajuste.CantidadAjuste,
                    Fecha = DateTime.UtcNow,
                    FechaVencimiento = ajuste.FechaVencimiento
                });
            }
        }
    }
}