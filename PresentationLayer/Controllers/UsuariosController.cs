using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System.Net;
using System.Security.Claims;
using System.Net.Http;
using BusinessLayer.Concrete;
using Newtonsoft.Json.Linq;
using DataAccessLayer.EntityFramework;
using Microsoft.Extensions.Logging;
namespace PresentationLayer.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IEmailSender _emailSender;
        private readonly IToolsService _toolService;
        private readonly IBriefService _briefService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IAuthService authService, IUsuarioService usuarioService, IRolService rolService,
                                  IEmailSender emailSender, IToolsService toolService, IBriefService briefService,
                                  ILogger<UsuariosController> logger)
        {
            _authService = authService;
            _usuarioService = usuarioService;
            _rolService = rolService;
            _emailSender = emailSender;
            _toolService = toolService;
            _briefService = briefService;
            _logger = logger;
        }
        // GET: UsuariosController
        public ActionResult Index()
        {
            IEnumerable<Menu> menus = null;
       
            if (User?.Identity?.IsAuthenticated == true)
            {
                ViewBag.RolId = Int32.Parse(User.FindFirst(ClaimTypes.Role)?.Value);
                ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                ViewBag.Menus = _authService.GetMenusByRole(ViewBag.RolId);
                ViewBag.ConteoAlertas = _toolService.GetUnreadAlertsCount(ViewBag.UsuarioId);
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            respuestaServicio res = new respuestaServicio();
            var usuarios = _usuarioService.TGetAll()
                .Select(q => new Usuario
                {
                    Id = q.Id,
                    Nombre = q.Nombre,
                    ApellidoMaterno = q.ApellidoMaterno,
                    ApellidoPaterno = q.ApellidoPaterno,
                    Correo = q.Correo,
                    Contrasena = q.Contrasena,
                    Estatus = q.Estatus,
                    RolId = q.RolId,
                    UserRol = _rolService.TGetById(q.RolId)
                })
                .ToList();

            res.Datos = usuarios;
            res.Exito = true;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("This is an example response", System.Text.Encoding.UTF8, "text/plain")
            };
            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            respuestaServicio res = new respuestaServicio();
            var roles = _rolService.TGetAll();
            res.Datos = roles;
            res.Exito = true;
        

            return Ok(res);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            respuestaServicio res = new respuestaServicio();
            var usuario = _usuarioService.TGetById(id);
            res.Datos = usuario;
            res.Exito = true;
            return Ok(res);
        }

        [HttpPost]
        public ActionResult Create([FromBody] Usuario usuario)
        {
            respuestaServicio res = new respuestaServicio();


            usuario.UserRol = _rolService.TGetById(usuario.RolId);
            usuario.Estatus = true; // Usuario activo por defecto
            _usuarioService.TInsert(usuario);
            
            var urlBase = $"{Request.Scheme}://{Request.Host}";
            
            var valoresDinamicos = new Dictionary<string, string>
            {
                { "usuario", usuario.Correo },
                { "password", usuario.Contrasena },
                { "link", urlBase + "/Login" }
            };
            var Destinatarios = new List<string>();
            Destinatarios.Add(usuario.Correo);

            _emailSender.SendEmail(Destinatarios, "RegistroUsuarioAdmin", valoresDinamicos);
            res.Mensaje = "Usuario agregado exitosamente";
            res.Exito = true;
            return Ok(res);
        }

        [HttpPut]
        public ActionResult Edit([FromBody] Usuario usuario)
        {
            respuestaServicio res = new respuestaServicio();
         
            _usuarioService.TUpdate(usuario);
            res.Datos = usuario;
            res.Mensaje = "Usuario Actualizado exitosamente";
            res.Exito = true;
            return Ok(res);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var usuario = _usuarioService.TGetById(id);
                if (usuario == null)
                {
                    res.Mensaje = "Usuario no encontrado";
                    res.Exito = false;
                    return Ok(res);
                }

                _usuarioService.TDelete(id);
                res.Mensaje = $"Usuario '{usuario.Nombre}' eliminado permanentemente";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = $"Error al eliminar usuario: {ex.Message}";
                res.Exito = false;
            }
            return Ok(res);
        }

        [HttpPost]
        public IActionResult SolicitudUsuario(string Nombre, string Correo,string ApellidoPaterno, string ApellidoMaterno, string Contrasena)
        {
            var urlBase = $"{Request.Scheme}://{Request.Host}";

            Usuario usuario = new()
            {
                Correo = Correo,
                Nombre = Nombre,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Contrasena = Contrasena,
                RolId = 2,
                Estatus = false, // Usuario inactivo hasta aprobación del administrador
                SolicitudRegistro = true, // Marcar como solicitud pendiente
                FechaRegistro = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            var resp =_authService.SolicitudUsuario(usuario);

            if (!resp.Exito)
            {
                TempData["Mensaje"] = resp.Mensaje;
                return RedirectToAction("Registro", "Login");
            }
            else
            {
                // Diccionario con los valores dinámicos a reemplazar
                var valoresDinamicos = new Dictionary<string, string>()
                {
                    { "nombre", usuario.Nombre },
                    { "link", urlBase + "/Login" }
                };
                List<string> Destinatarios = new List<string>();
                Destinatarios.Add(Correo);

                _emailSender.SendEmail(Destinatarios, "RegistroUsuario", valoresDinamicos);

                var valoresDinamicosAdmin = new Dictionary<string, string>()
                {
                    { "nombre", usuario.Nombre + " " + usuario.ApellidoPaterno },
                    { "correo", usuario.Correo },
                    { "link", urlBase + "/Invitaciones" }
                };

                var usuarioAdmin = _authService.ObtenerUsuarioByRol(1);

                Alerta alerta = new()
                {
                    IdUsuario = usuarioAdmin.Id,
                    Nombre = "Solicitud Usuario Nuevo",
                    Descripcion = "Se creo una solicitud para el usuario " + Nombre,
                    FechaCreacion = DateTime.Now,
                    Accion = urlBase + "/Invitaciones",
                    IdTipoAlerta = 2,
                    lectura = false
                };
                _toolService.CrearAlerta(alerta);

                List<string> DestinatariosAdmin = new List<string>();
                DestinatariosAdmin.Add(usuarioAdmin.Correo);

                _emailSender.SendEmail(DestinatariosAdmin, "RegistroUsuarioAdmin", valoresDinamicosAdmin);

                TempData["Mensaje"] = "Tu solicitud fue recibida, espera la confirmación del administrador para que tengas acceso a la plataforma";
                return RedirectToAction("Index", "Login");
            }
           
        }

        [HttpGet]
        public IActionResult ObtenerSolicitudesUsuario()
        {
            respuestaServicio res = new respuestaServicio();
            var usuarios = _toolService.GetUsuarioBySolicitud();
            res.Datos = usuarios;
            res.Exito = true;


            return Ok(res);
        }

        [HttpPost]
        public IActionResult CambioContrasena(int id, string Contrasena)
        {

            respuestaServicio resp = new respuestaServicio();
          
            Usuario usuario = _usuarioService.TGetById(id);
            usuario.Contrasena = Contrasena;
            usuario.CambioContrasena = false;

            _usuarioService.TUpdate(usuario);

            TempData["MensajeExito"] = "La contraseña ha sido actualizada exitosamente.";
            return RedirectToAction("CambioContrasena", "Login");
            
        }
        [HttpPost]
        public ActionResult BuscarUsuario([FromBody] Usuario usuario)
        {
            respuestaServicio res = new respuestaServicio();
            var usuarios = _toolService.BuscarUsuario(usuario.Nombre.ToUpper(),2);
            res.Datos = usuarios;
            res.Exito = true;
            return Ok(res);
        }
        [HttpPost]
        public ActionResult BuscarAllUsuarios([FromBody] Usuario usuario)
        {
            respuestaServicio res = new respuestaServicio();
            var usuarios = _toolService.BuscarUsuario(usuario.Nombre.ToUpper(),0);
            res.Datos = usuarios;
            res.Exito = true;
            return Ok(res);
        }
        // GET: BriefController/ObtenerParticipantes/5
        public ActionResult ObtenerParticipantes(int id)
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                res.Datos = _toolService.ObtenerParticipantes(id);
                res.Mensaje = "Solicitud Exitosa";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Petición fallida";
                res.Exito = false;
            }
            return Ok(res);

        }
        [HttpPost]
        public ActionResult AgregarParticipante([FromBody] Participante participante)
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                var participanteBD = _toolService.AgregarParticipante(participante);
                participante.Usuario = _usuarioService.TGetById(participante.UsuarioId);

                var brief = _briefService.GetById(participante.BriefId);

                var urlBase = $"{Request.Scheme}://{Request.Host}";

                // Crear alerta para el participante agregado
                var alertaParticipante = new Alerta
                {
                    IdUsuario = participante.UsuarioId,
                    Nombre = "Agregado como Participante",
                    Descripcion = $"Has sido agregado como participante en el proyecto '{brief.Nombre}'",
                    IdTipoAlerta = 3, // Tipo: Actualización
                    Accion = $"{urlBase}/Brief/Index?filtroNombre={Uri.EscapeDataString(brief.Nombre)}"
                };
                _toolService.CrearAlerta(alertaParticipante);

                // Diccionario con los valores dinámicos a reemplazar
                var valoresDinamicos = new Dictionary<string, string>()
                {
                    { "nombreProyecto", brief.Nombre },
                    { "link", urlBase + "/Brief/Index?filtroNombre=" + Uri.EscapeDataString(brief.Nombre)  }

                };
                var Destinatarios = new List<string>();
                Destinatarios.Add(participante.Usuario.Correo);

                // Intentar enviar el correo, pero no fallar si hay un error
                try
                {
                    _logger.LogInformation($"Enviando notificación de participante agregado a: {participante.Usuario.Correo} para el proyecto: {brief.Nombre}");
                    _emailSender.SendEmail(Destinatarios, "RegistroParticipante", valoresDinamicos);
                    _logger.LogInformation($"Notificación enviada exitosamente a: {participante.Usuario.Correo}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogError(emailEx, $"Error al enviar correo de notificación a {participante.Usuario.Correo}: {emailEx.Message}");
                    // Continuar, el participante se agregó correctamente aunque falló el correo
                }

                res.Datos = participanteBD;
                res.Mensaje = "Creado exitosamente";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar participante: {ex.Message}");
                res.Mensaje = "Error al Crear el Participante: " + ex.Message;
                res.Exito = false;
            }

            return Ok(res);
        }

    }
}
