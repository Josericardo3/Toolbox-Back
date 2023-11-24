using inti_model.usuario;
using inti_model.dboresponse;
using inti_model;
using Microsoft.AspNetCore.Components.Web;

namespace inti_repository.usuario
{
    public interface IUsuarioPstRepository
    {
        Task<ResponseUsuarioPst> GetUsuarioPst(int id);
        Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst);
        Task<string> UpdateUsuarioPst(UsuarioPstUpd usuariopst);
        Task<bool> DeleteUsuarioPst(int id);
        Task<UsuarioPstLogin> LoginUsuario(InputLogin objLogin);
        Task<int> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo);
        Task<IEnumerable<Usuario>> GetUsuariosxPst(string rnt);  
        Task<bool> GetPermiso(int usuario,int modelo);
        Task<IEnumerable<ResponseModuloUsuario>> GetPermisoPorPerfil(int idtipousuario);
        Task<UsserSettings> GetUserSettings(int id);
        Task<bool> UpdateUsserSettings(UsserSettings usserSettings, int id);
        Task<IEnumerable<dynamic>> GetPstRoles(int rnt);
        Task<bool> DeletePstRoles(int pst);
        Task<bool> UpdatePstRoles(PstRolesUpdateModel pstRolesUpdateModel);





    }
}
