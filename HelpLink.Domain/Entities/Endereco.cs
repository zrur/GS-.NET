using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;


public class Endereco : BaseEntity
{
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string CEP { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    public int BairroId { get; set; }
    public Bairro Bairro { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Instituicao> Instituicoes { get; set; } = new List<Instituicao>();
}