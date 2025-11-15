using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MaterialAudiencia
    {
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public int AudienciaId { get; set; }
        public Audiencia Audiencia { get; set; }
    }
}
