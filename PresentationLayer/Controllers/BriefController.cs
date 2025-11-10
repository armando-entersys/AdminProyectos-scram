using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Hubs;
using PresentationLayer.Models;
using System.Security.Claims;


namespace PresentationLayer.Controllers
{
    [Authorize]
    public class BriefController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly IBriefService _briefService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IToolsService _toolsService;
        private readonly IUsuarioService _usuarioService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _configuration;

        public BriefController(IEmailSender emailSender, IBriefService briefService, IAuthService authService, 
                                IWebHostEnvironment hostingEnvironment, IToolsService toolsService, 
                                IUsuarioService usuarioService, IHubContext<NotificationHub> hubContext,IConfiguration configuration)
        {
            _emailSender = emailSender;
            _briefService = briefService;
            _authService = authService;
            _hostingEnvironment = hostingEnvironment;
            _toolsService = toolsService;
            _usuarioService = usuarioService;
            _hubContext = hubContext;
            _configuration = configuration;
        }
        // GET: BriefController
        public ActionResult Index(string filtroNombre = null)
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
        [Authorize]
        public ActionResult IndexAdmin()
        {
            ViewBag.RolId = Int32.Parse(User.FindFirst(ClaimTypes.Role)?.Value);
            ViewBag.Email = User.FindFirst(ClaimTypes.Email)?.Value;
            ViewBag.Name = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            ViewBag.Menus = _authService.GetMenusByRole(ViewBag.RolId);
            ViewBag.ConteoAlertas = _toolsService.GetUnreadAlertsCount(ViewBag.UsuarioId);

            return View();
        }
        [HttpGet]
        public IActionResult GetAllColumns()
        {
            int UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            respuestaServicio res = new respuestaServicio();
            var columns = _briefService.GetColumnsByUserId(UsuarioId);
            res.Datos = columns;
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllbyUserId()
        {
            int UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            respuestaServicio res = new respuestaServicio();
            res.Datos = _briefService.GetAllbyUserId(UsuarioId,true);
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllbyUserBrief()
        {
            int UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            respuestaServicio res = new respuestaServicio();
            res.Datos = _briefService.GetAllbyUserId(UsuarioId,false);
            res.Exito = true;

            return Ok(res);
        }
        // GET: BriefController/Details/5
        public ActionResult Details(int id)
        {
            respuestaServicio res = new respuestaServicio();
            var brief = _briefService.GetById(id);
            res.Datos = brief;
            res.Exito = true;


            return Ok(res);

        }
        [HttpGet]
        public IActionResult DownloadFile(int id)
        {
            var brief = _briefService.GetById(id);

            if (brief == null || string.IsNullOrEmpty(brief.RutaArchivo))
            {
                return NotFound("Ruta no encontrado.");
            }

            // Obtener la ruta del archivo en el servidor
            // Leer la ruta desde appsettings.json
            var uploadPath = _configuration["FileStorage:UploadPath"];
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, uploadPath, brief.Id.ToString(), brief.RutaArchivo);

            if (!System.IO.File.Exists(filePath))
            {
                // Registra detalles para verificar qué ruta está generando
                return NotFound("Archivo no encontrado: " +  filePath);
            }

            // Obtener el tipo MIME del archivo
            var mimeType = GetMimeType(filePath);

            // Devolver el archivo para descargar
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType, Path.GetFileName(filePath));
        }

