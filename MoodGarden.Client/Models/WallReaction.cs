using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("wall_reactions")]
public class WallReaction : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("post_id")]
    public Guid PostId { get; set; }

    [Column("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [Column("reaction")]
    public string Reaction { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
