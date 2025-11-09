using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class MaterialesController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IBriefService _briefService;
        private readonly IUsuarioService _usuarioService;
        private readonly IToolsService _toolsService;
        private readonly ILogger<MaterialesController> _logger;

        public MaterialesController(IEmailSender emailSender, IAuthService authService, IWebHostEnvironment hostingEnvironment, IBriefService briefService, IUsuarioService usuarioService, IToolsService toolsService, ILogger<MaterialesController> logger)
        {
            _emailSender = emailSender;
            _authService = authService;
            _hostingEnvironment = hostingEnvironment;
            _briefService = briefService;
            _usuarioService = usuarioService;
            _toolsService = toolsService;
            _logger = logger;
        }
        public IActionResult Index(string filtroNombre = null)
        {
            IEnumerable<Menu> menus = null;

            if (User?.Identity?.IsAuthenticated == true)
            {
                ViewBag.RolId = Int32.Parse(User.FindFirst(ClaimTypes.Role)?.Value);
                ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value;
                ViewBag.UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                ViewBag.Menus = _authService.GetMenusByRole(ViewBag.RolId);
                ViewBag.ConteoAlertas = _toolsService.GetUnreadAlertsCount(ViewBag.UsuarioId);
                ViewBag.FiltroNombre = filtroNombre;
            }
            else
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult ObtenerMateriales()
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var materiales = _briefService.GetMaterialesByUser(id);

                // Proyectar a un objeto anónimo para evitar referencias circulares
                var materialesDto = materiales.Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    m.Mensaje,
                    m.Area,
                    m.Responsable,
                    m.FechaEntrega,
                    Brief = new
                    {
                        m.Brief.Id,
                        m.Brief.Nombre,
                        m.Brief.LinksReferencias,
                        m.Brief.RutaArchivo
                    },
                    Formato = new
                    {
                        m.Formato.Id,
                        m.Formato.Descripcion
                    },
                    Audiencia = new
                    {
                        m.Audiencia.Id,
                        m.Audiencia.Descripcion
                    },
                    EstatusMaterial = new
                    {
                        m.EstatusMaterial.Id,
                        m.EstatusMaterial.Descripcion
                    },
                    Prioridad = new
                    {
                        m.Prioridad.Id,
                        m.Prioridad.Descripcion
                    },
                    m.EstatusMaterialId,
                    MaterialPCNs = m.MaterialPCNs.Select(mp => new
                    {
                        mp.MaterialId,
                        mp.PCNId,
                        PCN = new
                        {
                            mp.PCN.Id,
                            mp.PCN.Descripcion
                        }
                    }).ToList()
                }).ToList();

                res.Datos = materialesDto;
                res.Exito = true;
            }
            catch (Exception ex) // Capturamos el error exacto
            {
                res.Mensaje = $"Petición fallida: {ex.Message}";
                res.Exito = false;

                // Registrar detalles del error para la depuración
                Console.WriteLine($"Error en ObtenerMateriales: {ex}");
            }

            return Ok(res);
        }
        [HttpPost]
        public ActionResult ObtenerMaterialesPorNombre([FromBody] Material material)
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                material.Id = id;
                var materiales = _briefService.GetMaterialesFilter(material);
                res.Datos = materiales;
                res.Exito = true;
            }
            catch (Exception)
            {

                res.Mensaje = "Petición fallida";
                res.Exito = false;
            }


            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerConteoEstatusMateriales()
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var materiales = _briefService.ObtenerConteoEstatusMateriales(id);
                res.Datos = materiales;
                res.Exito = true;
            }
            catch (Exception)
            {

                res.Mensaje = "Petición fallida";
                res.Exito = false;
            }


            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerEstatusMateriales()
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var EstatusMaterial = _briefService.GetAllEstatusMateriales();
                res.Datos = EstatusMaterial;
                res.Exito = true;
            }
            catch (Exception)
            {

                res.Mensaje = "Petición fallida";
                res.Exito = false;
            }


            return Ok(res);

        }
        [HttpPost]
        public ActionResult AgregarHistorialMaterial([FromBody] AgregarHistorialMaterialRequest historialMaterialRequest)
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var rolId = Int32.Parse(User.FindFirst(ClaimTypes.Role)?.Value);

                // Validar permisos: Usuario (RolId=2) no puede cambiar estatus
                var material = _briefService.GetMaterial(historialMaterialRequest.HistorialMaterial.MaterialId);
                if (rolId == 2 && material.EstatusMaterialId != historialMaterialRequest.HistorialMaterial.EstatusMaterialId)
                {
                    res.Mensaje = "No tiene permisos para cambiar el estatus del material.";
                    res.Exito = false;
                    return Ok(res);
                }

                historialMaterialRequest.HistorialMaterial.UsuarioId = UsuarioId;
                var id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var usuarioLogueado =_usuarioService.TGetById(id);
               _briefService.ActualizaHistorialMaterial(historialMaterialRequest.HistorialMaterial);

                var urlBase = $"{Request.Scheme}://{Request.Host}";

                // Siempre crear alerta al usuario del brief cuando hay un nuevo comentario
                if (material != null && material.Brief != null)
                {
                    // Notificar al dueño del brief
                    var alertaComentario = new Alerta
                    {
                        IdUsuario = material.Brief.UsuarioId,
                        Nombre = "Nuevo Comentario en Material",
                        Descripcion = $"{usuarioLogueado.Nombre} agregó un comentario en el material '{material.Nombre}'",
                        IdTipoAlerta = 3,
                        Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                    };
                    _toolsService.CrearAlerta(alertaComentario);

                    // Notificar a todos los participantes del brief (excepto quien comentó)
                    var participantes = _toolsService.ObtenerParticipantes(material.BriefId);
                    foreach (var participante in participantes)
                    {
                        // No notificar al usuario que hizo el comentario
                        if (participante.UsuarioId != UsuarioId)
                        {
                            var alertaParticipante = new Alerta
                            {
                                IdUsuario = participante.UsuarioId,
                                Nombre = "Nuevo Comentario en Material",
                                Descripcion = $"{usuarioLogueado.Nombre} agregó un comentario en el material '{material.Nombre}'",
                                IdTipoAlerta = 3,
                                Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                            };
                            _toolsService.CrearAlerta(alertaParticipante);
                        }
                    }
                }

                // Si cambió el estatus, notificar también
                if (material.EstatusMaterialId != historialMaterialRequest.HistorialMaterial.EstatusMaterialId)
                {
                    var estatusNuevo = _briefService.GetAllEstatusMateriales()
                        .FirstOrDefault(e => e.Id == historialMaterialRequest.HistorialMaterial.EstatusMaterialId);

                    if (material.Brief != null && estatusNuevo != null)
                    {
                        // Notificar al dueño del brief
                        var alertaEstatus = new Alerta
                        {
                            IdUsuario = material.Brief.UsuarioId,
                            Nombre = "Cambio de Estatus en Material",
                            Descripcion = $"El material '{material.Nombre}' cambió a estatus '{estatusNuevo.Descripcion}'",
                            IdTipoAlerta = 4,
                            Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                        };
                        _toolsService.CrearAlerta(alertaEstatus);

                        // Notificar a todos los participantes del brief (excepto quien cambió el estatus)
                        var participantes = _toolsService.ObtenerParticipantes(material.BriefId);
                        foreach (var participante in participantes)
                        {
                            if (participante.UsuarioId != UsuarioId)
                            {
                                var alertaEstatusParticipante = new Alerta
                                {
                                    IdUsuario = participante.UsuarioId,
                                    Nombre = "Cambio de Estatus en Material",
                                    Descripcion = $"El material '{material.Nombre}' cambió a estatus '{estatusNuevo.Descripcion}'",
                                    IdTipoAlerta = 4,
                                    Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                                };
                                _toolsService.CrearAlerta(alertaEstatusParticipante);
                            }
                        }
                    }
                }

                // Si el material pasa a estado "Entregado" (5), crear alerta adicional
                if (historialMaterialRequest.HistorialMaterial.EstatusMaterialId == 5)
                {
                    if (material != null && material.Brief != null)
                    {
                        // Notificar al dueño del brief
                        var alertaUsuario = new Alerta
                        {
                            IdUsuario = material.Brief.UsuarioId,
                            Nombre = "Material Entregado",
                            Descripcion = $"El material '{material.Nombre}' ha sido entregado",
                            IdTipoAlerta = 5,
                            Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                        };
                        _toolsService.CrearAlerta(alertaUsuario);

                        // Notificar a todos los participantes del brief (excepto quien marcó como entregado)
                        var participantes = _toolsService.ObtenerParticipantes(material.BriefId);
                        foreach (var participante in participantes)
                        {
                            if (participante.UsuarioId != UsuarioId)
                            {
                                var alertaEntregadoParticipante = new Alerta
                                {
                                    IdUsuario = participante.UsuarioId,
                                    Nombre = "Material Entregado",
                                    Descripcion = $"El material '{material.Nombre}' ha sido entregado",
                                    IdTipoAlerta = 5,
                                    Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                                };
                                _toolsService.CrearAlerta(alertaEntregadoParticipante);
                            }
                        }
                    }
                }

                if (historialMaterialRequest.EnvioCorreo && historialMaterialRequest.Usuarios != null && historialMaterialRequest.Usuarios.Any())
                {
                    var EstatusMaterial = _briefService.GetAllEstatusMateriales().Where(q => q.Id == historialMaterialRequest.HistorialMaterial.EstatusMaterialId).FirstOrDefault();
                    var Destinatarios = new List<string>();

                    // Solo extraer los correos de los usuarios, sin hacer consultas adicionales
                    foreach(var item in historialMaterialRequest.Usuarios)
                    {
                        if (!string.IsNullOrEmpty(item.Correo))
                        {
                            Destinatarios.Add(item.Correo);
                        }
                    }
                    Destinatarios.AddRange(_toolsService.GetUsuarioByRol(3).Select(q => q.Correo).ToList());

                    // Diccionario con los valores dinámicos a reemplazar
                    var valoresDinamicos = new Dictionary<string, string>()
                {
                    { "nombreMaterial", material.Nombre},
                    { "usuario",usuarioLogueado.Nombre },
                    { "estatus", EstatusMaterial.Descripcion},
                    { "comentario", historialMaterialRequest.HistorialMaterial.Comentarios },
                    { "link", urlBase + "/Materiales?filtroNombre=" + material.Nombre }
                };
                    _emailSender.SendEmail(Destinatarios, "ComentarioMaterial", valoresDinamicos);
                }

                res.Mensaje = "Solicitud Exitosa";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en AgregarHistorialMaterial");
                res.Mensaje = $"Petición fallida: {ex.Message}";
                res.Exito = false;
            }


            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerHistorial(int id)
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var materiales = _briefService.GetAllHistorialMateriales(id);
                res.Datos = materiales;
                res.Exito = true;
            }
            catch (Exception)
            {

                res.Mensaje = "Petición fallida";
                res.Exito = false;
            }


            return Ok(res);

        }

        [HttpPost]
        public ActionResult NotificarParticipante([FromBody] NotificarParticipanteRequest request)
        {
            respuestaServicio res = new respuestaServicio();
            try
            {
                var usuarioActualId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var usuarioActual = _usuarioService.TGetById(usuarioActualId);
                var material = _briefService.GetMaterial(request.MaterialId);
                var participante = _usuarioService.TGetById(request.ParticipanteId);

                if (material == null || participante == null)
                {
                    res.Mensaje = "Material o participante no encontrado.";
                    res.Exito = false;
                    return Ok(res);
                }

                var urlBase = $"{Request.Scheme}://{Request.Host}";

                // Crear alerta para el participante agregado
                var alerta = new Alerta
                {
                    IdUsuario = participante.Id,
                    Nombre = "Agregado como Participante",
                    Descripcion = $"{usuarioActual.Nombre} te agregó como participante en el material '{material.Nombre}'",
                    IdTipoAlerta = 3,
                    Accion = $"{urlBase}/Materiales?filtroNombre={material.Nombre}"
                };
                _toolsService.CrearAlerta(alerta);

                res.Mensaje = "Notificación enviada exitosamente.";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = $"Error al enviar notificación: {ex.Message}";
                res.Exito = false;
            }

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Upload: No file provided");
                    return BadRequest(new { error = "No se proporcionó ningún archivo." });
                }

                _logger.LogInformation($"Upload: Receiving file {file.FileName}, size: {file.Length} bytes");

                var uploadsFolder = Path.Combine("wwwroot", "uploads");

                // Asegurar que el directorio existe
                if (!Directory.Exists(uploadsFolder))
                {
                    _logger.LogInformation($"Upload: Creating directory {uploadsFolder}");
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                _logger.LogInformation($"Upload: Saving file to {filePath}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = Url.Content($"~/uploads/{fileName}");
                _logger.LogInformation($"Upload: File saved successfully, URL: {fileUrl}");

                return Json(new { location = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload: Error uploading image");
                return BadRequest(new { error = $"Error al cargar la imagen: {ex.Message}" });
            }
        }
    }
}