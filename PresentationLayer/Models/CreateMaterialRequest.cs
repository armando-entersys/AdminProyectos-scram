using System;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class CreateMaterialRequest
    {
        public int BriefId { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public int PrioridadId { get; set; }
        public string Ciclo { get; set; }
        public List<int> PCNIds { get; set; } // Lista de PCN IDs
        public int AudienciaId { get; set; }
        public int FormatoId { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Responsable { get; set; }
        public string Area { get; set; }
    }
}
