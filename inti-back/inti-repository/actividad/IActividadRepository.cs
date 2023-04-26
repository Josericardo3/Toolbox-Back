﻿using inti_model.asesor;
using inti_model.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.actividad
{
    public interface IActividadRepository
    {
        Task<IEnumerable<ActividadesAsesor>> GetAllActividades(int idAsesor);
        Task<ActividadesAsesor> GetActividad(int id, int idAsesor);
        Task<bool> InsertActividad(ActividadesAsesor actividades);
        Task<bool> UpdateActividad(ActividadesAsesor actividades);
        Task<bool> DeleteActividad(int id, int idAsesor);
        Task<IEnumerable<Usuario>> ListarResponsable(string rnt);
        Task<bool> AsignarAvatar(UsuarioPst usuario);
        Task<bool> AsignarLogo(UsuarioPst usuario);


    }
}
