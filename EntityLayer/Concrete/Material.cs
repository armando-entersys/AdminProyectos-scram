using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Material
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public int PrioridadId { get; set; }
        public Prioridad Prioridad { get; set; } // Propiedad de navegación

        public string Ciclo { get; set; }

        // Relación muchos a muchos con PCN a través de MaterialPCN
        public ICollection<MaterialPCN> MaterialPCNs { get; set; } = new List<MaterialPCN>();

        // Relación muchos a muchos con Audiencia a través de MaterialAudiencia
        public ICollection<MaterialAudiencia> MaterialAudiencias { get; set; } = new List<MaterialAudiencia>();

        public int FormatoId { get; set; }
        public Formato Formato { get; set; } // Propiedad de navegación

        public DateTime FechaEntrega { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public bool FechaPublicacionLiberada { get; set; } = false;
        public string Responsable { get; set; }
        public string Area { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; }

        // Llave foránea para relacionarse con Brief
        public int BriefId { get; set; }
        public Brief Brief { get; set; }

        // Llave foránea para relacionarse con EstatusMaterial
        public int EstatusMaterialId { get; set; }
        public EstatusMaterial EstatusMaterial { get; set; }

        // Relación de uno a muchos con HistorialMaterial
        public ICollection<HistorialMaterial> Historiales { get; set; } = new List<HistorialMaterial>();

        // Relación de uno a muchos con RetrasoMaterial
        public ICollection<RetrasoMaterial> Retrasos { get; set; } = new List<RetrasoMaterial>();

    }
}
