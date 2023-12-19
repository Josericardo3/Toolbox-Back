using inti_model.matrizlegal;
using inti_model.dboresponse;
using inti_model.dboinput;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.matrizlegal
{
    public interface IMatrizLegalRepository
    {
        Task<IEnumerable<ResponseMatrizLegal>> GetMatrizLegal(int IdDoc, int IdUsuario);
        Task<bool> InsertLey(List<InputMatrizLegal> oMatrizLegal);
        Task<bool> RespuestaMatrizLegal(RespuestaMatrizLegal respuestaMatrizLegal);
        Task<bool> RespuestaMatrizLegalResumen(RespuestaMatrizLegalResumen respuestaMatrizLegalResumen);
        List<CategoriaMatrizLegal> ArchivoMatrizLegal(int IdDocumento, int idUsuario);
        List<DatosHeader> GetDatosHeaderMatriz(String RNT);
        Task<bool> DeleteLey(int id);
    }
}