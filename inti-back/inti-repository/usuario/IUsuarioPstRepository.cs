using inti_model.usuario;


namespace inti_repository.usuario
{
    public interface IUsuarioPstRepository
    {
        Task<IEnumerable<UsuarioPst>> GetAllUsuariosPst();
        Task<UsuarioPst> GetUsuarioPst(int id);
        Task<bool> InsertUsuarioPst(UsuarioPstPost usuariopst);
        Task<string> UpdateUsuarioPst(UsuarioPstUpd usuariopst);
        Task<bool> DeleteUsuarioPst(int id);
        Task<UsuarioPstLogin> LoginUsuario(string usuario, string Password, string Correo);
        Task<int> RegistrarEmpleadoPst(int id, string correo, string rnt);

    }
}
