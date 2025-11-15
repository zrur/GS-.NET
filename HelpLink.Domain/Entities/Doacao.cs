using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;

public class Doacao : BaseEntity
{
    public DateTime DataDoacao { get; set; }
    public string Status { get; set; } = string.Empty; // Pendente, Agendada, Concluída, Cancelada
    public string? Observacoes { get; set; }
    public decimal? ValorEstimado { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    
    public int InstituicaoId { get; set; }
    public Instituicao Instituicao { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<DoacaoItem> DoacaoItens { get; set; } = new List<DoacaoItem>();
    public Agendamento? Agendamento { get; set; }
    public ICollection<Impacto> Impactos { get; set; } = new List<Impacto>();
}

public class DoacaoItem : BaseEntity
{
    public int Quantidade { get; set; }
    public string? Observacoes { get; set; }
    
    public int DoacaoId { get; set; }
    public Doacao Doacao { get; set; } = null!;
    
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}

public class Agendamento : BaseEntity
{
    public DateTime DataAgendada { get; set; }
    public string HorarioInicio { get; set; } = string.Empty;
    public string HorarioFim { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Agendado, Confirmado, Realizado, Cancelado
    public string? Observacoes { get; set; }
    public DateTime? DataConfirmacao { get; set; }
    public DateTime? DataRealizacao { get; set; }
    
    public int DoacaoId { get; set; }
    public Doacao Doacao { get; set; } = null!;
}