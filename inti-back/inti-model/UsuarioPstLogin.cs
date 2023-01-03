using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class UsuarioPstLogin
    {
        public int IdUsuarioPst { get; set; }
        public int IdUsuarioPst { get; set; }
        public String? nit { get; set; }
        public String? Password { get; set; }
        public String? HoraLogueo { get; set; }
        public String? TokenAcceso { get; set; }
        public String? TokenRefresco { get; set; }

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
