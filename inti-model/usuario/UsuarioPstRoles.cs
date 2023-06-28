using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPstRoles
    {
        public int ID_PST_ROLES { get; set; }
        public int FK_ID_PST { get; set; }
        public string? RNT { get; set; }
        public string? CORREO { get; set; }
        public string? CARGO { get; set; }
        public string? NIT { get; set; }
        public bool ESTADO { get; set; }

    }
}
