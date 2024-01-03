
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseEncuestaGeneral
    {
        public int ID_MAE_ENCUESTA { get; set; }
        public string? TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public int NUM_ENCUESTADOS { get; set; }
        public int ESTADO_HABILITADO { get; set; }
        public DateTime FECHA_REG { get; set; }
        public DateTime FECHA_ACT { get; set; }

    }

}