using inti_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
        Task<bool> RegistrarEmpleadoPst(int id, String correo, String rnt);
        Task<IEnumerable<PST_Asesor>> ListarPSTxAsesor(int idasesor, int idtablamaestro);
        Task<IEnumerable<ActividadesAsesor>> GetAllActividades(int idAsesor);
        Task<ActividadesAsesor> GetActividad(int id, int idAsesor);
        Task<bool> InsertActividad(ActividadesAsesor actividades);
        Task<bool> UpdateActividad(ActividadesAsesor actividades);
        Task<bool> DeleteActividad(int id, int idAsesor);
        Task<bool> RegistrarAsesor(Usuario objasesor);
        Task<bool> RegistrarPSTxAsesor(PST_AsesorUpdate objPST_Asesor);
        Task<bool> UpdateAsesor(UsuarioUpdate objAsesor);
        Task<UsuarioPassword> RecuperacionContraseña(String correo);
        Task<bool> UpdatePassword(String password, String id);
        Task<IEnumerable<Usuario>> ListAsesor();
    }
}
