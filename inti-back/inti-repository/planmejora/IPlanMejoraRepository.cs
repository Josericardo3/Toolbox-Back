using inti_model.planmejora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.planmejora
{
    public interface IPlanMejoraRepository
    {
        Task<ResponseArchivoPlanMejora> GetResponseArchivoPlanMejora(int idnorma, int idusuario, int idValorTituloListaChequeo, int idValorSeccionListaChequeo, int idValordescripcionCalificacion);
    }
}
