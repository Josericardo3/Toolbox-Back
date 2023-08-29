using inti_model.encuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.encuesta
{
    public class RespuestaEncuestas
    {
        public int FK_ID_MAE_ENCUESTA_PREGUNTA { get; set; }
        public string RESPUESTA { get; set; }
    }
}