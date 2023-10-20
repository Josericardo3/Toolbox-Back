using inti_model.encuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.encuesta
{
    public class PreguntaEncuesta
    {
        public int ID_MAE_ENCUESTA_PREGUNTA { get; set; }
        public int FK_MAE_ENCUESTA { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? TIPO { get; set; }
        public string? VALOR { get; set; }
        public bool OBLIGATORIO { get; set; }
    }
}