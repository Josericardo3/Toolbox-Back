using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.listachequeo
{
    public class CalifListaChequeo
    {
        public string? Numeral { get; set; }
        public string? tituloRequisito { get; set; }
        public string? Requisito { get; set; }
        public string? Evidencia { get; set; }
        public string? calificado { get; set; }
        public string? observacion { get; set; }
        public int idnormatecnica { get; set; }
        public int idusuario { get; set; }
        public string? valorcalificado { get; set; }
    }
}
