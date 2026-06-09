namespace EmprendimientoApi.Models
{
    public static class MensajeErrorHelper
    {
        public static string ObtenerMensaje(MensajeError error) => error switch
        {
            MensajeError.ProductoNoEncontrado => "El producto no fue encontrado.",
            MensajeError.InsumoNoEncontrado => "El insumo no fue encontrado.",
            MensajeError.ProveedorNoEncontrado => "El proveedor no fue encontrado.",
            MensajeError.UsuarioNoEncontrado => "El usuario no fue encontrado.",
            MensajeError.RolNoEncontrado => "El rol no fue encontrado.",
            MensajeError.EmailYaRegistrado => "El email ya está registrado.",
            MensajeError.CredencialesInvalidas => "Email o contraseña incorrectos.",
            MensajeError.StockInsuficiente => "Stock insuficiente para realizar la operación.",
            MensajeError.IdNoCoincide => "El ID no coincide con el recurso.",
            MensajeError.MovimientoNoEncontrado => "Movimiento no encontrado.",
            MensajeError.NoSePuedeAnularAnulacion => "Este movimiento es una anulación, no se puede anular nuevamente.",
            _ => "Ocurrió un error inesperado."
        };
    }
}