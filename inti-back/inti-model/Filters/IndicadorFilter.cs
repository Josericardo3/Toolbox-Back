using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.Filters
{
    public class IndicadorFilter:BaseFilter
    {
        public int ID_NORMA {  get; set; }
        public int ID_PAQUETE { get; set; }
        public int? ID_USUARIO { get; set; }
    }
    public class IndicadorGraficoFilter
    {
        public string? ANIO {  get; set; }
        public int ID_PAQUETE { get; set; }
        public int ID_PROCESO{ get; set; }
        public int ID_INDICADOR { get; set; }
    }
}
