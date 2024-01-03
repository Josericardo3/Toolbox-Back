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
        Task<int> UpdateMaeEncuestas(MaeEncuesta encuesta);
        Task<int> UpdateEstadoEncuesta(int id, int encuesta);
        Task<IEnumerable<ResponseEncuestaGeneral>> GetEncuestaGeneral(int idusuario);
        Task<IEnumerable<dynamic>> GetRespuestasEncuesta(int idEncuesta);

        Task<MaeEncuesta> GetEncuestaPregunta(int idEncuesta);
        Task<IEnumerable<ResponseEncuestaPorcentaje>> GetRespuestaPorcentaje(int id_encuesta);
        Task<bool> PostRespuestas(List<RespuestaEncuestas> respuestas);
        Task<bool> DeleteEncuesta(int idEncuesta);
        Task<bool> DeletePregunta(int idPregunta);
    }
}
