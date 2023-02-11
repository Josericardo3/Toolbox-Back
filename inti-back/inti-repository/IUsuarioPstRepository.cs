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
        Task<bool> DeleteUsuarioPst(int id);
        Task<ResponseCaracterizacion> GetResponseCaracterizacion(int id);
        Task<UsuarioPstLogin> LoginUsuario(string usuario, string Password, string Correo);
        Task<bool> InsertRespuestaCaracterizacion(RespuestaCaracterizacion respuestaCaracterizacion);
        Task<List<NormaTecnica>> GetNormaTecnica(int id);
        Task<ResponseDiagnostico> GetResponseDiagnostico(int idnorma, int idValorTituloFormulariodiagnostico, int idValorMaestroDiagnostico);
        Task<bool> InsertRespuestaDiagnostico(RespuestaDiagnostico respuestaDiagnostico);
        Task<bool> ValidarRegistroCorreo(string datoCorreo);
        Task<bool> ValidarRegistroTelefono(string datoTelefono);
        Task<bool> RegistrarEmpleadoPst(EmpleadoPst empleado);
        
        
    }
}
