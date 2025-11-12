using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class BriefRepository<T> : IBriefDal where T : class
    {
        private readonly DataAccesContext _context;

        public BriefRepository(DataAccesContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Elimina un Brief por su ID.
        /// </summary>
        /// <param name="id">ID del Brief a eliminar.</param>
        public void Delete(int id)
        {
            var brief = _context.Briefs.Find(id);
            if (brief != null)
            {
                var proyecto = _context.Proyectos.Where(p => p.BriefId == brief.Id).FirstOrDefault();
                if (proyecto != null) {
                    _context.Proyectos.Remove(proyecto);
                }
                var materiales = _context.Materiales.Where(p => p.BriefId == brief.Id).ToList();
                if (materiales != null) {
                    _context.Materiales.RemoveRange(materiales);
                }
                var participantes = _context.Participantes.Where(p => p.BriefId == brief.Id).ToList();
                if (participantes != null) {
                    _context.Participantes.RemoveRange(participantes);
                }
                _context.SaveChanges();

                _context.Briefs.Remove(brief);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene todos los Briefs cuya fecha de entrega es dentro de los últimos 15 días.
        /// </summary>
        /// <returns>Lista de Briefs.</returns>
        public List<Brief> GetAll()
        {
            return _context.Briefs
                .Where(q => q.FechaEntrega >= DateTime.Now.AddDays(-15))
                .ToList();
        }

        /// <summary>
        /// Obtiene las columnas de Briefs agrupadas por estatus para un usuario específico o para todos si es administrador.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Lista de Columnas de Briefs.</returns>
        public List<Column<Brief>> GetColumnsByUserId(int id)
        {
            var isAdmin = _context.Usuarios
                .Any(u => u.Id == id && (u.RolId == 1 || u.RolId == 3));

            var briefsQuery = _context.Briefs.Where(b => b.UsuarioId == id).AsQueryable(); 

            if (isAdmin)
            {
                briefsQuery = _context.Briefs.AsQueryable();
            }

            var briefs = briefsQuery
                .OrderByDescending(b => b.FechaEntrega)
                .Include(b => b.Usuario)
                .ToList();

            var estatusBriefs = _context.EstatusBriefs
                .Where(e => e.Activo)
                .ToList();

            return estatusBriefs.Select(e => new Column<Brief>
            {
                Id = e.Id,
                Name = e.Descripcion,
                Tasks = briefs
                    .Where(b => b.EstatusBriefId == e.Id)
                    .Select(b => new Tasks<Brief>
                    {
                        Id = b.Id,
                        Title = b.Nombre,
                        UsuarioId = b.UsuarioId,
                        NombreUsuario = b.Usuario.Nombre,
                        FechaEntrega = b.FechaEntrega.ToShortDateString()
                    })
                    .ToList()
            }).ToList();
        }

        /// <summary>
        /// Obtiene todos los Briefs asociados a un usuario. Si el usuario es administrador, devuelve todos los Briefs.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <param name="onlybrief">Si es true, solo devuelve Briefs del usuario.</param>
        /// <returns>Enumerable de Briefs.</returns>
        public IEnumerable<Brief> GetAllbyUserId(int usuarioId, bool onlybrief)
        {
            var isAdmin = _context.Usuarios
                .Any(u => u.Id == usuarioId && (u.RolId == 1 || u.RolId == 3));

            var query = _context.Briefs.Where(b => b.UsuarioId == usuarioId).AsQueryable();

            // Si onlybrief es TRUE y el usuario es admin, mostrar TODOS los proyectos
            // Si onlybrief es FALSE, mostrar solo los del usuario (sin importar el rol)
            if (onlybrief && isAdmin)
            {
                query = _context.Briefs.AsQueryable();
            }
            else if (!onlybrief)
            {
                // Forzar que solo muestre los proyectos del usuario actual
                query = _context.Briefs.Where(b => b.UsuarioId == usuarioId).AsQueryable();
            }

            return query.OrderByDescending(b => b.FechaRegistro).ToList();
        }

        /// <summary>
        /// Obtiene un Brief por su ID, incluyendo información del usuario relacionado.
        /// </summary>
        /// <param name="id">ID del Brief.</param>
        /// <returns>El Brief con su información, o null si no existe.</returns>
        public Brief GetById(int id)
        {
            return _context.Briefs
                .Include(b => b.Usuario)
                .FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Inserta un nuevo Brief en la base de datos.
        /// </summary>
        /// <param name="entity">El objeto Brief a insertar.</param>
        public void Insert(Brief entity)
        {
            _context.Briefs.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Actualiza un Brief existente con nuevos valores.
        /// </summary>
        /// <param name="entity">El objeto Brief con valores actualizados.</param>
        public void Update(Brief entity)
        {
            var existingEntity = _context.Briefs.Find(entity.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene todos los EstatusBrief activos.
        /// </summary>
        /// <returns>Enumerable de EstatusBrief activos.</returns>
        public IEnumerable<EstatusBrief> GetAllEstatusBrief()
        {
            return _context.EstatusBriefs
                .Where(e => e.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene todos los TipoBrief activos.
        /// </summary>
        /// <returns>Enumerable de TipoBrief activos.</returns>
        public IEnumerable<TipoBrief> GetAllTipoBrief()
        {
            return _context.TiposBrief
                .Where(t => t.Activo)
                .ToList();
        }

        /// <summary>
        /// Inserta un Proyecto. Si ya existe un Proyecto con el mismo BriefId, lo actualiza.
        /// </summary>
        /// <param name="entity">El objeto Proyecto a insertar o actualizar.</param>
        public void InsertProyecto(Proyecto entity)
        {
            var proyecto = _context.Proyectos
                .FirstOrDefault(p => p.BriefId == entity.BriefId);

            if (proyecto != null)
            {
                proyecto.BriefId = entity.BriefId;
                proyecto.Comentario = entity.Comentario;
                proyecto.RequierePlan = entity.RequierePlan;
                proyecto.Estado = entity.Estado;
                proyecto.FechaPublicacion = entity.FechaPublicacion;
                proyecto.FechaModificacion = DateTime.Now;
                _context.Proyectos.Update(proyecto);
            }
            else
            {
                _context.Proyectos.Add(entity);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Inserta un nuevo Material en la base de datos.
        /// </summary>
        /// <param name="entity">El objeto Material a insertar.</param>
        public void InsertMaterial(Material entity)
        {
            _context.Materiales.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Inserta un Material con múltiples PCNs.
        /// </summary>
        /// <param name="entity">El Material a insertar.</param>
        /// <param name="pcnIds">Lista de IDs de PCN a asociar.</param>
        public void InsertMaterialConPCNs(Material entity, List<int> pcnIds)
        {
            // Agregar el material primero
            _context.Materiales.Add(entity);
            _context.SaveChanges();

            // Luego agregar las relaciones MaterialPCN
            if (pcnIds != null && pcnIds.Any())
            {
                foreach (var pcnId in pcnIds)
                {
                    _context.MaterialPCN.Add(new MaterialPCN
                    {
                        MaterialId = entity.Id,
                        PCNId = pcnId
                    });
                }
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene un Proyecto asociado al Brief especificado por su ID.
        /// </summary>
        /// <param name="id">ID del Brief.</param>
        /// <returns>El objeto Proyecto asociado, o null si no existe.</returns>
        public Proyecto GetProyectoByBriefId(int id)
        {
            return _context.Proyectos
                .FirstOrDefault(p => p.BriefId == id);
        }

        /// <summary>
        /// Obtiene todos los Materiales asociados a un Brief por su ID.
        /// </summary>
        /// <param name="id">ID del Brief.</param>
        /// <returns>Lista de Materiales asociados.</returns>
        public List<Material> GetMaterialesByBriefId(int id)
        {
            return _context.Materiales
                .Where(m => m.BriefId == id)
                .Include(m => m.Prioridad)
                .Include(m => m.MaterialPCNs)
                    .ThenInclude(mp => mp.PCN)
                .Include(m => m.Audiencia)
                .Include(m => m.Formato)
                .Include(m => m.Brief)
                .Include(m => m.EstatusMaterial)
                .ToList();
        }

        /// <summary>
        /// Elimina un Material por su ID.
        /// </summary>
        /// <param name="id">ID del Material a eliminar.</param>
        public void EliminarMaterial(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material != null)
            {
                _context.Materiales.Remove(material);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina un Participante por su ID.
        /// </summary>
        /// <param name="id">ID del Participante a eliminar.</param>
        public void EliminarParticipante(int id)
        {
            var participante = _context.Participantes.Find(id);
            if (participante != null)
            {
                _context.Participantes.Remove(participante);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene el conteo de proyectos agrupados por fechas específicas y estatus.
        /// </summary>
        /// <param name="UsuarioId">ID del usuario.</param>
        /// <returns>Objeto ConteoProyectos con los resultados.</returns>
        public ConteoProyectos ObtenerConteoProyectos(int UsuarioId)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == UsuarioId);

            IQueryable<Brief> briefsQuery;

            if (usuario != null && usuario.RolId == 1)
            {
                // Admin: ver todos los proyectos
                briefsQuery = _context.Briefs.AsQueryable();
            }
            else if (usuario != null && usuario.RolId == 3)
            {
                // Producción: solo proyectos donde es participante
                var briefIdsParticipante = _context.Participantes
                    .Where(p => p.UsuarioId == UsuarioId)
                    .Select(p => p.BriefId)
                    .ToList();

                briefsQuery = _context.Briefs.Where(b => briefIdsParticipante.Contains(b.Id));
            }
            else
            {
                // Solicitante: solo sus propios proyectos
                briefsQuery = _context.Briefs.Where(b => b.UsuarioId == UsuarioId);
            }

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfNextWeek = startOfWeek.AddDays(7);
            var endOfNextWeek = startOfNextWeek.AddDays(6);

            // Proyección inicial para evitar múltiples consultas
            var briefs = briefsQuery
                .Select(b => new
                {
                    b.Id,
                    b.EstatusBriefId,
                    b.FechaPublicacion,
                    b.FechaEntrega,
                    b.UsuarioId
                }).ToList();

            var conteoProyectos = new ConteoProyectos
            {
                Hoy = briefs.Count(b => b.EstatusBriefId == 1 && b.FechaPublicacion.Date == today),
                EstaSemana = briefs.Count(b =>
                    b.EstatusBriefId == 1 &&
                    b.FechaPublicacion.Date >= startOfWeek &&
                    b.FechaPublicacion.Date <= today),
                ProximaSemana = briefs.Count(b =>
                    b.EstatusBriefId == 1 &&
                    b.FechaPublicacion.Date >= startOfNextWeek &&
                    b.FechaPublicacion.Date <= endOfNextWeek),
                TotalProyectos = briefs.Count
            };

            var briefIds = briefs.Select(b => b.Id).ToList();

            // Obtener proyectos finalizados (EstatusBriefId == 6)
            var proyectosFinalizados = briefs
                .Where(b => b.EstatusBriefId == 6)
                .ToList();

            // ProyectoTiempo: Proyectos finalizados que se entregaron en tiempo (FechaPublicacion <= FechaEntrega)
            conteoProyectos.ProyectoTiempo = proyectosFinalizados
                .Count(p => p.FechaPublicacion <= p.FechaEntrega);

            // ProyectoExtra: Proyectos finalizados que se entregaron fuera de tiempo (FechaPublicacion > FechaEntrega)
            conteoProyectos.ProyectoExtra = proyectosFinalizados
                .Count(p => p.FechaPublicacion > p.FechaEntrega);

            return conteoProyectos;
        }

        /// <summary>
        /// Obtiene el conteo de materiales agrupados por fechas y estatus.
        /// </summary>
        /// <param name="UsuarioId">ID del usuario.</param>
        /// <returns>Objeto ConteoProyectos con los resultados.</returns>
        public ConteoProyectos ObtenerConteoMateriales(int UsuarioId)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == UsuarioId);

            IQueryable<Material> materialesQuery;

            if (usuario != null && usuario.RolId == 1)
            {
                // Admin: ver todos los materiales
                materialesQuery = _context.Materiales
                    .Include(m => m.Brief)
                    .AsQueryable();
            }
            else if (usuario != null && usuario.RolId == 3)
            {
                // Producción: solo materiales de proyectos donde es participante
                var briefIdsParticipante = _context.Participantes
                    .Where(p => p.UsuarioId == UsuarioId)
                    .Select(p => p.BriefId)
                    .ToList();

                materialesQuery = _context.Materiales
                    .Include(m => m.Brief)
                    .Where(m => briefIdsParticipante.Contains(m.BriefId))
                    .AsQueryable();
            }
            else
            {
                // Solicitante: solo materiales de sus propios briefs
                materialesQuery = _context.Materiales
                    .Include(m => m.Brief)
                    .Where(m => m.Brief.UsuarioId == UsuarioId)
                    .AsQueryable();
            }

            var materiales = materialesQuery.ToList();

            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfNextWeek = startOfWeek.AddDays(7);
            var endOfNextWeek = startOfNextWeek.AddDays(6);

            return new ConteoProyectos
            {
                Hoy = materiales.Count(m => m.FechaRegistro.Date == today),
                EstaSemana = materiales.Count(m => m.FechaRegistro.Date >= startOfWeek && m.FechaRegistro.Date <= today),
                ProximaSemana = materiales.Count(m => m.FechaRegistro.Date >= startOfNextWeek && m.FechaRegistro.Date <= endOfNextWeek),
                TotalProyectos = materiales.Count
            };
        }

        /// <summary>
        /// Obtiene el conteo de proyectos cuya fecha de entrega es mayor o igual a la fecha de publicación.
        /// </summary>
        /// <param name="UsuarioId">ID del usuario.</param>
        /// <returns>Cantidad de proyectos que cumplen con la condición.</returns>
        public int ObtenerConteoProyectoFecha(int UsuarioId)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == UsuarioId);

            if (usuario != null && usuario.RolId == 1)
            {
                // Admin: contar todos los proyectos
                return _context.Briefs.Count(b => b.FechaEntrega >= b.FechaPublicacion);
            }
            else if (usuario != null && usuario.RolId == 3)
            {
                // Producción: contar solo proyectos donde es participante
                var briefIdsParticipante = _context.Participantes
                    .Where(p => p.UsuarioId == UsuarioId)
                    .Select(p => p.BriefId)
                    .ToList();

                return _context.Briefs
                    .Where(b => briefIdsParticipante.Contains(b.Id) && b.FechaEntrega >= b.FechaPublicacion)
                    .Count();
            }
            else
            {
                // Solicitante: contar solo sus propios proyectos
                return _context.Briefs
                    .Where(b => b.UsuarioId == UsuarioId && b.FechaEntrega >= b.FechaPublicacion)
                    .Count();
            }
        }

        /// <summary>
        /// Obtiene todos los materiales relacionados con un usuario, incluyendo datos de prioridades y estatus.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Lista de Materiales asociados.</returns>
        public List<Material> GetMaterialesByUser(int id)
        {
            var usuario = _context.Usuarios.Where(q => q.Id == id).FirstOrDefault();

            IQueryable<Material> query;

            if (usuario != null && usuario.RolId == 1)
            {
                // Admin: ver todos los materiales
                query = _context.Materiales;
            }
            else if (usuario != null && usuario.RolId == 3)
            {
                // Producción: solo materiales de proyectos donde es participante
                var briefIdsParticipante = _context.Participantes
                    .Where(p => p.UsuarioId == id)
                    .Select(p => p.BriefId)
                    .ToList();

                query = _context.Materiales.Where(m => briefIdsParticipante.Contains(m.BriefId));
            }
            else
            {
                // Solicitante: solo materiales de sus propios briefs
                query = _context.Materiales.Where(q => q.Brief.UsuarioId == id);
            }

            var materiales = query
                .Include(m => m.Prioridad)
                .Include(m => m.MaterialPCNs)
                    .ThenInclude(mp => mp.PCN)
                .Include(m => m.Audiencia)
                .Include(m => m.Formato)
                .Include(m => m.Brief)
                    .ThenInclude(b => b.EstatusBrief)
                .Include(m => m.Brief)
                    .ThenInclude(b => b.TipoBrief)
                .Include(m => m.EstatusMaterial)
                .ToList();

            return materiales;
        }

        /// <summary>
        /// Obtiene un material por su ID, incluyendo relaciones como prioridades y estatus.
        /// </summary>
        /// <param name="id">ID del material.</param>
        /// <returns>El Material solicitado, o null si no existe.</returns>
        public Material GetMaterial(int id)
        {
            return _context.Materiales
                .Where(m => m.Id == id)
                .Include(m => m.Prioridad)
                .Include(m => m.MaterialPCNs)
                    .ThenInclude(mp => mp.PCN)
                .Include(m => m.Audiencia)
                .Include(m => m.Formato)
                .Include(m => m.Brief)
                .Include(m => m.EstatusMaterial)
                .FirstOrDefault();
        }

        /// <summary>
        /// Filtra materiales según los criterios especificados en el objeto Material.
        /// </summary>
        /// <param name="material">Objeto Material con los criterios de filtro.</param>
        /// <returns>Lista de Materiales filtrados.</returns>
        public List<Material> GetMaterialesFilter(Material material)
        {
            var usuario = _context.Usuarios.Where(u => u.Id == material.Id).FirstOrDefault();

            IQueryable<Material> query;

            if (usuario != null && usuario.RolId == 1)
            {
                // Admin: ver todos los materiales
                query = _context.Materiales.AsQueryable();
            }
            else if (usuario != null && usuario.RolId == 3)
            {
                // Producción: solo materiales de proyectos donde es participante
                var briefIdsParticipante = _context.Participantes
                    .Where(p => p.UsuarioId == material.Id)
                    .Select(p => p.BriefId)
                    .ToList();

                query = _context.Materiales.Where(m => briefIdsParticipante.Contains(m.BriefId)).AsQueryable();
            }
            else
            {
                // Solicitante: solo materiales de sus propios briefs
                query = _context.Materiales.Where(m => m.Brief.UsuarioId == material.Id).AsQueryable();
            }

            return query
                .Where(m =>
                    m.Nombre == material.Nombre &&
                    m.FechaRegistro == material.FechaRegistro)
                .Include(m => m.Prioridad)
                .Include(m => m.MaterialPCNs)
                    .ThenInclude(mp => mp.PCN)
                .Include(m => m.Audiencia)
                .Include(m => m.Formato)
                .Include(m => m.Brief)
                    .ThenInclude(b => b.EstatusBrief)
                .Include(m => m.Brief)
                    .ThenInclude(b => b.TipoBrief)
                .Include(m => m.EstatusMaterial)
                .OrderByDescending(m => m.FechaEntrega)
                .ToList();
        }

        /// <summary>
        /// Obtiene todas las Audiencias activas.
        /// </summary>
        /// <returns>Enumerable de Audiencias activas.</returns>
        public IEnumerable<Audiencia> GetAllAudiencias()
        {
            return _context.Audiencia
                .Where(a => a.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene todos los Formatos activos.
        /// </summary>
        /// <returns>Enumerable de Formatos activos.</returns>
        public IEnumerable<Formato> GetAllFormatos()
        {
            return _context.Formato
                .Where(f => f.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene todos los PCN activos.
        /// </summary>
        /// <returns>Enumerable de PCN activos.</returns>
        public IEnumerable<PCN> GetAllPCN()
        {
            return _context.PCN
                .Where(p => p.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene todas las Prioridades activas.
        /// </summary>
        /// <returns>Enumerable de Prioridades activas.</returns>
        public IEnumerable<Prioridad> GetAllPrioridades()
        {
            return _context.Prioridad
                .Where(p => p.Activo)
                .ToList();
        }

        /// <summary>
        /// Obtiene el conteo de materiales agrupados por estatus.
        /// </summary>
        /// <param name="UsuarioId">ID del usuario.</param>
        /// <returns>Objeto ConteoMateriales con los resultados.</returns>
        public ConteoMateriales ObtenerConteoEstatusMateriales(int UsuarioId)
        {
            var usuarioAdmin = _context.Usuarios.Where(q => q.Id == UsuarioId && (q.RolId == 1 || q.RolId == 3)).FirstOrDefault();
            var materiales = _context.Materiales.Where(q => q.Brief.UsuarioId == UsuarioId).ToList();
            ConteoMateriales conteoMateriales = new ConteoMateriales();
            if (usuarioAdmin != null)
            {
                materiales = _context.Materiales.ToList();
            }

            conteoMateriales.Registros = materiales.Count();
            conteoMateriales.Revision = materiales.Where(q => q.EstatusMaterialId == 1).Count();
            conteoMateriales.FaltaInfo = materiales.Where(q => q.EstatusMaterialId == 2).Count();
            conteoMateriales.Aprobado = materiales.Where(q => q.EstatusMaterialId == 3).Count();
            conteoMateriales.Programado = materiales.Where(q => q.EstatusMaterialId == 4).Count();
            conteoMateriales.Entregado = materiales.Where(q => q.EstatusMaterialId == 5).Count();
            conteoMateriales.InicioCiclo = materiales.Where(q => q.EstatusMaterialId == 6).Count();


            return conteoMateriales;
        }

        /// <summary>
        /// Obtiene todos los EstatusMaterial activos.
        /// </summary>
        /// <returns>Enumerable de EstatusMaterial activos.</returns>
        public IEnumerable<EstatusMaterial> GetAllEstatusMateriales()
        {
            return _context.EstatusMateriales
                .Where(e => e.Activo)
                .ToList();
        }

        /// <summary>
        /// Actualiza el historial de un material y cambia su estatus. Si el estatus es 4 o 5, actualiza también el Brief.
        /// </summary>
        /// <param name="historialMaterial">Objeto HistorialMaterial con los datos a actualizar.</param>
        public void ActualizaHistorialMaterial(HistorialMaterial historialMaterial)
        {
            var material = _context.Materiales.FirstOrDefault(m => m.Id == historialMaterial.MaterialId);

            if (material != null)
            {
                // Si se proporciona una nueva fecha de entrega, actualizar el material
                if(historialMaterial.FechaEntrega != null && historialMaterial.FechaEntrega.HasValue)
                {
                    if (material.FechaEntrega != historialMaterial.FechaEntrega.Value)
                    {
                        material.FechaEntrega = historialMaterial.FechaEntrega.Value;
                        _context.Materiales.Update(material);
                    }
                }
                else
                {
                    // Si no se proporciona fecha, usar la fecha actual del material
                    historialMaterial.FechaEntrega = material.FechaEntrega;
                }

                // Actualizar Fecha de Publicación si se proporciona
                if (historialMaterial.FechaPublicacion != null && historialMaterial.FechaPublicacion.HasValue)
                {
                    material.FechaPublicacion = historialMaterial.FechaPublicacion.Value;
                    _context.Materiales.Update(material);
                }

                // Actualizar el estado de liberación de fecha de publicación
                material.FechaPublicacionLiberada = historialMaterial.FechaPublicacionLiberada;

                material.EstatusMaterialId = historialMaterial.EstatusMaterialId;
                _context.HistorialMateriales.Add(historialMaterial);

                if (historialMaterial.EstatusMaterialId == 4 || historialMaterial.EstatusMaterialId == 5)
                {
                    var briefs = _context.Materiales
                        .Where(m => m.BriefId == material.BriefId && m.EstatusMaterialId != historialMaterial.EstatusMaterialId)
                        .ToList();

                    if (!briefs.Any())
                    {
                        var brief = _context.Briefs.FirstOrDefault(b => b.Id == material.BriefId);
                        if (brief != null)
                        {
                            brief.EstatusBriefId = historialMaterial.EstatusMaterialId;
                            _context.Briefs.Update(brief);
                        }
                    }
                }

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Agrega un registro de retraso para un material.
        /// </summary>
        /// <param name="retrasoMaterial">Objeto RetrasoMaterial a agregar.</param>
        public void ActualizaRetrasoMaterial(RetrasoMaterial retrasoMaterial)
        {
            _context.RetrasoMateriales.Add(retrasoMaterial);
            _context.SaveChanges();
        }

        /// <summary>
        /// Obtiene el historial de un material por su ID.
        /// </summary>
        /// <param name="MaterialId">ID del material.</param>
        /// <returns>Enumerable de objetos HistorialMaterial.</returns>
        public IEnumerable<HistorialMaterial> GetAllHistorialMateriales(int MaterialId)
        {
            return _context.HistorialMateriales
                .Where(h => h.MaterialId == MaterialId)
                .Include(h => h.Usuario)
                    .ThenInclude(u => u.UserRol)
                .OrderByDescending(h => h.Id)
                .ToList();
        }
        

    }
}
