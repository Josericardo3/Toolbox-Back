using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.formulario
{
    public class DataFormulario
    {
        public int ID_USUARIO { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string MUNICIPIO { get; set; }
        public List<Formulario> RESPUESTAS { get; set; }
        public List<Formulario> RESPUESTA_GRILLA { get; set; }

        public DataFormulario()
        {
            RESPUESTAS = new List<Formulario>();
            RESPUESTA_GRILLA = new List<Formulario>();
        }

    }
    public class Formulario
    {
        public int ID_RESPUESTA_FORMULARIOS { get; set; }
        public int FK_MAE_FORMULARIOS { get; set; }
        public string PREGUNTA { get; set; }
        public int ORDEN { get; set; }
        public string RESPUESTA { get; set; }
        public int FK_USUARIO { get; set; }
    }
}
