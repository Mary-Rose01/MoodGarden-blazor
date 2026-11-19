using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("cat_moots")]
public class CatMoot : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("cat_name")]
    public string CatName { get; set; } = "Mochi";

    [Column("mood_tag")]
    public string MoodTag { get; set; } = string.Empty;

    [Column("emoji")]
    public string Emoji { get; set; } = "🐱";

    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}