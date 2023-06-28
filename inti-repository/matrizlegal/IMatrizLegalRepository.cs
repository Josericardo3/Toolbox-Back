﻿using inti_model.matrizlegal;
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
        Task<bool> InsertLey(InputMatrizLegal oMatrizLegal);
        Task<bool> RespuestaMatrizLegal(RespuestaMatrizLegal respuestaMatrizLegal);
        List<CategoriaMatrizLegal> ArchivoMatrizLegal(int IdDocumento, int idUsuario);


    }
}