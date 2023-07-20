using inti_model.encuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.encuestas
{
    public interface IEncuestasRepository
    {
        Task<bool> PostMaeEncuestas(List<MaeEncuesta> encuestas);

        Task<List<Encuesta>> GetEncuesta(int id);
        Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas);
    }
}
