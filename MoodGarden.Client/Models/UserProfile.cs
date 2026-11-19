using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("user_profiles")]
public class UserProfile : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("auth_user_id")]
    public Guid? AuthUserId { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("display_name")]
    public string? DisplayName { get; set; }

    [Column("avatar_emoji")]
    public string AvatarEmoji { get; set; } = "🌱";

    [Column("garden_title")]
    public string GardenTitle { get; set; } = "My Mood Garden";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}