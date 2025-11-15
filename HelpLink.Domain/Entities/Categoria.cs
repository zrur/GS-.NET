using HelpLink.Domain.Common;

namespace HelpLink.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    
    // Relacionamentos
    public ICollection<Item> Itens { get; set; } = new List<Item>();
}

public class Item : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? Foto { get; set; }
    public string Estado { get; set; } = string.Empty; // Novo, Usado, Seminovo
    
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    
    // Relacionamentos
    public ICollection<DoacaoItem> DoacaoItens { get; set; } = new List<DoacaoItem>();
}
