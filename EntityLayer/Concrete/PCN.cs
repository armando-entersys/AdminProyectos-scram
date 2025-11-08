using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class PCN
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = false;

        // Relación muchos a muchos con Material a través de MaterialPCN
        public ICollection<MaterialPCN> MaterialPCNs { get; set; } = new List<MaterialPCN>();
    }
}
