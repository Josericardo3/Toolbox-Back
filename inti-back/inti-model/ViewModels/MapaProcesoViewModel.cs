using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.ViewModels
{
    public class MapaProcesoViewModel
    {
        public int ID_USUARIO_CREA {  get; set; }
        public string RNT { get; set; }
        public MapaDiagramaProcesoViewModel PROCESOS_DIAGRAMA {  get; set; }
    }
    public class MapaDiagramaProcesoViewModel
    {
        public List<MapaDiagramaDetalleProcesoViewModel> ESTRATEGICOS {  get; set; }
        public List<MapaDiagramaDetalleProcesoViewModel> MISIONALES { get; set; }
        public List<MapaDiagramaDetalleProcesoViewModel> APOYO { get; set; }
    }
    public class MapaDiagramaDetalleProcesoViewModel
    {
        public int ID_DETALLE { get; set; }
        public int ID_MAPA_PROCESO {  get; set; }
        public int ID_USUARIO { get; set; }
        public int ORDEN {  get; set; }
        public string TIPO { get; set; }
        public int id { get; set; }


    }
    public class DeleteDetalleProcesoViewModel
    {
        public int ID_DETALLE { get; set; }
        public string RNT { get; set; }
        public int ID_USUARIO { get; set; }
        


    }

}
