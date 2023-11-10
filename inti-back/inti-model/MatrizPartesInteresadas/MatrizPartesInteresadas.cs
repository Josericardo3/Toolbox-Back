using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model.matrizpartesinteresadas
{
    public class MatrizPartesInteresadas
    {
        public int ID_MATRIZ_PARTES_INTERESADAS { get; set; }
        public int ID_INTERESADA { get; set; }
        public string? PARTE_INTERESADA { get; set; }
        public string? NECESIDAD { get; set; }
        public string? EXPECTATIVA { get; set; }
        public string? ESTADO_DE_CUMPLIMIENTO { get; set; }
        public string? OBSERVACIONES { get; set; }
        public string? ACCIONES_A_REALIZAR { get; set; }
        public string? RESPONSABLE { get; set; }
        public string? ESTADO_ABIERTO_CERRADO { get; set; }
        public int ESTADO_ACTIVO_INACTIVO { get; set; }
        public int ID_USUARIO { get; set; }
        public string? FECHA_EJECUCION { get; set; }
        public string? FECHA_REGISTRO { get; set; }
        public int MEJORA_CONTINUA { get; set; }
        public string? ID_RNT { get; set; }
        public int FK_ID_MEJORA_CONTINUA { get; set; }
        public int FK_ID_ACTIVIDAD { get; set; }
    }
}