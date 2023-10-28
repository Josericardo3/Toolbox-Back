using inti_model;
using inti_model.Base;
using inti_model.DocumentosRequerimientos;
using inti_model.DTOs;
using inti_model.Filters;
using inti_model.kpis;
using inti_model.ViewModels;
using inti_repository.Base;
using inti_repository.kpisRepo.Indicadores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.DocumentoRequerimientoRepo
{
    public class DocumentoRequerimientoRepository : RepoBase<DocumentoRequerimiento>, IDocumentoRequerimientoRepository
    {
        private BaseHelpers baseHelpers = new BaseHelpers();
        private readonly IConfiguration _configuration;
        private readonly string dominio;
        public DocumentoRequerimientoRepository(DbContextOptions<IntiDBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            dominio = _configuration.GetSection("DominioApi").Value;
        }

        public async Task<BaseResponseDTO> GuardarDocumentoRequerimiento(DocumentoRequerimientoViewModel model)
        {
            var response= new BaseResponseDTO();
            try
            {
               
                if (model.ADJUNTO == null)
                {
                    response.Mensaje = "Sin Archivo";
                    return response;
                }
                var id = Guid.NewGuid();

                var normbreArchivo = id + model.TIPO_AJUNTO;


                var filePath = Path.Combine($"wwwroot/requerimientos//{model.RNT}//documentos");

                var existePath = Directory.Exists(filePath);
                if (!existePath)
                {
                    Directory.CreateDirectory(filePath);
                }
                var rutaArchivo = Path.Combine(filePath, normbreArchivo);

                File.WriteAllBytesAsync(rutaArchivo, Convert.FromBase64String(model.ADJUNTO.Split(',')[1]));
                var objArchivo = new DocumentoRequerimiento
                {
                    NOMBRE_DOCUMENTO = model.NOMBRE_DOCUMENTO,
                    NOMBRE_REQUERIMIENTO = model.NOMBRE_REQUERIMIENTO,
                    RNT = model.RNT,
                    TIPO_DOCUMENTO = model.TIPO_DOCUMENTO,
                    NOMBRE_ADJUNTO = normbreArchivo,
                    FECHA_CREACION = baseHelpers.DateTimePst(),
                    IDENTIFICADOR= id
                };
                Context.DocumentosRequerimientos.Add(objArchivo);
                await Context.SaveChangesAsync();
                response.Confirmacion = true;
                response.Mensaje = "Documento Cargado Correctamente";
               
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
                response.Mensaje = "No se puede realizar la carga del documento";
               
            }
            return response;
        }
        public async Task<BaseComboDTO<ArchivoInfoDTO>> ListarDocumentos(DocumentoRequisitoFilter filter)
        {
            var response = new BaseComboDTO<ArchivoInfoDTO>();
            try
            {
                
                var query = await Context.DocumentosRequerimientos.Where(x=>x.NOMBRE_REQUERIMIENTO == filter.NOMBRE_REQUERIMIENTO && x.RNT == filter.RNT)
                    .OrderByDescending(x => x.FECHA_CREACION  ).Skip(0).Take(1).ToListAsync();

                if(query.Count<1)
                {
                    response.Mensaje = "No tiene documentos cargados";
                   return response;
                }
                var respuesta = query.Select(x => new ArchivoInfoDTO
                {
                    NOMBRE_ARCHIVO=x.NOMBRE_DOCUMENTO,
                    URL=Path.Combine($"{dominio}/requerimientos/{x.RNT}/documentos/{x.NOMBRE_ADJUNTO}")//ObtenerRuta(x.RNT,x.NOMBRE_ADJUNTO)
                });
                response.Data = respuesta;
                response.Confirmacion = true;
                response.Mensaje = "Lista Obtenida Correctamente";
            }
            catch (Exception ex)
            {
                response.Mensaje = "No se puede listar combo de las fuente Datos";
                response.Exception = ex.Message;
            }
            return response;
        }
        private string ObtenerRuta(string rnt, string nombreArchivo)
        {
            var archivob64 = "";
            var ruta_archivo = $"{Path.Combine($"requerimientos/{rnt}/documentos/{nombreArchivo}")}";
            
            if (System.IO.File.Exists(ruta_archivo))
            {
                var archivoBytes = System.IO.File.ReadAllBytes(ruta_archivo);
                var base64Image = Convert.ToBase64String(archivoBytes);
                archivob64 = base64Image;
            }
            else
            {
                return "";
            }
            return archivob64;
                
        }
    }
}
