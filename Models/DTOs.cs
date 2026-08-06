namespace EmprendimientoApi.Models
{

    public record VentaRequest(int ProductoId, int Cantidad);


    public record ProductoStockResponse(
       int ProductoId,
       string Nombre,
       int StockMinimo,
       int StockActual,
       bool EsStockBajo,
       int DiasMaxFrescura,
       DateTime? UltimaProduccion,
       decimal CostoProduccion
   );

    public record MovimientoRequest(
        int ProductoId,
        TipoMovimiento Tipo,
        int Cantidad,
        MotivoSalida? MotivoSalida
    );


    public record ProduccionRequest(int ProductoId, int Cantidad);


    public record CrearInsumoRequest(
        string Nombre,
        decimal PrecioPorUnidad,
        UnidadMedida UnidadMedida,
        int StockMinimo
    );


    public record CrearLoteRequest(
        int InsumoId,
        decimal CantidadInicial,
        DateTime FechaVencimiento
    );

    public record CerrarLoteRequest(int LoteId, string MotivoCierre);


    public record PerdidaResponse(
        string NombreProducto,
        int Cantidad,
        decimal PrecioVenta,
        decimal TotalPerdido,
        MotivoSalida Motivo,
        DateTime Fecha
    );

    public record CrearComboRequest(
        string Nombre,
        string Descripcion,
        decimal Precio
    );

    public record AgregarOpcionRequest(
        int ComboId,
        string NombreOpcion,
        bool EsEleccion,
        int? ProductoFijoId
    );

    public record AgregarProductoOpcionRequest(
        int ComboOpcionId,
        int ProductoId
    );

    public record VentaComboRequest(
        int ComboId,
        int Cantidad,
        List<OpcionElegidaRequest> OpcionesElegidas
    );

    public record OpcionElegidaRequest(
        int ComboOpcionId,
        int ProductoElegidoId
    );

    public record CrearPedidoRequest(
        string? OrigenPedido,
        string? IdExterno,
        List<CrearPedidoItemRequest> Items
    );

    public record CrearPedidoItemRequest(
        int? ProductoId,
        int? ComboId,
        int Cantidad,
        decimal PrecioUnitario,
        List<OpcionElegidaRequest>? OpcionesElegidas
    );

    public record ActualizarEstadoPedidoRequest(EstadoPedido NuevoEstado);

    public record MovimientoStockResponse(
       int Id,
       int ProductoId,
       string NombreProducto,
       TipoMovimiento Tipo,
       int Cantidad,
       DateTime Fecha,
       MotivoSalida? MotivoSalida,
       DateTime? FechaVencimiento,
       bool EstaAnulado,
       DateTime CreadoEn,
       DateTime ActualizadoEn
   );

    public record SalidaRequest(
     int ProductoId,
     int Cantidad,
     MotivoSalida MotivoSalida
 );

    public record LoteProductoResponse(
       int Id,
       DateTime Fecha,
       int Cantidad,
       decimal? CantidadActual,
       DateTime? FechaVencimiento,
       string Estado,
       bool EstaAnulado
   );

    public record EventoHistorialResponse(
     DateTime Fecha,
     string TipoEvento,
     int Cantidad,
     bool EstaAnulado
 );

    public record InsumoConStockResponse(
       int Id,
       string Nombre,
       string UnidadMedida,
       decimal PrecioPorUnidad,
       decimal StockActual,
       int StockMinimo
   );
    public record ConfirmarInventarioRequest(
     string Motivo,
     List<AjusteInventarioItem> Ajustes
 );

    public record AjusteInventarioItem(
        string Tipo,
        int? InsumoId,
        int? ProductoId,
        decimal CantidadAjuste,
        DateTime? FechaVencimiento
    );



}







