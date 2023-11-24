using Dapper;
using inti_model.normas;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository.requisitosnorma
{
    public class RequisitosNormasRepository : IRequisitosNormasRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public RequisitosNormasRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<List<Data1>> GetResponseRequisitosNormas(int idnorma)
        {
            var db = dbConnection();
            var estructuraList = new List<Data1>();
            var requisitos = new List<Data>();

            if (idnorma == 10)
            {
                var query1 = @"
              SELECT
            DESCRIPCION AS ORDEN,
            VALOR AS NOMBRE
              FROM
            MaeGeneral
              WHERE
            ITEM = 10 AND ID_TABLA = 22 ";
              var data1 = await db.QueryAsync<Data>(query1);
                requisitos = data1.ToList();
            }
            else if (idnorma == 3)
            {
                var query1 = @"
              SELECT
            DESCRIPCION AS ORDEN,
            VALOR AS NOMBRE
              FROM
            MaeGeneral
              WHERE
            ITEM = 3 AND ID_TABLA = 23 ";
              var data1 = await db.QueryAsync<Data>(query1);
                requisitos = data1.ToList();
            }
            else if (idnorma == 1)
            {
                var query1 = @"
              SELECT
            DESCRIPCION AS ORDEN,
            VALOR AS NOMBRE
              FROM
            MaeGeneral
              WHERE
            ITEM = 1 AND ID_TABLA = 24 ";
              var data1 = await db.QueryAsync<Data>(query1);
                requisitos = data1.ToList();
            }
            else
            {

                var query1 = @"
              SELECT
            DESCRIPCION AS ORDEN,
            VALOR AS NOMBRE
              FROM
            MaeGeneral
              WHERE
            ITEM = 0 AND ID_TABLA = 21 ";
              var data1 = await db.QueryAsync<Data>(query1);
                requisitos = data1.ToList();

            }

            var query = @"
          SELECT
          TITULO AS NOMBRE,
          NUMERAL AS ORDEN
          FROM
          MaeRequisitoNorma
          WHERE
          NORMA = @NORMA AND ESTADO = 1 ";
            var parameterNorma = new
            {
                NORMA = idnorma
            };
            var data = await db.QueryAsync<Data>(query, parameterNorma);

            var requisitosNormaList = data.ToList();

            foreach (var item in requisitos)
            {
                var estructura = new Data1
                {
                    DETALLE = new List<Data>()
                };

                foreach (var item1 in requisitosNormaList)
                {
                    if (item.ORDEN == item1.ORDEN)
                    {
                        estructura.DETALLE.Add(item1);
                    }
                }

                estructura.NOMBRE = item.NOMBRE;
                estructura.ORDEN = item.ORDEN;

                estructuraList.Add(estructura);
            }

            return estructuraList;
        }

        public async Task<IEnumerable<dynamic>> GetRequisitosNormas(int idnorma)
        {
            var db = dbConnection();

            var query = @"SELECT 
                            TITULO
                        FROM
                            MaeRequisitoNorma
                        WHERE
                            NORMA = @ID_NORMA";

            var result = await db.QueryAsync<dynamic>(query, new { ID_NORMA  = idnorma });

            return result.ToList();

        }

        public async Task<bool> UpdateRequisitoNorma(Requisito requisito)
        {
            var db = dbConnection();


                var query = @"UPDATE AuditoriaRequisito
                                SET REQUISITO = @REQUISITO,
                                EVIDENCIA = @EVIDENCIA,
                                PREGUNTA = @PREGUNTA,
                                HALLAZGO = @HALLAZGO,
                                OBSERVACION = @OBSERVACION
                                WHERE ID_REQUISITO = @ID_REQUISITO";

                var result = await db.ExecuteAsync(query, new { requisito.REQUISITO, requisito.EVIDENCIA, requisito.PREGUNTA, requisito.HALLAZGO, requisito.OBSERVACION, requisito.ID_REQUISITO });
            
            return result > 0;
        }
    }

}
