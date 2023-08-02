
using inti_model;
using inti_model.dboresponse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace inti_repository.general
{
    public interface IGeneralRepository
    {
        Task<IEnumerable<Maestro>> ListarMaestros(int idtabla);
        Task<Maestro> GetMaestro(int idtabla, int item);
        Task <IEnumerable<dynamic>> GetNormas();
        Task<IEnumerable<ResponseResponsable>> ListarResponsable(string rnt);
        Task<IEnumerable<dynamic>> ListarCategorias();
        Task<IEnumerable<dynamic>> ListarPst();
        Task<bool> PostMonitorizacionUsuario(ResponseMonitorizacionUsuario data);

    }
}
