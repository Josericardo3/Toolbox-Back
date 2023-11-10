namespace inti_model.matrizlegal
{
    public class RespuestaMatrizLegal
    {
        public int FK_ID_USUARIO { get; set; }
        public int FK_ID_MATRIZ_LEGAL { get; set; }
        public String? ESTADO_CUMPLIMIENTO { get; set; }
        public int? ID_RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? RESPONSABLE_CUMPLIMIENTO { get; set; }
        public String? DATA_CUMPLIMIENTO { get; set; }
        public List<PlanIntervencion>? PLAN_INTERVENCION { get; set; }        
        
        public RespuestaMatrizLegal()
        {
            PLAN_INTERVENCION = new List<PlanIntervencion>();
        }
        public int FK_ID_MEJORA_CONTINUA { get; set; }
        public int FK_ID_ACTIVIDAD { get; set; }

        public bool ENVIO_MEJORA_CONTINUA { get; set; }
    }
}
