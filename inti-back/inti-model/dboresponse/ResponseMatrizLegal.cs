
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.dboresponse
{
    public class ResponseMatrizLegal
    {
        public int ID_MATRIZ { get; set; }
        public int ID_DOCUMENTO { get; set; }
        public string? CATEGORIA { get; set; }
        public string TIPO_NORMATIVIDAD { get; set; }
        public string NUMERO { get; set; }
        public string ANIO { get; set; }
        public string EMISOR { get; set; }
        public string DESCRIPCION { get; set; }
        public string DOCS_ESPECIFICOS { get; set; }
        public int FK_ID_USUARIO { get; set; }
        public string? ESTADO_CUMPLIMIENTO { get; set; }
        public int? ID_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public string? RESPONSABLE_CUMPLIMIENTO { get; set; }
        public string? DATA_CUMPLIMIENTO { get; set; }
        public string? PLAN_ACCIONES_A_REALIZAR { get; set; }
        public int? ID_PLAN_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public string? PLAN_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public string? PLAN_FECHA_EJECUCION { get; set; }
        public string? PLAN_ESTADO { get; set; }
        public bool ESTADO { get; set; }
        public int ID_USUARIO_REG { get; set; }
        public DateTime FECHA_REG { get; set; }
        public int ID_USUARIO_ACT { get; set; }
        public DateTime FECHA_ACT { get; set; }



    }
}