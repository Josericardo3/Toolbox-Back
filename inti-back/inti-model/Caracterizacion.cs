using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class Caracterizacion
    {
        public int idcaracterizaciondinamica { get; set; }
        public String? nombre { get; set; }
        public String? tipodedato { get; set; }
        public int idcategoriarnt { get; set; }
        public String? mensaje { get; set; }
        public String? codigo { get; set; }
        public String? Dependiente { get; set; }
        public String? desplegable_id { get; set; }
        public String? tablarelacionada { get; set; }
        public bool existe { get; set; }
        public List<DesplegableCaracterizacion>? desplegable { get; set; }
        public List<Municipios> municipios { get; set; }
        public String? relations { get; set; }
        public String? campo_local { get; set; }
        public bool requerido { get; set; }
        public String? values { get; set; }
        public bool activo { get; set; }
        public String ffcontext { get; set; }

        public Caracterizacion()
        {
            this.relations = "{}";
            this.desplegable = new List<DesplegableCaracterizacion>();
            this.municipios = new List<Municipios>();
        }
    }

}
