using MoodGarden.Client.Models;
using Supabase.Postgrest;

namespace MoodGarden.Client.Services;

/// <summary>CRUD operations for <see cref="Reflection"/> entries in Supabase.</summary>
public class ReflectionService
{
    private readonly Supabase.Client _supabase;

    public ReflectionService(Supabase.Client supabase) => _supabase = supabase;

    public async Task<List<Reflection>> GetAllAsync(string clientId)
    {
        var response = await _supabase
            .From<Reflection>()
            .Where(x => x.ClientId == clientId && x.DeletedAt == null)
            .Order("created_at", Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task<Reflection> AddAsync(Reflection reflection)
    {
        var response = await _supabase.From<Reflection>().Insert(reflection);
        return response.Models.FirstOrDefault() ?? reflection;
    }
}
