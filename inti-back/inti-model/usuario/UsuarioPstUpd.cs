using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Clase generada para la actualización de datos del UsuarioPST.

namespace inti_model.usuario
{
    public class UsuarioPstUpd
    {
        public int FK_ID_USUARIO { get; set; }
        public String? NIT { get; set; }
        public String? RNT { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public int FK_ID_SUB_CATEGORIA_RNT { get; set; }
        public String? RAZON_SOCIAL_PST { get; set; }
        public String? CORREO_PST { get; set; }
        public String? TELEFONO_PST { get; set; }
        public String? NOMBRE_REPRESENTANTE_LEGAL { get; set; }
        public String? APELLIDO_REPRESENTANTE_LEGAL { get; set; }
        public String? DEPARTAMENTO { get; set; }
        public String? MUNICIPIO { get; set; }
        public int FK_ID_TIPO_AVATAR { get; set; }

    }
}
