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

    [Column("client_id")]
    public string? ClientId { get; set; }

    [Column("username")]
    public string Username { get; set; } = "Alex";

    [Column("display_name")]
    public string? DisplayName { get; set; }

    [Column("avatar_emoji")]
    public string AvatarEmoji { get; set; } = "🐱";

    [Column("garden_title")]
    public string GardenTitle { get; set; } = "My Mood Garden";

    [Column("night_mode")]
    public bool NightMode { get; set; }

    [Column("reminders_enabled")]
    public bool RemindersEnabled { get; set; } = true;

    [Column("equipped_items")]
    public string EquippedItems { get; set; } = "🌻,☁️";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
