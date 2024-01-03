using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.ViewModels
{
    public class IndicadorViewModel
    {
        public string TITULO { get; set; }
        public string? DESCRIPCION { get; set; }
        public string FORMULA_TEXT { get; set; }
        public string FORMULA_HTML { get; set; }
        public int ID_OBJETIVO { get; set; }
        public int ID_PERIODO_MEDICION { get; set; }
        public int ID_USUARIO_CREA { get; set; }
        public int ID_PAQUETE { get; set; }
        //public int? ID_NORMA { get; set; }
        public List<int> VARIABLES { get; set; }
        public List<int> ID_NORMA { get; set; }

    }
    
    public class IndicadorDeletaViewModel
    {
        public int ID_INDICADOR { get; set; }
        public int ID_USUARIO_CREA { get; set; }
        public int ID_PAQUETE { get; set; }
        public List<int> ID_NORMA { get; set; } = new List<int>();
    }
    public class DetalleEvaluacionViewModel
    {
        public int ID_INDICADOR { get; set; }
        public List<int> ID_NORMA { get; set; }=new List<int>();
        public int ID_OBJETIVO { get; set; }
        public int ID_USUARIO_CREA { get; set; }
        public int ID_PERIODO_MEDICION { get; set; }
        public int ID_FUENTE_DATO { get; set; }
        public int META {  get; set; }
        public bool? ES_INCREMENTO { get; set; }
        public List<ProcesosViewModel> Procesos { get; set; } = new List<ProcesosViewModel>();
    }
    public class ProcesosViewModel
    {
        public int ID_PROCESO { get; set; }
        public int ID_USUARIO_ASIGNADO { get; set; }
        public string NOMBRE_PROCESO { get; set; }
    }

    public class RegistroEvaluacionViewModel
    {
        public int ID_EVALUACION_INDICADOR { get; set; }
        public float RESULTADO { get; set; }
        public string ESTADO { get; set; }
        public int ID_ACCION { get; set; }
        public string? ANALISIS { get; set; }
        public bool ENVIO_CORREO { get; set; }
        public List<VariablesEvaluacionViewModel> VARIABLES_EVALUACION { get; set; } = new List<VariablesEvaluacionViewModel>();
        public string? NORMA { get; set; }
    }
    public class VariablesEvaluacionViewModel
    {
        public int ID_VARIABLE_EVALUACION_INDICADOR { get; set; }
        public int ID_VARIABLE { get; set; }
        public float VALOR { get; set; }
    }
    public class RecordatorioAddViewModel
    {
        public int ID_EVALUACION_INDICADOR { get; set; }
        public string FECHA_RECORDATORIO  { get; set; }
        public string HORA_RECORDATORIO { get; set; }
        public bool ENVIO_CORREO { get; set; }
    }
}

