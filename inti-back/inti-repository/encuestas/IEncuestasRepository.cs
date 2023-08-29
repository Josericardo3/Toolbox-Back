using inti_model.encuesta;
using inti_model.dboresponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.encuestas
{
    public interface IEncuestasRepository
    {
        Task<int> PostMaeEncuestas(MaeEncuesta encuesta);

        Task<IEnumerable<ResponseEncuestaGeneral>> GetEncuestaGeneral();
        Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas);
    }
}
