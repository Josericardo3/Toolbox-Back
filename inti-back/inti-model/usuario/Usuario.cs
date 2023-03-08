using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? password { get; set; }
        public string? rnt { get; set; }
        public string? nit { get; set; }

    }
}
