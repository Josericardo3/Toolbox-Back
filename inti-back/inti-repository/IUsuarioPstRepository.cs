using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;


namespace inti_repository
{
    public interface IUsuarioPstRepository
    {
        Task<IEnumerable<UsuarioPst>> GetAllUsuariosPst();
        Task<UsuarioPst> GetUsuarioPst(int id);
        Task<bool> InsertUsuarioPst(UsuarioPst usuariopst);
        Task<String> UpdateUsuarioPst(UsuarioPstUpd usuariopst);
        Task<bool> DeleteUsuarioPst(int  id);
        Task<UsuarioPstLogin> LoginUsuario(string usuario, string Password);
    }
}
