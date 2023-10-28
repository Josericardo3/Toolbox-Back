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
    public class Indicador { 
   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_INDICADOR { get; set; }

        [Required]
        [MaxLength(255)]
        public string TITULO { get; set; }

        
        [MaxLength(255)]
        public string? DESCRIPCION { get; set; }

        [MaxLength(500)]
        public string FORMULA_TEXT { get; set; }

        
        public string FORMULA_HTML { get; set; }

        [Required]
        public int ID_OBJETIVO { get; set; }
        [ForeignKey(nameof(ID_OBJETIVO))]
        public Objetivo Objetivo { get; set; }

        [Required]
        public int ID_PERIODO_MEDICION { get; set; }
        [ForeignKey(nameof(ID_PERIODO_MEDICION))]
        public PeriodoMedicion PeriodoMedicion { get; set; }

        public int ID_PAQUETE { get; set; }
        [ForeignKey(nameof(ID_PAQUETE))]
        public Paquete Paquete { get; set; }

        [Required]
        public int ID_USUARIO_CREA { get; set; }
        [ForeignKey(nameof(ID_USUARIO_CREA))]
        public Usuario Usuario { get; set; }
        public int? ID_USUARIO_MODIFICA { get; set; }

        [Required]
        public DateTime FECHA_CREACION { get; set; }

        public DateTime? FECHA_MODIFICACION { get; set; }

        public DateTime? FECHA_ELIMINACION { get; set; }
    

    }
}
