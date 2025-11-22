using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;

public class AuditoriaLog : BaseEntity
{
    public string EntidadeNome { get; set; } = string.Empty;
    public int EntidadeId { get; set; }
    public string Acao { get; set; } = string.Empty; // INSERT, UPDATE, DELETE
    public string? ValoresAntigos { get; set; }
    public string? ValoresNovos { get; set; }
    public string? UsuarioId { get; set; }
    public string? EnderecoIP { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
}
