using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MaterialPCN
    {
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public int PCNId { get; set; }
        public PCN PCN { get; set; }
    }
}
