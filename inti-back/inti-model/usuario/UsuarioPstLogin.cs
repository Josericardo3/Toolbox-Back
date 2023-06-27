using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPstLogin
    {
        public int ID_USUARIO { get; set; }
        public string? NIT { get; set; }
        public string? PASSWORD { get; set; }
        public string? CORREO { get; set; }
        public string? HoraLogueo { get; set; }
        public string? TokenAcceso { get; set; }
        public string? TokenRefresco { get; set; }

        public List<Permiso> Grupo { get; set; }
        public List<Permiso> SubGrupo { get; set; }
        public List<Permiso> permisoUsuario { get; set; }

        public UsuarioPstLogin()
        {
            Grupo = new List<Permiso>();
            SubGrupo = new List<Permiso>();
            permisoUsuario = new List<Permiso>();
        }
    }
}
