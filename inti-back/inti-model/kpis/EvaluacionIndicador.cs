using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.usuario;

namespace inti_model.kpis
{
    public class EvaluacionIndicador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_EVALUACION_INDICADOR {  get; set; }
        [Required]
        public int ID_PROCESO {  get; set; }
        [ForeignKey(nameof(ID_PROCESO))]
        public Proceso Proceso { get; set; }

        [Required]
        public int  ID_INDICADOR { get; set; }
        [ForeignKey(nameof(ID_INDICADOR))]
        public Indicador Indicador { get; set; }
        [Required]
        public int ID_FUENTE_DATO { get; set; }
        [ForeignKey(nameof(ID_FUENTE_DATO))]
        public FuenteDato FuenteDato { get; set; }
        [Required]
        public int  ID_PERIODO_MEDICION { get; set; }
        [ForeignKey(nameof(ID_PERIODO_MEDICION))]
        public PeriodoMedicion PeriodoMedicion { get; set; }
        [Required]
        public int ID_OBJETIVO { get; set; }
        [ForeignKey(nameof(ID_OBJETIVO))]
        public Objetivo Objetivo { get; set; }
        [Required]
        public int ID_USUARIO_CREA { get; set; }
        [ForeignKey(nameof(ID_USUARIO_CREA))]
        public Usuario Usuario { get; set; }

        [Required]
        public int ID_USUARIO_ASIGNADO { get; set; }
        [ForeignKey(nameof(ID_USUARIO_ASIGNADO))]
        public Usuario UsuarioAsignado { get; set; }

        [Required]
        [MaxLength(150)]
        public string PERIODO { get; set; }
        public float? RESULTADO { get; set; }
        public int META { get; set; }
        
        [MaxLength(150)]
        public string? ESTADO { get; set; }
       
        [MaxLength(800)]
        public string? ANALISIS  {  get; set; }
        /*public int ID_IDENTIFICADOR_FORMULA { get; set; }
        [ForeignKey(nameof(ID_IDENTIFICADOR_FORMULA))]
        public IdentificadorFormula IdentificadorFormula { get; set; }*/
        public int? ID_ACCION { get; set; }
        [ForeignKey(nameof(ID_ACCION))]
        public Accion Accion { get; set; }
        public DateTime FECHA_INICIO_MEDICION { get; set; }
        public DateTime FECHA_FIN_MEDICION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        
        public DateTime? FECHA_MODIFICACION { get; set; }
        public string FORMULA { get; set; }
        public DateTime? FECHA_RECORDATORIO { get; set; }
        public TimeSpan? HORA_RECORDATORIO { get; set; }
    }
}
