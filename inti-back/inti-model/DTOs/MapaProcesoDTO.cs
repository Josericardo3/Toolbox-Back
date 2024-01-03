using inti_model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.DTOs
{
    public class MapaProcesoDTO
    {
        public int ID_DETALLE { get; set; }
        public int ID_USUARIO_CREA { get; set; }
       public string RNT { get; set; }
        public int ID_MAPA_PROCESO { get; set; }
        public int ORDEN { get; set; }
        public int id {  get; set; }
        public string TIPO { get; set; }
        public string DESCRIPCION_PROCESO { get; set; }
        public string TIPO_PROCESO { get; set; }
    }
    
        public class MapaDiagramaProcesoDTO: BaseResponseDTO
    {
            
            public List<MapaProcesoDTO> ESTRATEGICOS { get; set; }
            public List<MapaProcesoDTO> MISIONALES { get; set; }
            public List<MapaProcesoDTO> APOYO { get; set; }
        }
      
        
    
}
