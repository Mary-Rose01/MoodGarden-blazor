using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("reflections")]
public class Reflection : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [Column("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [Column("text")]
    public string Text { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
