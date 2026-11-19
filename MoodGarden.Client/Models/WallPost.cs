using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("wall_posts")]
public class WallPost : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("client_id")]
    public string? ClientId { get; set; }

    [Column("author")]
    public string Author { get; set; } = "Anonymous cat";

    [Column("mood_name")]
    public string MoodName { get; set; } = "Happy";

    [Column("paws")]
    public int Paws { get; set; } = 5;

    [Column("text")]
    public string Text { get; set; } = string.Empty;

    [Column("at")]
    public DateTime At { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}