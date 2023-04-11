using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.auditoria
{
    public class Auditoria
    {
        public int ID_AUDITORIA_DINAMICA { get; set; }
        public string? NOMBRE { get; set; }
        public string? TIPO_FORMULARIO { get; set; }
        public string? TIPO_DATO { get; set; }
        public string? DEPENDIENTE { get; set; }
        public string? DESPLEGABLE_ID { get; set; }
        public string? TABLA_RELACIONADA { get; set; }
        public string? CAMPO_LOCAL { get; set; }
        public bool REQUERIDO { get; set; }
        public bool ESTADO { get; set; }

    }

}