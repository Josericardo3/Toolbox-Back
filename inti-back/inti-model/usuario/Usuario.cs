using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class Usuario
    {
        [Key]
        public int ID_USUARIO { get; set; }
        public string? NOMBRE { get; set; }
        public string? CORREO { get; set; }
        public string? TELEFONO { get; set; }
        public string? PASSWORD { get; set; }
        public string? RNT { get; set; }
        public string? NIT { get; set; }
        public int FK_ID_PST { get; set; }
        public int FK_ID_ASESOR { get; set; }
        public bool ESTADO { get; set; }
        public int ID_TIPO_USUARIO { get; set; }
        public int ID_CARGO { get; set; }


    }

    public class UsserSettings
    {
        public string? NOMBRE_REPRESENTANTE_LEGAL { get; set; }
        public string? CORREO_REPRESENTANTE_LEGAL { get; set; }
        public string? TELEFONO_REPRESENTANTE_LEGAL { get; set; }
        public string? PAGINA_WEB { get; set; }
        public string? INSTAGRAM { get; set; }
        public string? TWITTER { get; set; }
        public string? FACEBOOK { get; set; }
        public string? OTROS { get; set; }
    }

    public class PstRolesUpdateModel
    {
        public string CORREO { get; set; }
        public string NOMBRE { get; set; }
        public int ID_CARGO { get; set; }
        public int ID_PST_ROLES { get; set; }
    }



}
