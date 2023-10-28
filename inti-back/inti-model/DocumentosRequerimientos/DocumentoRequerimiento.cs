using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.DocumentosRequerimientos
{
    public class DocumentoRequerimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_DOCUMENTO_REQUERIMIENTO { get; set; }
     
        public string NOMBRE_REQUERIMIENTO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        
        public string RNT { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public Guid IDENTIFICADOR { get; set; }
        public string NOMBRE_DOCUMENTO { get; set; }
        public string NOMBRE_ADJUNTO { get; set; }
        

    }
}
