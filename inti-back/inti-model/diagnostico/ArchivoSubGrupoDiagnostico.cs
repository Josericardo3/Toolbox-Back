
namespace inti_model.diagnostico
{
    public class ArchivoSubGrupoDiagnostico
    {
        public int idsubgrupo { get; set; }
        public int iddiagnosticodinamico { get; set; }
        public int idnormatecnica { get; set; }
        public string? numeralprincipal { get; set; }
        public string? numeralespecifico { get; set; }
        public string? titulo { get; set; }
        public string? requisito { get; set; }
        public int activo { get; set; }
        public string? tipodedato { get; set; }

        public string? campo_local { get; set; }
        public string? nombre { get; set; }

        public string? tipodedato_evidencia { get; set; }

        public string? campo_local_evidencia { get; set; }
        public string? nombre_evidencia { get; set; }
        public string? observacion { get; set; }


        public int tituloeditable { get; set; }
        public int requisitoeditable { get; set; }
        public int observacioneditable { get; set; }
        public int observacionobligatorio { get; set; }

        public ArchivoSubGrupoDiagnostico()
        {


        }
    }
}
