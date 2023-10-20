using inti_model.mejoracontinua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.mejoracontinua
{
    public interface IMejoraContinuaRepository
    {
        Task<IEnumerable<MejoraContinua>> Get(int id);
        Task<int> Create(MejoraContinua entity);
        Task<bool> Update(MejoraContinua entity);
        Task<bool> Delete(int id);
    }
}
