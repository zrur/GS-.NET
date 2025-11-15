namespace HelpLink.Application.DTOs;

public class DoacaoDto
{
    public int Id { get; set; }
    public DateTime DataDoacao { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public decimal? ValorEstimado { get; set; }
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int InstituicaoId { get; set; }
    public string NomeInstituicao { get; set; } = string.Empty;
    public IEnumerable<DoacaoItemDto> Itens { get; set; } = new List<DoacaoItemDto>();
    public AgendamentoDto? Agendamento { get; set; }
}

public class DoacaoCreateDto
{
    public string? Observacoes { get; set; }
    public int UsuarioId { get; set; }
    public int InstituicaoId { get; set; }
    public IEnumerable<DoacaoItemCreateDto> Itens { get; set; } = new List<DoacaoItemCreateDto>();
}

public class DoacaoUpdateDto
{
    public string? Status { get; set; }
    public string? Observacoes { get; set; }
    public decimal? ValorEstimado { get; set; }
}

public class DoacaoItemDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public string CategoriaItem { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public string? Observacoes { get; set; }
}

public class DoacaoItemCreateDto
{
    public int ItemId { get; set; }
    public int Quantidade { get; set; }
    public string? Observacoes { get; set; }
}

public class AgendamentoDto
{
    public int Id { get; set; }
    public DateTime DataAgendada { get; set; }
    public string HorarioInicio { get; set; } = string.Empty;
    public string HorarioFim { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}

public class AgendamentoCreateDto
{
    public int DoacaoId { get; set; }
    public DateTime DataAgendada { get; set; }
    public string HorarioInicio { get; set; } = string.Empty;
    public string HorarioFim { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}