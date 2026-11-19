using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("user_streaks")]
public class UserStreak : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("current_streak")]
    public int CurrentStreak { get; set; }

    [Column("longest_streak")]
    public int LongestStreak { get; set; }

    [Column("total_plantings")]
    public int TotalPlantings { get; set; }

    [Column("last_planted_date")]
    public DateTime? LastPlantedDate { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}