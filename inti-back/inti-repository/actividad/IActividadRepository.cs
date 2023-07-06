using inti_model.asesor;
using inti_model.usuario;
using inti_model.actividad;
using inti_model.dboresponse;
using inti_model.dboinput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.actividad
{
    public interface IActividadRepository
    {
        Task<IEnumerable<ResponseActividad>> GetAllActividades(int idAsesor);
        Task<ResponseActividad> GetActividad(int id);
        Task<bool> InsertActividad(InputActividad actividades);
        Task<bool> UpdateActividad(Actividad actividades);
        Task<bool> DeleteActividad(int id);
        Task<IEnumerable<ResponseActividadResponsable>> ListarResponsable(string rnt);
        Task<IEnumerable<Avatar>> ListarAvatar();

        Task<bool> AsignarAvatar(int idusuariopst, int idavatar);
        Task<bool> AsignarLogo(UsuarioLogo usuario);
        Task<bool> ActualizarActividades();


    }
}
