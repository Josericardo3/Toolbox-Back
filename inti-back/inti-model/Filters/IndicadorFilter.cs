using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.Filters
{
    public class IndicadorFilter:BaseFilter
    {
        //public int ID_NORMA {  get; set; }
        public int ID_PAQUETE { get; set; }
        public int? ID_USUARIO { get; set; }
        public List<int> ID_NORMA { get; set; }=new List<int>();
    }
    public class IndicadorGraficoFilter
    {
        public string? ANIO {  get; set; }
        public int ID_PAQUETE { get; set; }
        public int ID_PROCESO{ get; set; }
        public int ID_INDICADOR { get; set; }
    }
    public class IndicadorMonitorizacionFilter : BaseFilter
    {
        //public int ID_NORMA {  get; set; }
        public List<int> ID_PAQUETE { get; set; }= new List<int>();
        public List<int> ID_CATEGORIA { get; set; } = new List<int>();
        public List<int> ID_SUBCATEGORIAS { get; set; } = new List<int>();

        public List<string> DEPARTAMENTOS { get; set; } = new List<string>();
        public List<string> MUNICIPIOS { get; set; } = new List<string>();
        public List<int> ID_PST { get; set; } = new List<int>();
        public int? ID_USUARIO { get; set; }
        public List<int> ID_NORMA { get; set; } = new List<int>();
    }
}
