using inti_model.usuario;
using inti_model.dboresponse;


namespace inti_repository.usuario
{
    public interface IUsuarioPstRepository
    {
        Task<ResponseUsuarioPst> GetUsuarioPst(int id);
        Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst);
        Task<string> UpdateUsuarioPst(UsuarioPstUpd usuariopst);
        Task<bool> DeleteUsuarioPst(int id);
        Task<UsuarioPstLogin> LoginUsuario(string usuario, string Password, string Correo);
        Task<int> RegistrarEmpleadoPst(int id, string nombre, string correo, int idcargo);
        Task<IEnumerable<Usuario>> GetUsuariosxPst(string rnt);  
        Task<bool> GetPermiso(int usuario,int modelo);  

    }
}
