using inti_model.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.validaciones
{
    public interface IValidacionesRepository
    {
        Task<bool> ValidarRegistroCorreo(string datoCorreo);
        Task<bool> ValidarRegistroTelefono(string datoTelefono);
        Task<UsuarioPassword> RecuperacionContraseña(string correo);
        Task<bool> UpdatePassword(string password, string id);
    }
}
