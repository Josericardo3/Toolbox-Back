using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class UsuarioPstLogin
    {
        public int IdUsuarioPst { get; set; }
        public int IdAsesor { get; set; }
        public string? nit { get; set; }
        public string? Password { get; set; }
        public string? Correo { get; set; }
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
