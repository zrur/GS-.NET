using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;

public class Impacto : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public int QuantidadeBeneficiados { get; set; }
    public string? Foto { get; set; }
    public DateTime DataImpacto { get; set; }
    
    public int InstituicaoId { get; set; }
    public Instituicao Instituicao { get; set; } = null!;
    
    public int? DoacaoId { get; set; }
    public Doacao? Doacao { get; set; }
}

public class Voluntario : BaseEntity
{
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string Status { get; set; } = string.Empty; // Ativo, Inativo
    public string? Habilidades { get; set; }
    public string? Disponibilidade { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    
    public int InstituicaoId { get; set; }
    public Instituicao Instituicao { get; set; } = null!;
}
