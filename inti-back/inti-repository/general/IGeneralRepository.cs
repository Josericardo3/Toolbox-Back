using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.general
{
    public interface IGeneralRepository
    {
        Task<IEnumerable<Maestro>> ListarMaestros(int idtabla);
        Task<Maestro> GetMaestro(int idtabla, int item);
            
    }
}
