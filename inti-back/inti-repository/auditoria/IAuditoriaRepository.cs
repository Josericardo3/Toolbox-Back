
using inti_model.auditoria;
using inti_model.asesor;
using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.matrizlegal;

namespace inti_repository.auditoria
{
    public interface IAuditoriaRepository
    {
 
        Task<IEnumerable<Asesor>> ListarAuditor();
        Task<bool> InsertRespuestaAuditoria(RespuestaAuditoria respuestaAuditoria);
        Task<IEnumerable<Auditoria>> GetResponseAuditoria(string tipo);



    }
}
