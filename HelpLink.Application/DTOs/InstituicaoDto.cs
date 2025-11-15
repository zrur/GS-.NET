namespace HelpLink.Application.DTOs;

public class InstituicaoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Site { get; set; }
    public string? Logo { get; set; }
    public bool Verificada { get; set; }
    public EnderecoDto? Endereco { get; set; }
}

public class InstituicaoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Site { get; set; }
    public int EnderecoId { get; set; }
}

public class InstituicaoUpdateDto
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Site { get; set; }
    public string? Logo { get; set; }
}

public class EnderecoDto
{
    public int Id { get; set; }
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}