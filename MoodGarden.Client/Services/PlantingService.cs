using MoodGarden.Client.Models;
using Supabase.Postgrest;

namespace MoodGarden.Client.Services;

/// <summary>CRUD operations for <see cref="Planting"/> records in Supabase.</summary>
public class PlantingService
{
    private readonly Supabase.Client _supabase;

    public PlantingService(Supabase.Client supabase) => _supabase = supabase;

    public async Task<List<Planting>> GetAllAsync()
    {
        var response = await _supabase
            .From<Planting>()
            .Where(x => x.DeletedAt == null)
            .Order("at", Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task<List<Planting>> GetRecentAsync(TimeSpan window)
    {
        var cutoff = DateTime.UtcNow - window;
        var response = await _supabase
            .From<Planting>()
            .Where(x => x.DeletedAt == null && x.At >= cutoff)
            .Order("at", Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task<Planting> AddAsync(Planting planting)
    {
        var response = await _supabase.From<Planting>().Insert(planting);
        return response.Models.FirstOrDefault() ?? planting;
    }

    public async Task UpdateNoteAsync(Guid id, string? note)
    {
        var cleaned = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        await _supabase
            .From<Planting>()
            .Where(x => x.Id == id)
            .Set(x => x.Note!, cleaned)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Update();
    }

    public async Task UpdateEntryAsync(Guid id, string emoji, string moodName, string? note)
    {
        var cleaned = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        await _supabase
            .From<Planting>()
            .Where(x => x.Id == id)
            .Set(x => x.Emoji, emoji)
            .Set(x => x.MoodName, moodName)
            .Set(x => x.Note!, cleaned)
            .Set(x => x.UpdatedAt, DateTime.UtcNow)
            .Update();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        await _supabase
            .From<Planting>()
            .Where(x => x.Id == id)
            .Set(x => x.DeletedAt!, DateTime.UtcNow)
            .Update();
    }
}
