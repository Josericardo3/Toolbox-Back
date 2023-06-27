using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Permiso
    {
        public int ID_PERMISO { get; set; }
        public int ID_TABLA { get; set; }
        public int ITEM { get; set; }
        public String? DESCRIPCION { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int TIPO_USUARIO { get; set; }
    }
}
