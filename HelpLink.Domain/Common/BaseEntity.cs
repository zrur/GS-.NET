using System.ComponentModel.DataAnnotations.Schema;

namespace HelpLink.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    
    [Column(TypeName = "NUMBER(1)")]
    public int Ativo { get; set; } 
}