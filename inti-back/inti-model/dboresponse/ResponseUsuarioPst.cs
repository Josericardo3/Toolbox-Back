using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseUsuarioPst
    {
        public int ID_PST { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public string? RNT { get; set; }
        public string? CATEGORIA_RNT { get; set; }
        public string? SUB_CATEGORIA_RNT { get; set; }

        public int FK_ID_SUB_CATEGORIA_RNT { get; set; }

        public string? NOMBRE_PST { get; set; }

        public string? RAZON_SOCIAL_PST { get; set; }

        public string? CORREO_PST { get; set; }

        public string? TELEFONO_PST { get; set; }
        public string? LOGO { get; set; }
        public string? NOMBRE { get; set; }
        public string? CORREO { get; set; }
        public int FK_ID_TIPO_AVATAR { get; set; }
        public int ID_TIPO_USUARIO { get; set; }
        public bool ESTADO { get; set; }

    }

}
