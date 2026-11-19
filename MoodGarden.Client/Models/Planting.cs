using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace MoodGarden.Client.Models;

[Table("plantings")]
public class Planting : BaseModel
{
    public Planting() { }

    public Planting(Guid id, string emoji, string moodName, double x, double y, DateTime at, string? note)
    {
        Id = id;
        Emoji = emoji;
        MoodName = moodName;
        X = x;
        Y = y;
        At = at;
        Note = note;
    }

    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("emoji")]
    public string Emoji { get; set; } = "😸";

    [Column("mood_name")]
    public string MoodName { get; set; } = "Happy";

    [Column("x")]
    public double X { get; set; }

    [Column("y")]
    public double Y { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("at")]
    public DateTime At { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}