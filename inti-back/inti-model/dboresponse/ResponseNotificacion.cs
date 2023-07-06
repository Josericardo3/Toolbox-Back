using System;


namespace inti_model.dboresponse
{
    public class ResponseNotificacion
    {
        public int FK_ID_ACTIVIDAD{ get; set; }
        public string? DESCRIPCION_ACTIVIDAD { get; set; }
        public string? FECHA_INICIO_ACTIVIDAD { get; set; }
        public string? FECHA_FIN_ACTIVIDAD { get; set; }
        public int FK_ID_NOTICIA { get; set; }
        public string? NOMBRE_FIRMA { get; set; }
        public string? TITULO_NOTICIA { get; set; }
        public string? DESCRIPCION_NOTICIA { get; set; }
        public string? IMAGEN_NOTICIA { get; set; }
        public DateTime FECHA_REG_NOTICIA { get; set; }
        public DateTime FECHA_ACT_NOTICIA { get; set; }
        public string? TIPO { get; set; }
        public bool ESTADO { get; set; }
        public string? COD_IMAGEN { get; set; }


    }

}
