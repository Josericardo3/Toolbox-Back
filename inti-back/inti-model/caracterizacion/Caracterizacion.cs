using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.caracterizacion
{
    public class Caracterizacion
    {
        public int idcaracterizaciondinamica { get; set; }
        public string? nombre { get; set; }
        public string? tipodedato { get; set; }
        public int idcategoriarnt { get; set; }
        public string? mensaje { get; set; }
        public string? codigo { get; set; }
        public string? Dependiente { get; set; }
        public string? desplegable_id { get; set; }
        public string? tablarelacionada { get; set; }
        public bool existe { get; set; }
        public List<DesplegableCaracterizacion>? desplegable { get; set; }
        public List<Municipios> municipios { get; set; }
        public string? relations { get; set; }
        public string? campo_local { get; set; }
        public bool requerido { get; set; }
        public string? values { get; set; }
        public bool activo { get; set; }
        public string ffcontext { get; set; }

        public Caracterizacion()
        {
            relations = "{}";
            desplegable = new List<DesplegableCaracterizacion>();
            municipios = new List<Municipios>();
        }
    }

}
