using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.kpis
{
    public class FuenteDato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_FUENTE_DATO {  get; set; }
        [Required]
        [MaxLength(255)]
        public string NOMBRE {  get; set; }
        [MaxLength(255)]
        public string DESCRIPCION { get; set; }
        [Required]
        [MaxLength(4)]
        public string CODIGO { get; set; }
    }
}
