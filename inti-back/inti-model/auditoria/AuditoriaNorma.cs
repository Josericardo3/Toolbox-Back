namespace inti_repository.auditoria
{
    public class AuditoriaNorma
    {
        public int ID_NORMA { get; set; }
        public List<Titulos>? TITUTLOS { get; set; }

        public AuditoriaNorma() {
            TITUTLOS = new List<Titulos>();
        }

    }

    public class Titulos
    {
        public String? TITULO_PRINCIPAL { get; set; }
    }
    
}