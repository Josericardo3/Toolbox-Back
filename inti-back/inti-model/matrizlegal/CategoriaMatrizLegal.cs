using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizlegal
{
    public class CategoriaMatrizLegal
    {
        public List<MatrizUsuario> USUARIO { get; set; }
        public String? CATEGORIA { get; set; }
        public List<DocumentoMatrizLegal>? DOCUMENTO { get; set; }
        
        public CategoriaMatrizLegal()
        {
            DOCUMENTO = new List<DocumentoMatrizLegal>();
            USUARIO = new List<MatrizUsuario>();
        }
    }

}

