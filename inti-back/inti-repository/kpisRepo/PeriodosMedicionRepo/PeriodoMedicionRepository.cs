﻿using inti_model.Base;
using inti_model.Filters;
using inti_model.kpis;
using inti_model;
using inti_repository.Base;
using inti_repository.kpisRepo.VariablesRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.kpisRepo.PeriodosMedicionRepo
{
    public class PeriodoMedicionRepository : RepoBase<PeriodoMedicion>, IPeriodoMedicionRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        public PeriodoMedicionRepository(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }


        public async Task<BaseComboDTO<BaseInformacionComboDTO>> ListarPeriodosCombo(BaseFilter baseFilter)
        {
            var response = new BaseComboDTO<BaseInformacionComboDTO>();
            try
            {
                
                var query = await Context.PeriodosMedicion.OrderByDescending(x => x.ID_PERIODO_MEDICION).Select(x => new BaseInformacionComboDTO
                {
                    Id = x.ID_PERIODO_MEDICION,
                    Nombre = x.NOMBRE

                }).ToListAsync();


                response.Data = query;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de las variables";
                response.Exception = ex.Message;
            }
            return response;
        }
    }
}