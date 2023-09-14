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

namespace inti_repository.noticia
{
    public interface INoticiaRepository
    {
        Task<IEnumerable<ResponseNoticia>> GetAllNoticias(string rnt, int idTipoUsuario);
        Task<ResponseNoticia> GetNoticia(int id);
        Task<ReturnInsertNoticia> InsertNoticia(InputNoticiaString noticia);
        Task<bool> UpdateNoticia(Noticia noticia);
        Task<bool> DeleteNoticia(int id);
        Task<bool> ActualizarNotificaciones();
        Task<IEnumerable<ResponseNotificacion>> GetNotificacionesUsuario(int idusuario);
        Task<IEnumerable<ResponseNotificacion>> GetHistorialNotificaciones(int idusuario);

    }
}
