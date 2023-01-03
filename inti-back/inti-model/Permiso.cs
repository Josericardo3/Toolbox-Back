using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public int idtabla { get; set; }
        public int item { get; set; }
        public String? descripcion { get; set; }
        public int idusuariopst { get; set; }
    }
}
