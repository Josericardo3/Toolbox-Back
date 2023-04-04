
using inti_model.auditoria;
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

        Task<IEnumerable<Asesor>> ListarAuditor(int idPst);
        Task<bool> InsertRespuestaAuditoria(RespuestaAuditoria respuestaAuditoria);


    }
}
