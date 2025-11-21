using HelpLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpLink.Infrastructure.Data;

public class HelpLinkDbContext : DbContext
{
    public HelpLinkDbContext(DbContextOptions<HelpLinkDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Pais> Paises { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Bairro> Bairros { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Instituicao> Instituicoes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Item> Itens { get; set; }
    public DbSet<Doacao> Doacoes { get; set; }
    public DbSet<DoacaoItem> DoacaoItens { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<Impacto> Impactos { get; set; }
    public DbSet<Voluntario> Voluntarios { get; set; }
    public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações de Pais
        modelBuilder.Entity<Pais>(entity =>
        {
            entity.ToTable("Paises");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Sigla).IsRequired().HasMaxLength(3);
            entity.Property(e => e.CodigoIBGE).HasMaxLength(10);
            entity.HasIndex(e => e.Sigla).IsUnique();
        });
        modelBuilder.Entity<Instituicao>()
            .Property(i => i.Ativo)
            .HasConversion<int>();

        modelBuilder.Entity<Instituicao>()
            .Property(i => i.Verificada)
            .HasConversion<int>();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Ativo)
            .HasConversion<int>();

        modelBuilder.Entity<Voluntario>()
            .Property(v => v.Ativo)
            .HasConversion<int>();

        modelBuilder.Entity<Doacao>()
            .Property(d => d.Ativo)
            .HasConversion<int>();
        // Configurações de Estado
        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estados");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Sigla).IsRequired().HasMaxLength(2);
            entity.Property(e => e.CodigoIBGE).HasMaxLength(10);
            
            entity.HasOne(e => e.Pais)
                .WithMany(p => p.Estados)
                .HasForeignKey(e => e.PaisId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Cidade
        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.ToTable("Cidades");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CodigoIBGE).HasMaxLength(10);
            
            entity.HasOne(e => e.Estado)
                .WithMany(e => e.Cidades)
                .HasForeignKey(e => e.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Bairro
        modelBuilder.Entity<Bairro>(entity =>
        {
            entity.ToTable("Bairros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CEPInicial).HasMaxLength(9);
            entity.Property(e => e.CEPFinal).HasMaxLength(9);
            
            entity.HasOne(e => e.Cidade)
                .WithMany(c => c.Bairros)
                .HasForeignKey(e => e.CidadeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Endereco
        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("Enderecos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Logradouro).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Numero).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Complemento).HasMaxLength(100);
            entity.Property(e => e.CEP).IsRequired().HasMaxLength(9);
            entity.Property(e => e.Latitude).HasPrecision(10, 8);
            entity.Property(e => e.Longitude).HasPrecision(11, 8);
            
            entity.HasOne(e => e.Bairro)
                .WithMany(b => b.Enderecos)
                .HasForeignKey(e => e.BairroId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CPF).IsRequired().HasMaxLength(14);
            entity.Property(e => e.Foto).HasMaxLength(500);
            
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.CPF).IsUnique();
            
            entity.HasOne(e => e.Endereco)
                .WithMany(en => en.Usuarios)
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurações de Instituicao
        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.ToTable("Instituicoes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CNPJ).IsRequired().HasMaxLength(18);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Site).HasMaxLength(200);
            entity.Property(e => e.Logo).HasMaxLength(500);
            
            entity.HasIndex(e => e.CNPJ).IsUnique();
            
            entity.HasOne(e => e.Endereco)
                .WithMany(en => en.Instituicoes)
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Icone).IsRequired().HasMaxLength(100);
        });

        // Configurações de Item
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Itens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Foto).HasMaxLength(500);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
            
            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.Itens)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Doacao
        modelBuilder.Entity<Doacao>(entity =>
        {
            entity.ToTable("Doacoes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Observacoes).HasMaxLength(1000);
            entity.Property(e => e.ValorEstimado).HasPrecision(10, 2);
            
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Doacoes)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Instituicao)
                .WithMany(i => i.Doacoes)
                .HasForeignKey(e => e.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de DoacaoItem
        modelBuilder.Entity<DoacaoItem>(entity =>
        {
            entity.ToTable("DoacaoItens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Observacoes).HasMaxLength(500);
            
            entity.HasOne(e => e.Doacao)
                .WithMany(d => d.DoacaoItens)
                .HasForeignKey(e => e.DoacaoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Item)
                .WithMany(i => i.DoacaoItens)
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Agendamento
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.ToTable("Agendamentos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HorarioInicio).IsRequired().HasMaxLength(5);
            entity.Property(e => e.HorarioFim).IsRequired().HasMaxLength(5);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Observacoes).HasMaxLength(1000);
            
            entity.HasOne(e => e.Doacao)
                .WithOne(d => d.Agendamento)
                .HasForeignKey<Agendamento>(e => e.DoacaoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurações de Impacto
        modelBuilder.Entity<Impacto>(entity =>
        {
            entity.ToTable("Impactos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Foto).HasMaxLength(500);
            
            entity.HasOne(e => e.Instituicao)
                .WithMany(i => i.Impactos)
                .HasForeignKey(e => e.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Doacao)
                .WithMany(d => d.Impactos)
                .HasForeignKey(e => e.DoacaoId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurações de Voluntario
        modelBuilder.Entity<Voluntario>(entity =>
        {
            entity.ToTable("Voluntarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Habilidades).HasMaxLength(1000);
            entity.Property(e => e.Disponibilidade).HasMaxLength(500);
            
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Voluntarios)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Instituicao)
                .WithMany(i => i.Voluntarios)
                .HasForeignKey(e => e.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de AuditoriaLog
        modelBuilder.Entity<AuditoriaLog>(entity =>
        {
            entity.ToTable("AuditoriaLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntidadeNome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Acao).IsRequired().HasMaxLength(20);
            entity.Property(e => e.UsuarioId).HasMaxLength(50);
            entity.Property(e => e.EnderecoIP).HasMaxLength(45);
            
            entity.HasIndex(e => new { e.EntidadeNome, e.EntidadeId });
            entity.HasIndex(e => e.DataHora);
        });
    }
}