        // Método auxiliar para determinar el tipo MIME basado en la extensión del archivo
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".mkv" => "video/x-matroska",
                _ => "application/octet-stream",
            };
        }

        [HttpPost]
        public ActionResult Create([FromBody] Brief brief)
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                // Asignar usuario actual si no viene en el brief
                if (brief.UsuarioId == 0)
                {
                    brief.UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }

                // Asignar estatus inicial "En revisión" (id=1) si no viene
                if (brief.EstatusBriefId == 0)
                {
                    brief.EstatusBriefId = 1;
                }

                brief.FechaModificacion = DateTime.Now;
                brief.FechaRegistro = DateTime.Now;

                _briefService.Insert(brief);

                res.Datos = brief;
                res.Mensaje = "Brief agregado exitosamente";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = $"Error al crear brief: {ex.Message}";
                res.Exito = false;
            }

            return Ok(res);
        }

        [HttpPut]
        public ActionResult EditStatus([FromBody] Brief brief)
        {
            var res = new respuestaServicio();

            // Verificar que solo el Administrador (RolId=1) pueda cambiar el estatus
            int rolId = Int32.Parse(User.FindFirst(ClaimTypes.Role)?.Value);
            if (rolId != 1)
            {
                res.Mensaje = "No tiene permisos para cambiar el estatus del proyecto.";
                res.Exito = false;
                return Ok(res);
            }

            var BriefOrg = _briefService.GetById(brief.Id);

            if (brief.EstatusBriefId == BriefOrg.EstatusBriefId)
            {
                res.Mensaje = "El estatus no ha cambiado.";
                res.Exito = false;
                return Ok(res);
            }

            // Actualizar el estatus en el Brief original
            BriefOrg.EstatusBriefId = brief.EstatusBriefId;
            BriefOrg.FechaModificacion = DateTime.Now;

            // Actualizar el Brief en la base de datos
            _briefService.Update(BriefOrg);

            // Obtener estatus actualizado
            var estatusBriefs = _briefService.GetAllEstatusBrief();
            BriefOrg.EstatusBrief = estatusBriefs.FirstOrDefault(q => q.Id == BriefOrg.EstatusBriefId);

            // Obtener destinatarios según el nuevo estatus
            var urlBase = $"{Request.Scheme}://{Request.Host}";
            var destinatarios = GetRecipientsByStatus(BriefOrg, urlBase);

            // Crear una alerta general
            CreateAlert(BriefOrg, urlBase);

            // Enviar notificación por correo
            SendStatusChangeEmail(destinatarios, BriefOrg, urlBase);

            res.Datos = BriefOrg;
            res.Mensaje = "Actualizado exitosamente";
            res.Exito = true;

            return Ok(res);
        }

        // Métodos auxiliares

        private void UpdateBriefData(Brief brief, Brief BriefOrg)
        {
            brief.UsuarioId = BriefOrg.UsuarioId;
            brief.Comentario = BriefOrg.Comentario;
            brief.Nombre = BriefOrg.Nombre;
            brief.Objetivo = BriefOrg.Objetivo;
            brief.RutaArchivo = BriefOrg.RutaArchivo;
            brief.LinksReferencias = BriefOrg.LinksReferencias;
            brief.DirigidoA = BriefOrg.DirigidoA;
            brief.Descripcion = BriefOrg.Descripcion;
            brief.FechaEntrega = BriefOrg.FechaEntrega;
            brief.FechaRegistro = BriefOrg.FechaRegistro;
            brief.FechaModificacion = DateTime.Now;
            brief.TipoBriefId = BriefOrg.TipoBriefId;
        }

        private List<string> GetRecipientsByStatus(Brief brief, string urlBase)
        {
            var recipients = new List<string>();

            if (brief.EstatusBriefId == 1)
            {
                recipients = _toolsService.GetUsuarioByRol(1).Select(q => q.Correo).ToList();
                recipients.AddRange(_toolsService.ObtenerParticipantes(brief.Id).Select(q => q.Usuario.Correo));
            }
            else if (brief.EstatusBriefId == 2)
            {
                // Obtener solo los participantes del brief que tienen rol Producción
                var participantes = _toolsService.ObtenerParticipantes(brief.Id);
                var participantesProduccion = participantes
                    .Where(p => p.Usuario.RolId == 3)
                    .ToList();

                // Agregar correos de participantes de producción
                recipients.AddRange(participantesProduccion.Select(p => p.Usuario.Correo));
                recipients.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);

                // Crear alertas solo para participantes de producción
                foreach (var participante in participantesProduccion)
                {
                    CreateProductionAlert(participante.UsuarioId, brief, urlBase);
                }
            }
            else if (brief.EstatusBriefId >= 3 && brief.EstatusBriefId <= 6)
            {
                recipients.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);
            }
            else if (brief.EstatusBriefId == 7 || brief.EstatusBriefId == 8)
            {
                // Obtener solo los participantes del brief que tienen rol Producción
                var participantes = _toolsService.ObtenerParticipantes(brief.Id);
                var participantesProduccion = participantes
                    .Where(p => p.Usuario.RolId == 3)
                    .Select(p => p.Usuario.Correo)
                    .ToList();

                recipients.AddRange(participantesProduccion);
                recipients.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);
            }

            return recipients;
        }

        private void CreateProductionAlert(int userId, Brief brief, string urlBase)
        {
            _toolsService.CrearAlerta(new Alerta
            {
                IdUsuario = userId,
                Nombre = $"Cambio Estatus Proyecto {brief.Nombre}",
                Descripcion = $"Cambio de estatus a {brief.EstatusBrief.Descripcion}",
                IdTipoAlerta = 3,
                Accion = $"{urlBase}/Brief?filtroNombre={brief.Nombre}"
            });
        }

        private void CreateAlert(Brief brief, string urlBase)
        {
            _toolsService.CrearAlerta(new Alerta
            {
                IdUsuario = brief.UsuarioId,
                Nombre = $"Cambio Estatus Proyecto {brief.Nombre}",
                Descripcion = $"Cambio de estatus a {brief.EstatusBrief.Descripcion}",
                IdTipoAlerta = 3,
                Accion = $"{urlBase}/Brief?filtroNombre={brief.Nombre}"
            });
        }

        private void SendStatusChangeEmail(List<string> recipients, Brief brief, string urlBase)
        {
            var dynamicValues = new Dictionary<string, string>
    {
        { "estatus", brief.EstatusBrief.Descripcion },
        { "nombreProyecto", brief.Nombre },
        { "link", $"{urlBase}/Brief?filtroNombre={brief.Nombre}" }
    };

            _emailSender.SendEmail(recipients, "ActualizaEstatusProyecto", dynamicValues);
        }


        [HttpPost]
        public ActionResult AddBrief([FromForm] ArchivoT Addbrief)
        {
            respuestaServicio res = new respuestaServicio();

           
            Addbrief.Comentario = "";
            Brief brief = new Brief
            {
                Id = Addbrief.Id,
                UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Nombre = Addbrief.Nombre,
                Descripcion = Addbrief.Descripcion,
                Objetivo = Addbrief.Objetivo,
                DirigidoA = Addbrief.DirigidoA,
                Comentario = Addbrief.Comentario,
                LinksReferencias = Addbrief.LinksReferencias,
                RutaArchivo = Addbrief.RutaArchivo,
                TipoBriefId = Addbrief.TipoBriefId,
                FechaEntrega = Addbrief.FechaEntrega,
                EstatusBriefId = Addbrief.EstatusBriefId,
                FechaModificacion = DateTime.Now

                // Puedes asignar más propiedades de `Addbrief` si es necesario
            };

            _briefService.Insert(brief);

            if (Addbrief.Archivo != null && (Addbrief.Archivo.ContentType == "application/pdf" || // PDF
                                      Addbrief.Archivo.ContentType == "application/msword" || // Word (doc)
                                      Addbrief.Archivo.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || // Word (docx)
                                      Addbrief.Archivo.ContentType == "application/vnd.ms-excel" || // Excel (xls)
                                      Addbrief.Archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || // Excel (xlsx)
                                      Addbrief.Archivo.ContentType == "image/jpeg" || // JPG
                                      Addbrief.Archivo.ContentType == "image/png" || // PNG
                                      Addbrief.Archivo.ContentType == "video/mp4" || // MP4
                                      Addbrief.Archivo.ContentType == "video/x-msvideo" || // AVI
                                      Addbrief.Archivo.ContentType == "video/x-matroska")) // MKV
            {
                // Guardar el archivo en una ruta específica o procesarlo según sea necesario
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "Brief", brief.Id.ToString());

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, Addbrief.Archivo.FileName);
                Addbrief.RutaArchivo = Addbrief.Archivo.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Addbrief.Archivo.CopyTo(stream);
                }
            }
            //Envio Correo
            var urlBase = $"{Request.Scheme}://{Request.Host}";
            // Diccionario con los valores dinámicos a reemplazar
            var valoresDinamicos = new Dictionary<string, string>()
            {
                { "nombre", User.FindFirst(ClaimTypes.Name)?.Value },
                { "nombreProyecto", brief.Nombre },
                { "link", urlBase + "/Brief/IndexAdmin?filtroNombre=" + brief.Nombre }
            };
            var Destinatarios = _toolsService.GetUsuarioByRol(1).Select(q => q.Correo).ToList();

            _emailSender.SendEmail(Destinatarios, "NuevoProyecto", valoresDinamicos);
            
            var usuariosAdmin = _toolsService.GetUsuarioByRol(1).Select(q => q.Id).ToList();
            foreach(var item in usuariosAdmin)
            {
                Alerta alertaUsuario = new Alerta
                {
                    IdUsuario = item,
                    Nombre = "Nuevo Proyecto",
                    Descripcion = "Se agrego un nuevo proyecto " + brief.Nombre,
                    IdTipoAlerta = 1,
                    Accion = urlBase + "/Brief/IndexAdmin?filtroNombre=" + brief.Nombre

                };

                _toolsService.CrearAlerta(alertaUsuario);
            }
           

            res.Datos = brief;
            res.Mensaje = "Se ha recibido correctamente tu petición.\r\nEn breve recibirás una notificación del estatus de tu proceso.";
            res.Exito = true;

            return Ok(res);
        }
        [HttpPost]
        public ActionResult EditBrief([FromForm] ArchivoT Addbrief)
        {
            respuestaServicio res = new respuestaServicio();
            Brief briefOld = _briefService.GetById(Addbrief.Id);
            if (Addbrief.Archivo != null && (Addbrief.Archivo.ContentType == "application/pdf" || // PDF
                                             Addbrief.Archivo.ContentType == "application/msword" || // Word (doc)
                                             Addbrief.Archivo.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || // Word (docx)
                                             Addbrief.Archivo.ContentType == "application/vnd.ms-excel" || // Excel (xls)
                                             Addbrief.Archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || // Excel (xlsx)
                                             Addbrief.Archivo.ContentType == "image/jpeg" || // JPG
                                             Addbrief.Archivo.ContentType == "image/png" || // PNG
                                             Addbrief.Archivo.ContentType == "video/mp4" || // MP4
                                             Addbrief.Archivo.ContentType == "video/x-msvideo" || // AVI
                                             Addbrief.Archivo.ContentType == "video/x-matroska")) // MKV
            {
                // Guardar el archivo en una ruta específica o procesarlo según sea necesario
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "Brief", briefOld.Id.ToString());

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, Addbrief.Archivo.FileName);
                Addbrief.RutaArchivo = Addbrief.Archivo.FileName;
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Addbrief.Archivo.CopyTo(stream);
                    }
                }
                catch (Exception ex)
                {
                    res.Mensaje = "Error al guardar el archivo: " + ex.Message;
                }
            }
            else
            {
                Addbrief.RutaArchivo = briefOld.RutaArchivo;
            }

            Brief brief = new Brief
            {
                Id = Addbrief.Id,
                UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Nombre = Addbrief.Nombre,
                Descripcion = Addbrief.Descripcion,
                Objetivo = Addbrief.Objetivo,
                DirigidoA = Addbrief.DirigidoA,
                Comentario = briefOld.Comentario,
                RutaArchivo = Addbrief.RutaArchivo,
                TipoBriefId = Addbrief.TipoBriefId,
                FechaEntrega = Addbrief.FechaEntrega,
                EstatusBriefId = Addbrief.EstatusBriefId,
                FechaModificacion = DateTime.Now,
                LinksReferencias = Addbrief.LinksReferencias

                // Puedes asignar más propiedades de `Addbrief` si es necesario
            };

            _briefService.Update(brief);
            //Envio Correo
            var urlBase = $"{Request.Scheme}://{Request.Host}";
            // Diccionario con los valores dinámicos a reemplazar
            var valoresDinamicos = new Dictionary<string, string>()
            {
                { "nombre", User.FindFirst(ClaimTypes.Name)?.Value },
                { "nombreProyecto", brief.Nombre },
                { "link", urlBase + "/BriefAdmin?filtroNombre=" + brief.Nombre  }

            };
            var Destinatarios = _toolsService.GetUsuarioByRol(1).Select(q => q.Correo).ToList();

            // Obtener todos los participantes del brief y agregar sus correos
            var participantes = _toolsService.ObtenerParticipantes(brief.Id);
            foreach (var participante in participantes)
            {
                if (!Destinatarios.Contains(participante.Usuario.Correo))
                {
                    Destinatarios.Add(participante.Usuario.Correo);
                }
            }

            _emailSender.SendEmail(Destinatarios, "EdicionBreaf", valoresDinamicos);

            // Crear alertas para todos los participantes
            var usuarioLogueado = _usuarioService.TGetById(Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            foreach (var participante in participantes)
            {
                var alertaParticipante = new Alerta
                {
                    IdUsuario = participante.UsuarioId,
                    Nombre = "Proyecto Modificado",
                    Descripcion = $"{usuarioLogueado.Nombre} modificó el proyecto '{brief.Nombre}'",
                    FechaCreacion = DateTime.Now,
                    lectura = false,
                    IdTipoAlerta = 3,
                    Accion = $"{urlBase}/Brief?filtroNombre={brief.Nombre}"
                };
                _toolsService.CrearAlerta(alertaParticipante);
            }

            res.Datos = brief;
            res.Mensaje = "Se ha recibido correctamente tu solicitud.";
            res.Exito = true;

            return Ok(res);
        }

        [HttpGet]
        public IActionResult GetAllEstatusBrief()
        {
            respuestaServicio res = new respuestaServicio();
            var roles = _briefService.GetAllEstatusBrief();
            res.Datos = roles;
            res.Exito = true;


            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllTipoBrief()
        {
            respuestaServicio res = new respuestaServicio();
            var roles = _briefService.GetAllTipoBrief();
            res.Datos = roles;
            res.Exito = true;


            return Ok(res);
        }

        [HttpPost]
        public ActionResult CreateProyecto([FromBody] Proyecto proyecto)
        {
            respuestaServicio res = new respuestaServicio();

            proyecto.FechaModificacion = DateTime.Now;
            try
            {
                _briefService.InsertProyecto(proyecto);
                res.Datos = _briefService.GetProyectoByBriefId(proyecto.BriefId);
                res.Mensaje = "Creado exitosamente";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Error al guardar";
                res.Exito = false;
            }

            return Ok(res);
        }
        [HttpPost]
        public ActionResult CreateMaterial([FromBody] CreateMaterialRequest request)
        {
            respuestaServicio res = new respuestaServicio();
            var urlBase = $"{Request.Scheme}://{Request.Host}";

            // Validar que exista al menos un participante con perfil de producción
            var participantes = _toolsService.ObtenerParticipantes(request.BriefId);
            var tieneProduccion = participantes.Any(p => p.Usuario.RolId == 3);

            if (!tieneProduccion)
            {
                res.Mensaje = "DEBE AGREGAR AL MENOS UN PERFIL PRODUCCIÓN A SU PROYECTO";
                res.Exito = false;
                return Ok(res);
            }

            // Crear el material desde el request
            var material = new Material
            {
                BriefId = request.BriefId,
                Nombre = request.Nombre,
                Mensaje = request.Mensaje,
                PrioridadId = request.PrioridadId,
                Ciclo = request.Ciclo,
                AudienciaId = request.AudienciaId,
                FormatoId = request.FormatoId,
                FechaEntrega = request.FechaEntrega,
                Responsable = request.Responsable,
                Area = request.Area,
                FechaModificacion = DateTime.Now,
                EstatusMaterialId = 1
            };

            try
            {
                // Insertar material con sus PCNs
                _briefService.InsertMaterialConPCNs(material, request.PCNIds);
                var brief = _briefService.GetById(material.BriefId);
                Alerta alertaUsuario = new Alerta
                {
                    IdUsuario = brief.UsuarioId,
                    Nombre = "Nuevo Material",
                    Descripcion = "Se agrego un material al proyecto " + brief.Nombre,
                    IdTipoAlerta = 4,
                    Accion = urlBase + "/Materiales?filtroNombre="+material.Nombre

                };

                _toolsService.CrearAlerta(alertaUsuario);

                // Obtener todos los administradores
                var usuariosAdmin = _toolsService.GetUsuarioByRol(1).Select(q=> q.Id).ToList();

                // Notificar a todos los administradores
                foreach(var adminId in usuariosAdmin)
                {
                    Alerta alertaAdmin = new Alerta
                    {
                        IdUsuario = adminId,
                        Nombre = "Nuevo Material",
                        Descripcion = "Se agrego un material al proyecto " + brief.Nombre,
                        IdTipoAlerta = 4,
                        Accion = urlBase + "/Materiales?filtroNombre=" + material.Nombre
                    };
                    _toolsService.CrearAlerta(alertaAdmin);
                }

                // Obtener solo los participantes del brief que tienen rol Producción
                var participantesBrief = _toolsService.ObtenerParticipantes(material.BriefId);
                var participantesProduccion = participantesBrief
                    .Where(p => p.Usuario.RolId == 3) // Solo los de producción
                    .ToList();

                // Notificar solo a los usuarios de producción que son participantes del brief
                foreach(var participante in participantesProduccion)
                {
                    Alerta alertaProduccion = new Alerta
                    {
                        IdUsuario = participante.UsuarioId,
                        Nombre = "Nuevo Material",
                        Descripcion = "Se agrego un material al proyecto " + brief.Nombre,
                        IdTipoAlerta = 4,
                        Accion = urlBase + "/Materiales?filtroNombre=" + material.Nombre
                    };
                    _toolsService.CrearAlerta(alertaProduccion);
                }
               

                // Diccionario con los valores dinámicos a reemplazar
                var valoresDinamicos = new Dictionary<string, string>()
                {
                    { "nombreProyecto", brief.Nombre },
                    { "nombreMaterial", material.Nombre },
                    { "link", urlBase + "/Materiales?filtroNombre=" + material.Nombre  }

                };
                var Destinatarios = new List<string>();
                Destinatarios.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);

                Destinatarios.AddRange(_toolsService.GetUsuarioByRol(3).Select(q => q.Correo).ToList());

                _emailSender.SendEmail(Destinatarios, "NuevoMaterial", valoresDinamicos);

                res.Mensaje = "Creado exitosamente";
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Error al Crear el Material";
                res.Exito = false;
            }

            return Ok(res);
        }
        [HttpGet]
        public ActionResult ObtenerProyectoPorBrief(int id)
        {
            respuestaServicio res = new respuestaServicio();
            var proyecto = _briefService.GetProyectoByBriefId(id);
            res.Datos = proyecto;
            res.Exito = true;

            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerMateriales(int id)
        {
            respuestaServicio res = new respuestaServicio();
            var materiales = _briefService.GetMaterialesByBriefId(id);
            res.Datos = materiales;
            res.Exito = true;

            return Ok(res);

        }
        [HttpGet]
        public ActionResult EliminarMaterial(int id)
        {
            respuestaServicio res = new respuestaServicio();
            
            try
            {
                _briefService.EliminarMaterial(id);
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Error al remover el Material";
                res.Exito = false;
            }
            return Ok(res);

        }
        [HttpGet]
        public ActionResult EliminarParticipante(int id)
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                _briefService.EliminarParticipante(id);
                res.Exito = true;
            }
            catch (Exception ex)
            {
                res.Mensaje = "Error al remover el Participante";
                res.Exito = false;
            }
            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerConteoPorProyectos()
        {
            respuestaServicio res = new respuestaServicio();

            try
           {
                var UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                res.Datos =_briefService.ObtenerConteoProyectos(UsuarioId);
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
        [HttpGet]
        public ActionResult ObtenerConteoMateriales()
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                Console.WriteLine("=== INICIO ObtenerConteoMateriales ===");
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"UserIdClaim: {userIdClaim}");

                var UsuarioId = Int32.Parse(userIdClaim);
                Console.WriteLine($"UsuarioId parsed: {UsuarioId}");

                var resultado = _briefService.ObtenerConteoMateriales(UsuarioId);
                Console.WriteLine($"Resultado obtenido: Hoy={resultado.Hoy}, EstaSemana={resultado.EstaSemana}, ProximaSemana={resultado.ProximaSemana}, Total={resultado.TotalProyectos}");

                res.Datos = resultado;
                res.Mensaje = "Solicitud Exitosa";
                res.Exito = true;

                Console.WriteLine($"Respuesta: Exito={res.Exito}, Mensaje={res.Mensaje}");
                Console.WriteLine("=== FIN ObtenerConteoMateriales EXITOSO ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR en ObtenerConteoMateriales ===");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                Console.WriteLine("=== FIN ERROR ===");

                res.Mensaje = $"Petición fallida: {ex.Message}";
                res.Exito = false;
            }
            return Ok(res);

        }
        [HttpGet]
        public ActionResult ObtenerConteoProyectoFecha()
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                var UsuarioId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                res.Datos = _briefService.ObtenerConteoProyectoFecha(UsuarioId);
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
        [HttpGet]
        public IActionResult GetAllPrioridad()
        {
            respuestaServicio res = new respuestaServicio();
            var data = _briefService.GetAllPrioridades();
            res.Datos = data;
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllAudiencias()
        {
            respuestaServicio res = new respuestaServicio();
            var data = _briefService.GetAllAudiencias();
            res.Datos = data;
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllPCN()
        {
            respuestaServicio res = new respuestaServicio();
            var data = _briefService.GetAllPCN();
            res.Datos = data;
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public IActionResult GetAllFormatos()
        {
            respuestaServicio res = new respuestaServicio();
            var data = _briefService.GetAllFormatos();
            res.Datos = data;
            res.Exito = true;

            return Ok(res);
        }
        [HttpGet]
        public ActionResult EliminarBrief(int id)
        {
            respuestaServicio res = new respuestaServicio();

            try
            {
                var brief = _briefService.GetById(id);
                _briefService.Delete(id);
                var urlBase = $"{Request.Scheme}://{Request.Host}";

                res.Exito = true;
                var usuarioLogin = _usuarioService.TGetById(Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                // Diccionario con los valores dinámicos a reemplazar
                var valoresDinamicos = new Dictionary<string, string>()
                {
                    { "nombreProyecto", brief.Nombre },
                    { "usuario", usuarioLogin.Nombre + " " + usuarioLogin.ApellidoPaterno },

                };
                var Destinatarios = new List<string>();
                Destinatarios.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);
                Destinatarios.AddRange(_usuarioService.TGetAll().Where(q=> q.RolId == 1).Select(p => p.Correo).ToList());

                Destinatarios.Add(_usuarioService.TGetById(brief.UsuarioId).Correo);


                _emailSender.SendEmail(Destinatarios, "EliminarProyecto", valoresDinamicos);
            }
            catch (Exception ex)
            {
                res.Mensaje = "Error al remover el Material";
                res.Exito = false;
            }
            return Ok(res);

        }
        
    }
}
