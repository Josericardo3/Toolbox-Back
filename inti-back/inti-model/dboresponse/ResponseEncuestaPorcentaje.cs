using inti_model.formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseEncuestaPorcentaje
    {
        public int ID_MAE_ENCUESTA_PREGUNTA { get; set; }
        public int FK_MAE_ENCUESTA { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? TIPO { get; set; }
        public string? VALOR { get; set; }
        //public List<RespuestaPregunta> RESPUESTAS { get; set; }
        public List<ResumenPregunta> RESUMEN { get; set; }
        public ResponseEncuestaPorcentaje()
        {
            //RESPUESTAS = new List<RespuestaPregunta>();
            RESUMEN = new List<ResumenPregunta>();
        }
    }
    public class ResumenPregunta
    {
        public String ITEM { get; set; }
        public int N_RESPUESTAS { get; set; }
        public String RESPUESTA { get; set; }
        public float PORCENTAJE { get; set; }
    }
    public class RespuestaPregunta
    {
        public int ID_RESPUESTA_ENCUESTA { get; set; }
        public int NUM_ENCUESTADO { get; set; }
        public String RESPUESTA { get; set; }
        public int ESTADO { get; set; }
    }
}
