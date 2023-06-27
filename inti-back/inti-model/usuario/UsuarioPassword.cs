using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPassword
    {
        public int ID_USUARIO { get; set; }
        public String? CORREO { get; set; }
        public String? PASSWORD { get; set; }
        public int ID_RECUPERACION { get; set; }
        public String? CODIGO_RECUPERACION { get; set; }
        public String? ENCRIPTACION { get; set; }

        public UsuarioPassword() {
            CORREO = "";
        }
    }
}
