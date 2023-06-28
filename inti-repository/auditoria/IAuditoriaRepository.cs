using inti_model.auditoria;
using inti_model.usuario;
using inti_model.dboinput;
using inti_model.caracterizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_model.dboresponse;

namespace inti_repository.auditoria
{
    public interface IAuditoriaRepository
    {

        Task<IEnumerable<ResponseAuditor>> ListarAuditor(string rnt);
        Task<Auditoria> GetAuditoria(int id);

        Task<bool> InsertPlanAuditoria(InputPlanAuditoria auditoria);
        Task<bool> UpdatePlanAuditoria(InputPlanAuditoria auditoria);
        Task<bool> InsertVerificacionAuditoria(InputVerficacionAuditoria proceso);
        Task<bool> UpdateVerificacionAuditoria(InputVerficacionAuditoria proceso);
        Task<bool> UpdateInformeAuditoria(InputInformeAuditoria proceso);
        Task<bool> DeleteAuditoria(int id_auditoria);
        Task<AuditoriaNorma> GetTituloNormaAuditoria(int id_norma);
        Task<IEnumerable<ResponseAuditorias>> ListarAuditorias(int idPst);
        Task<bool> DeleteRequisitoAuditoria(int id_auditoria);


    }
}