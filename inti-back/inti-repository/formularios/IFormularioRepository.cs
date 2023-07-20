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
        Task <DataFormulario> GetFormulario (int ID_FORMULARIO, string RNT, int ID_USUARIO);
        Task<bool> PostFormulario(List<Formulario> formularios);
        Task<bool> DeleteFormulario(List<int> idformularios);

    }
}
