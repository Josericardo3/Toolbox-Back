using inti_model.dboinput;
using inti_model.matrizpartesinteresadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.matrizpartesinteresadas
{
    public interface IMatrizPartesInteresadasRepository
    {
        Task<IEnumerable<MatrizPartesInteresadas>> Get(int id);
        Task<int> Create(InputMatrizPartesInteresadas ParteInteresadas);
        Task<bool> Update(MatrizPartesInteresadas UpdatePartesInteresadas);
        Task<bool> DeletePartesInteresadas(int id);
    }
}
