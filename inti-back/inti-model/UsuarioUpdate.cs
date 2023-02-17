using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class UsuarioUpdate
    {
        public int idUsuario { get; set; }
        public String? nombre { get; set; }
        public String? correo { get; set; }
        public String? telefono { get; set; }
        public String? password { get; set; }
        public String? rnt { get; set; }
        public String? nit { get; set; }
    }
}
