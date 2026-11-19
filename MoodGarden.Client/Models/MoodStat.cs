using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("mood_stats")]
public class MoodStat : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("mood_label")]
    public string MoodLabel { get; set; } = string.Empty;

    [Column("plant_emoji")]
    public string PlantEmoji { get; set; } = "🌱";

    [Column("entry_count")]
    public int EntryCount { get; set; }

    [Column("period_month")]
    public string PeriodMonth { get; set; } = string.Empty;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}