using inti_model.usuario;
using inti_model.dboresponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.validaciones
{
    public interface IValidacionesRepository
    {
        bool ValidarRegistroCorreo(string datoCorreo);
        bool ValidarRegistroTelefono(string datoTelefono);
        Task<bool?> ValidarUsuarioCaracterizacion(int idUsuario);
        Task<ResponseValidacionDiagnostico> ValidarUsuarioDiagnostico(int idUsuarioPst, int idNorma);
        bool ValidarUsuarioRnt(string rnt);
        Task<UsuarioPassword> RecuperacionContraseña(string correo);
        Task<bool> UpdatePassword(string password, string id);
        Task<IActionResult> SendEmail2(string correo);
    }
}
