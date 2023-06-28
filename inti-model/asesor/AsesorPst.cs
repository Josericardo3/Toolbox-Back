using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.asesor
{
    public class AsesorPst
    {
        public int ID_ASESOR_PST { get; set; }
        public int FK_ID_ASESOR { get; set; }
        public int FK_ID_PST { get; set; }
        public int ESTADO { get; set; }


    }
}
