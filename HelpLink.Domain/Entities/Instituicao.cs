using HelpLink.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    [Column(TypeName = "NUMBER(1)")]
    public int Verificada { get; set; } // 1 = sim | 0 = não
    
    public DateTime? DataVerificacao { get; set; }
    
    public int? EnderecoId { get; set; }  // Nullable temporariamente para teste
    public Endereco? Endereco { get; set; }
    
    // Relacionamentos
    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();
    public ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
    public ICollection<Impacto> Impactos { get; set; } = new List<Impacto>();
}
