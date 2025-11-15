using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;


public class Pais : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string CodigoIBGE { get; set; } = string.Empty;
    
    // Relacionamentos
    public ICollection<Estado> Estados { get; set; } = new List<Estado>();
}

public class Estado : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string CodigoIBGE { get; set; } = string.Empty;
    
    public int PaisId { get; set; }
    public Pais Pais { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
}

public class Cidade : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CodigoIBGE { get; set; } = string.Empty;
    
    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<Bairro> Bairros { get; set; } = new List<Bairro>();
}

public class Bairro : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CEPInicial { get; set; } = string.Empty;
    public string CEPFinal { get; set; } = string.Empty;
    
    public int CidadeId { get; set; }
    public Cidade Cidade { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
}