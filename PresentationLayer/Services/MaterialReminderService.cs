using BusinessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PresentationLayer.Services
{
    /// <summary>
    /// Servicio en background que ejecuta recordatorios diarios para materiales
    /// que tienen fecha de entrega al día siguiente
    /// </summary>
    public class MaterialReminderService : IHostedService, IDisposable
    {
        private readonly ILogger<MaterialReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer? _timer;

        public MaterialReminderService(
            ILogger<MaterialReminderService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MaterialReminderService iniciado.");

            // Calcular el tiempo hasta la próxima ejecución (hoy a las 9:00 AM o mañana a las 9:00 AM)
            var now = DateTime.Now;
            var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);

            if (now > scheduledTime)
            {
                // Si ya pasó las 9:00 AM, programar para mañana
                scheduledTime = scheduledTime.AddDays(1);
            }

            var firstRunDelay = scheduledTime - now;

            _logger.LogInformation($"Primer recordatorio programado para: {scheduledTime:dd/MM/yyyy HH:mm:ss}");

            // Ejecutar cada 24 horas a las 9:00 AM
            _timer = new Timer(
                DoWork,
                null,
                firstRunDelay,
                TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("Ejecutando verificación de recordatorios de materiales...");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataAccesContext>();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                    var toolsService = scope.ServiceProvider.GetRequiredService<IToolsService>();

                    // Obtener la fecha de mañana (solo la fecha, sin hora)
                    var tomorrow = DateTime.Now.Date.AddDays(1);
                    var dayAfterTomorrow = tomorrow.AddDays(1);

                    // Buscar materiales con fecha de entrega mañana
                    var materiales = context.Materiales
                        .Include(m => m.Brief)
                            .ThenInclude(b => b.Usuario)
                        .Where(m => m.FechaEntrega >= tomorrow && m.FechaEntrega < dayAfterTomorrow)
                        .ToList();

                    _logger.LogInformation($"Encontrados {materiales.Count} materiales con entrega mañana ({tomorrow:dd/MM/yyyy})");

                    foreach (var material in materiales)
                    {
                        try
                        {
                            // Preparar los valores dinámicos para la plantilla
                            var valoresDinamicos = new Dictionary<string, string>()
                            {
                                { "nombreProyecto", material.Brief?.Nombre ?? "Sin proyecto" },
                                { "nombreMaterial", material.Nombre },
                                { "fechaEntrega", material.FechaEntrega.ToString("dd/MM/yyyy") },
                                { "responsable", material.Responsable ?? "No asignado" },
                                { "link", $"https://adminproyectos.entersys.mx/Materiales?filtroNombre={material.Nombre}" }
                            };

                            // Preparar la lista de destinatarios
                            var destinatarios = new List<string>();

                            // Agregar el correo del creador del brief
                            if (material.Brief?.Usuario?.Correo != null)
                            {
                                destinatarios.Add(material.Brief.Usuario.Correo);
                            }

                            // Agregar los correos de los usuarios de Producción (Rol 3)
                            var usuariosProduccion = toolsService.GetUsuarioByRol(3);
                            destinatarios.AddRange(usuariosProduccion.Where(u => u.Estatus).Select(u => u.Correo));

                            // Eliminar duplicados
                            destinatarios = destinatarios.Distinct().ToList();

                            if (destinatarios.Any())
                            {
                                // Enviar el correo
                                emailSender.SendEmail(destinatarios, "ReminderEntregaMaterial", valoresDinamicos);

                                _logger.LogInformation($"✅ Recordatorio enviado para material '{material.Nombre}' a {destinatarios.Count} destinatarios");
                            }
                            else
                            {
                                _logger.LogWarning($"⚠️ No se encontraron destinatarios para el material '{material.Nombre}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"❌ Error al enviar recordatorio para material '{material.Nombre}'");
                        }
                    }

                    if (materiales.Count == 0)
                    {
                        _logger.LogInformation("No hay materiales con entrega para mañana.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error general al ejecutar MaterialReminderService");
            }

            _logger.LogInformation("Verificación de recordatorios completada.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MaterialReminderService detenido.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
