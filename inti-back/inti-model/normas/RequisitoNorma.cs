using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.normas
{
    public class RequisitoNorma
    {
        public string? NORMA { get; set; }
        public string? NUMERAL { get; set; }
        public string? TITULO { get; set; }
        public bool ESTADO { get; set; }
    }
    public class Data1
    {
        public string? NOMBRE { get; set; }
        public string? ORDEN { get; set; }
        public List<Data>? DETALLE { get; set; }
    }
    public class Data
    {
        public string? NOMBRE { get; set; }
        public string? ORDEN { get; set; }
    }
    public class Requisito
    {
        public int ID_REQUISITO { get; set; }
        public int FK_ID_PROCESO { get; set; }
        public string? REQUISITO { get; set; }
        public string? EVIDENCIA { get; set; }
        public string? PREGUNTA { get; set; }
        public string? HALLAZGO { get; set; }
        public string? OBSERVACION { get; set; }
    }
}

