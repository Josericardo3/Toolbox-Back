
using inti_model.auditoria;
using inti_model.usuario;
using inti_model.asesor;
using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.auditoria
{
    public interface IAuditoriaRepository
    {

        Task<IEnumerable<Usuario>> ListarAuditor(string rnt);
        Task<Auditoria> GetAuditoria(int id);
        Task<bool> InsertAuditoria(Auditoria auditoria);
        Task<bool> UpdateAuditoria(Auditoria auditoria);

        Task<ResponseAuditoria> GetResponseAuditoria(string tipo, int idAuditoria);
        Task<IEnumerable<Auditoria>> ListarAuditorias(int idPst);


    }
}