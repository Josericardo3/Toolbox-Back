
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboinput
{
    public class InputActividad
    {
        public int FK_ID_USUARIO_PST { get; set; }
        public int FK_ID_RESPONSABLE { get; set; }
        public string? TIPO_ACTIVIDAD { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? FECHA_INICIO { get; set; }
        public string? FECHA_FIN { get; set; }
        public string? ESTADO_PLANIFICACION { get; set; }
        public bool ENVIO_CORREO { get; set; } = false;
    }

}