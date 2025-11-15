using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;

public class Instituicao : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Site { get; set; }
    public string? Logo { get; set; }
    public bool Verificada { get; set; } = false;
    public DateTime? DataVerificacao { get; set; }
    
    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    public ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
    public ICollection<Impacto> Impactos { get; set; } = new List<Impacto>();
}
