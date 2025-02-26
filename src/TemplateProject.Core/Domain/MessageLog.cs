using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateProject.Core.Domain;

[Table("message_log")]
public class MessageLog : IEntity, IEntityCreated
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("message_type")]
    [StringLength(128)]
    public string MessageType { get; set; } = string.Empty;
    
    [Column("result_type")]
    [StringLength(128)]
    public string ResultType { get; set; } = string.Empty;
    
    [Column("message_json")]
    public string MessageJson { get; set; } = string.Empty;
    
    [Column("result_json")]
    public string ResultJson { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}