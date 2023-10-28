using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    
    public class VariableEvaluacionIndicador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_VARIABLE_EVALUACION_INDICADOR { get; set; }
        public int ID_EVALUACION_INDICADOR { get; set; }
        [ForeignKey(nameof(ID_EVALUACION_INDICADOR))]
        public EvaluacionIndicador EvaluacionIndicador { get; set; }

        public int ID_VARIABLE { get; set; }
        [ForeignKey(nameof(ID_VARIABLE))]
        public Variable Variable { get; set; }
        public int ID_INDICADOR { get; set; }
        [ForeignKey(nameof(ID_INDICADOR))]
        public Indicador Indicador { get; set; }
        public float VALOR {  get; set; }
    }
}
