using inti_model.asesor;
using inti_model.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.caracterizacion
{
    public interface IAsesorRepository
    {
        Task<int> RegistrarAsesor(Asesor objasesor);
        Task<bool> RegistrarPSTxAsesor(AsesorPstUpdate objPST_Asesor);
        Task<bool> UpdateAsesor(UsuarioUpdate objAsesor);
        Task<IEnumerable<Asesor>> ListAsesor();
        Task<IEnumerable<AsesorPst>> ListarPSTxAsesor(int idasesor, int idtablamaestro);
        Task<bool> CrearRespuestaAsesor(RespuestaAsesor objRespuestaAsesor);
    }
}
