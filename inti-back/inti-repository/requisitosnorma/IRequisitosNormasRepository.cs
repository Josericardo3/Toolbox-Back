using inti_model.normas;
using inti_model.planmejora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.requisitosnorma
{
    public interface IRequisitosNormasRepository
    {
        Task<List<Data1>> GetResponseRequisitosNormas(int idnorma);

        Task<IEnumerable<dynamic>> GetRequisitosNormas(int idnorma);
        Task<bool> UpdateRequisitoNorma(Requisito requisito);

    }
}
