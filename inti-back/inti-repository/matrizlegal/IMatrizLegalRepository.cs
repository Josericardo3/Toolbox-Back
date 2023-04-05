using inti_model.matrizlegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.matrizlegal
{
    public interface IMatrizLegalRepository
    {
        Task<IEnumerable<MatrizLegal>> GetMatrizLegal(int IdDoc);

    }
}
