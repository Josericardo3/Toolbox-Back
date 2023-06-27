using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class Caracterizacion
    {
        public int ID_CARACTERIZACION_DINAMICA { get; set; }
        public string? NOMBRE { get; set; }
        public string? TIPO_DE_DATO { get; set; }
        public int FK_ID_CATEGORIA_RNT { get; set; }
        public string? MENSAJE { get; set; }
        public string? CODIGO { get; set; }
        public string? DEPENDIENTE { get; set; }
        public string? DESPLEGABLE_ID { get; set; }
        public string? TABLA_RELACIONADA { get; set; }
        public List<DesplegableCaracterizacion>? DESPLEGABLE { get; set; }
        public bool EXISTE { get; set; }
        public string? RELATIONS { get; set; }
        public string? CAMPO_LOCAL { get; set; }
        public bool REQUERIDO { get; set; }
        public string? VALUES { get; set; }
        public bool ESTADO { get; set; }
        public string? FFCONTEXT { get; set; }
        public Caracterizacion()
        {
            RELATIONS = "{}";
            DESPLEGABLE = new List<DesplegableCaracterizacion>();
        }


    }

}
