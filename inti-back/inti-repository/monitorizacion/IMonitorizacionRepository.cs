using inti_model.noticia;
using inti_model.dboinput;
using inti_model.dboresponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inti_repository.noticia;
using Microsoft.AspNetCore.Mvc;

namespace inti_repository.monitorizacion
{
    public interface IMonitorizacionRepository
    {
        Task<IEnumerable<ResponseMonitorizacionIndicador>> GetAllMonitorizacionIndicador();
        Task<IEnumerable<dynamic>> GetContadorMonitorizacion();


    }
}
