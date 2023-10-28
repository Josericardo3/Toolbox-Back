using inti_model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.DTOs
{
    public class IndicadorDTO
    {
        public int ID_INDICADOR { get; set; }
        public string TITULO { get; set; }
        public string DESCRIPCION { get; set; }
        public string FORMULA_TEXT { get; set; }
        public string FORMULA_HTML { get; set; }
        public int ID_OBJETIVO { get; set; }
        public string TITULO_OBJETIVO { get; set; }
        public int ID_PERIODO_MEDICION { get; set; }
        public string NOMBRE_PERIODO { get; set; }
        public int ID_USUARIO_CREA { get; set; }
        public int ID_PAQUETE { get; set; }
        public string NOMBRE_PAQUETE { get; set; }
        public int ID_NORMA { get; set; }
        public int CANTIDAD_ASIGNACIONES { get; set; }
    }

    public class IndicadorEvaluacionDTO
    {
        public int ID_EVALUACION_INDICADOR { get; set; }
        public int ID_INDICADOR { get; set; }
        public string TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public string FORMULA_TEXT { get; set; }
        public string FORMULA_TEXT_VISTA { get; set; }
        public string FORMULA_HTML { get; set; }
        public int ID_OBJETIVO { get; set; }
        public string TITULO_OBJETIVO { get; set; }
        public int ID_PERIODO_MEDICION { get; set; }
        public string NOMBRE_PERIODO { get; set; }
        public int ID_USUARIO_CREA { get; set; }
        public string? NOMBRE_RESPONSABLE { get; set; }
        public int ID_PAQUETE { get; set; }
        public string NOMBRE_PAQUETE { get; set; }
        public string NOMBRE_PROCESO { get; set; }
        public int ID_NORMA { get; set; }
        public string FECHA_PERIODO { get; set; }
        public float META { get; set; }
        public string? ESTADO {  get; set; }
        public string? ANALISIS { get; set; }
        public string? ACCION { get; set; }
        public string? RESULTADO { get; set; }
        public bool TIENE_RECORDATORIO { get; set; }
        public string? FECHA_RECORDATORIO { get; set; }
        public List<VariablesEvaluacionDTO> VARIABLES_EVALUACION { get; set; }
        public string SEMAFORIZACION { get; set; }
    }
    public class VariablesEvaluacionDTO
    {
        public int ID_VARIABLE_EVALUACION_INDICADOR { get; set; }
        public int ID_VARIABLE { get; set; }
        public string NOMBRE { get; set; }
        public string VALOR { get; set; }
    }
    public class SemaforizacionDTO
    {
        public int VALOR_SEM {  get; set; }
        public string NOMBRE_SEM { get; set; }
        public string COLOR { get; set; }
    }

    public class GraficoPorProceso
    {
        public string NOMBRE_PROCESO { get; set; }
        public List<EvaluacionesDTO> PROCESOS_EVALUACION { get; set; }
    }
   

    public class GraficoIndicadoresPorProceso
    {
        public string NOMBRE_INDICADOR { get; set; }
        public List<GraficoEvaluacionIndicadorDTO> PROCESOS_EVALUACION { get; set; }
    }
    public class GraficoEvaluacionIndicadorDTO
    {
        public string NOMBRE_PROCESO { get; set; }
        public List<EvaluacionesDTO> EVALUACIONES { get; set; }
    }
    public class EvaluacionesDTO
    {
        public string PERIODO{ get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN {  get; set; }
        public float? RESULTADO { get; set; }
        public float META { get; set; }
    }
    public class RecordatorioIndicadorDTO
    {
        public int ID_INDICADOR { get; set; }
        public int ID_EVALUACION_INDICADOR { get; set; }
        public string NOMBRE_INDICADOR { get; set; }
        public string FORMULA_TEXT { get; set; }
        public string NOMBRE_PERIODO { get; set; }
        public string NOMBRE_PAQUETE { get; set; }
        

    }

}
