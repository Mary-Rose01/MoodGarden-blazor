using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("moods")]
public class Mood : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("plant_name")]
    public string PlantName { get; set; } = string.Empty;

    [Column("plant_emoji")]
    public string PlantEmoji { get; set; } = "🌱";

    [Column("color_hex")]
    public string ColorHex { get; set; } = "#22c55e";

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}