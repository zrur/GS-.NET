using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;


public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string? Foto { get; set; }
    public DateTime DataNascimento { get; set; }
    
    public int? EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }
    
    // Relacionamentos
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    public ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
}
