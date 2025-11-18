using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class CorreosController : Controller
    {
        private readonly IEmailSender _emailSender;

        public CorreosController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        // Endpoint para enviar correos por una categoría específica
        [HttpPost("enviar-por-categoria/{categoria}")]
        public async Task<IActionResult> EnviarCorreo([FromBody] List<string> _toEmails, string _category, [FromBody] Dictionary<string, string> dynamicValues)
        {
            // Validar parámetros de entrada
            if (_toEmails == null || !_toEmails.Any())
            {
                return BadRequest(new { mensaje = "Debe proporcionar al menos un destinatario" });
            }

            if (string.IsNullOrWhiteSpace(_category))
            {
                return BadRequest(new { mensaje = "Debe proporcionar una categoría" });
            }

            try
            {
                _emailSender.SendEmail(_toEmails, _category, dynamicValues);
                return Ok(new { mensaje = "Correos enviados exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
