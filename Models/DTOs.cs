namespace EmprendimientoApi.Models
{

    public record VentaRequest(int ProductoId, int Cantidad);


    public record ProductoStockResponse(
        int ProductoId,
        string Nombre,
        int StockMinimo,
        int StockActual,
        bool EsStockBajo
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

    // --- PEDIDO EXTERNO (n8n) ---

    public record PedidoExternoRequest(
        string Origen,
        string IdExterno,
        List<ItemExternoRequest> Items
    );

    public record ItemExternoRequest(
        string NombreProducto,
        int Cantidad,
        decimal PrecioUnitario
    );
    // --- PEDIDOS YA ---
    public record PedidosYaOrdenRequest(
        string OrderId,
        List<PedidosYaItemRequest> Items
    );

    public record PedidosYaItemRequest(
        string Name,
        int Quantity,
        decimal Price
    );

    public record PedidosYaEstadoRequest(
        string OrderId,
        string Status
    );

    // --- MERCADO LIBRE ---
    public record MercadoLibreOrdenRequest(
        long OrderId,
        List<MercadoLibreItemRequest> Items
    );

    public record MercadoLibreItemRequest(
        string Title,
        int Quantity,
        decimal UnitPrice
    );

    public record MercadoLibreEstadoRequest(
        long OrderId,
        string Status
    );
    // --- WHATSAPP ---
    // --- WHATSAPP ---
    public record WhatsAppWebhookRequest(
        List<WhatsAppEntry> Entry
    );

    public record WhatsAppEntry(
        List<WhatsAppChange> Changes
    );

    public record WhatsAppChange(
        WhatsAppValue Value
    );

    public record WhatsAppValue(
        List<WhatsAppMessage>? Messages
    );

    public record WhatsAppMessage(
        string From,
        WhatsAppText? Text
    );

    public record WhatsAppText(
        string Body
    );

}