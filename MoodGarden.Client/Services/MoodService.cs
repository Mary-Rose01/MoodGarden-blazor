using MoodGarden.Client.Models;
using Supabase.Postgrest;

namespace MoodGarden.Client.Services;

/// <summary>Loads mood definitions from the Supabase <c>moods</c> table.</summary>
public class MoodService
{
    private readonly Supabase.Client _supabase;

    public MoodService(Supabase.Client supabase) => _supabase = supabase;

    public async Task<List<Mood>> GetAllAsync()
    {
        var response = await _supabase
            .From<Mood>()
            .Where(x => x.DeletedAt == null)
            .Order("sort_order", Constants.Ordering.Ascending)
            .Get();

        return response.Models;
    }
}
