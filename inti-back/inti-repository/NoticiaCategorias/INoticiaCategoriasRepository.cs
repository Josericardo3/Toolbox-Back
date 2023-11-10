using inti_model.noticiaCategorias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.noticiaCategorias
{
    public interface INoticiaCategoriasRepository
    {
        Task<IEnumerable<NoticiaCategorias>> Get();
        Task<int> Create(NoticiaCategorias entity);
        Task<bool> Update(NoticiaCategorias entity);
        Task<bool> Delete(int id);
    }
}
