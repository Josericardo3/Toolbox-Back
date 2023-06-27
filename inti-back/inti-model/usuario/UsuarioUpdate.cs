using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioUpdate
    {
        public int ID_USUARIO { get; set; }
        public string? NOMBRE { get; set; }
        public string? CORREO { get; set; }
        public string? TELEFONO { get; set; }
        public string? PASSWORD { get; set; }
        public string? RNT { get; set; }
        public string? NIT { get; set; }
    }
}
