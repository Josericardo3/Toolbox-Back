using inti_model.formulario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.formularios
{
    public interface IFormularioRepository
    {
        Task <IEnumerable<dynamic>> GetFormulario (int ID, string RNT);
        Task<bool> PostFormulario(IEnumerable<Formulario> formularios);

    }
}
