using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.usuario
{
    public class ResponseUsuario
    {
        public int FK_ID_USUARIO { get; set; }
        public String? NIT { get; set; }
        public String? RNT { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public int FK_ID_SUB_CATEGORIA_RNT { get; set; }
        public String? NOMBRE_PST { get; set; }
        public String? RAZON_SOCIAL_PST { get; set; }
        public String? CORREO_PST { get; set; }
        public String? TELEFONO_PST { get; set; }
        public String? NOMBRE_REPRESENTANTE_LEGAL { get; set; }
        public String? CORREO_REPRESENTANTE_LEGAL { get; set; }
        public String? TELEFONO_REPRESENTANTE_LEGAL { get; set; }
        public int FK_ID_TIPO_IDENTIFICACION { get; set; }
        public String? IDENTIFICACION_REPRESENTANTE_LEGAL { get; set; }
        public String? DEPARTAMENTO { get; set; }
        public String? MUNICIPIO { get; set; }
        public String? NOMBRE_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public String? CORREO_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public String? TELEFONO_RESPONSABLE_SOSTENIBILIDAD { get; set; }
        public int FK_ID_TIPO_AVATAR { get; set; }
        public bool ESTADO { get; set; }
        public String? CREATED { get; set; }
        public String? CATEGORIA_RNT { get; set; }
        public String? SUB_CATEGORIA_RNT { get; set; }
        public String? TIPO_IDENTIFICACION { get; set; }
        public String? AVATAR { get; set; }
        public bool ACTIVO { get; set; }
        public object this[String? name]
        {
            get
            {
                var properties = typeof(ResponseUsuario)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.Name == name && property.CanRead)

                        return property.GetValue(this, null);

                }

                throw new ArgumentException("Can't find property");

            }

        }


    }

}
