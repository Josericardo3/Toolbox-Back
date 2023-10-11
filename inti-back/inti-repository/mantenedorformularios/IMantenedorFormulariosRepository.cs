using inti_model.mantenedorformularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.mantenedorformularios
{
    public interface IMantenedorFormulariosRepository
    {
        Task<MantenedorFormularios> Get(int id);
        Task<int> Create(MantenedorFormularios entity);
        Task<bool> Update(MantenedorFormularios entity);
        Task<bool> Delete(int id);

    }
}
