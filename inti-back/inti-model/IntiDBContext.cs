using inti_model.DocumentosRequerimientos;
using inti_model.kpis;
using inti_model.usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_model
{
    public class IntiDBContext:DbContext
    {
        public IntiDBContext()
        {

        }
        public IntiDBContext(DbContextOptions<IntiDBContext> options) : base(options)
        {

        }
        public DbSet<Indicador> Indicadores { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<PeriodoMedicion> PeriodosMedicion { get; set; }
        public DbSet<Proceso> Procesos { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<IndicadorPorNorma> IndicadoresPorNormas { get; set; }
        public DbSet<EvaluacionIndicador> EvaluacionIndicadores { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<VariableEvaluacionIndicador> VariablesEvaluacionIndicadores { get; set; }
        public DbSet<VariablePorIndicador> VariablesPorIndicador { get; set; }
        public DbSet<FuenteDato> FuenteDatos { get; set; }
        public DbSet<IdentificadorFormula> IdentificadoresFormulas { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<DocumentoRequerimiento> DocumentosRequerimientos { get; set; }
        public DbSet<UsuarioPst>UsuarioPst { get; set; }
    }
}
