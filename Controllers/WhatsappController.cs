using Microsoft.AspNetCore.Mvc;
using EmprendimientoApi.Data;
using EmprendimientoApi.Models;

namespace EmprendimientoApi.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WhatsAppController(AppDbContext context)
        {
            _context = context;
        }

        // Verificación de webhook requerida por Meta
        [HttpGet("webhook")]
        public IActionResult VerificarWebhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.verify_token")] string token,
            [FromQuery(Name = "hub.challenge")] string challenge)
        {
            var verifyToken = "brumarest_whatsapp_token";

            if (mode == "subscribe" && token == verifyToken)
                return Ok(int.Parse(challenge));

            return Unauthorized();
        }

        // Recibe mensajes — n8n se encarga del procesamiento con IA
        [HttpPost("webhook")]
        public IActionResult RecibirMensaje([FromBody] WhatsAppWebhookRequest request)
        {
            // n8n recibe este webhook, procesa con IA y llama a los endpoints necesarios
            return Ok();
        }
    }
}