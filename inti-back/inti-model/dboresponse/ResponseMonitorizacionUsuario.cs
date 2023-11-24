using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseMonitorizacionUsuario
    {
        public int FK_ID_USUARIO { get; set; }
        public string? TIPO { get; set; }
        public string? MODULO { get; set; }

        public DateTime FECHA_REG { get; set; }

    }

}
