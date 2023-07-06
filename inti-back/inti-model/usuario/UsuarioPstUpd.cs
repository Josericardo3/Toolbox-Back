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

        public string? NIT { get; set; }

        public string? RNT { get; set; }

        public int FK_ID_CATEGORIA_RNT { get; set; }

        public int FK_ID_SUB_CATEGORIA_RNT { get; set; }

        public string? NOMBRE_PST { get; set; }

        public string? RAZON_SOCIAL_PST { get; set; }

        public string? CORREO_PST { get; set; }

        public string? TELEFONO_PST { get; set; }

        public string? NOMBRE_REPRESENTANTE_LEGAL { get; set; }

        public string? CORREO_REPRESENTANTE_LEGAL { get; set; }

        public string? TELEFONO_REPRESENTANTE_LEGAL { get; set; }

        public int FK_ID_TIPO_IDENTIFICACION { get; set; }

        public string? IDENTIFICACION_REPRESENTANTE_LEGAL { get; set; }

        public String? DEPARTAMENTO { get; set; }

        public String? MUNICIPIO { get; set; }

        public string? NOMBRE_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        public string? CORREO_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        public string? TELEFONO_RESPONSABLE_SOSTENIBILIDAD { get; set; }

        public int FK_ID_TIPO_AVATAR { get; set; }

    }
}
