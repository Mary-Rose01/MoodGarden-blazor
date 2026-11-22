using MoodGarden.Client.Models;
using Supabase.Postgrest;

namespace MoodGarden.Client.Services;

/// <summary>CRUD + reaction operations for <see cref="WallPost"/> and <see cref="WallReaction"/> in Supabase.</summary>
public class WallService
{
    private readonly Supabase.Client _supabase;

    public WallService(Supabase.Client supabase) => _supabase = supabase;

    public async Task<List<WallPost>> GetAllAsync()
    {
        var response = await _supabase
            .From<WallPost>()
            .Where(x => x.DeletedAt == null)
            .Order("at", Constants.Ordering.Descending)
            .Get();

        return response.Models;
    }

    public async Task<WallPost> AddAsync(WallPost post)
    {
        var response = await _supabase.From<WallPost>().Insert(post);
        return response.Models.FirstOrDefault() ?? post;
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        await _supabase
            .From<WallPost>()
            .Where(x => x.Id == id)
            .Set(x => x.DeletedAt!, DateTime.UtcNow)
            .Update();
    }

    public async Task<List<WallReaction>> GetReactionsAsync()
    {
        try
        {
            var response = await _supabase
                .From<WallReaction>()
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetReactionsAsync fallback: {ex.Message}");
            return new List<WallReaction>();
        }
    }

    public async Task ToggleReactionAsync(Guid postId, string clientId, string reactionKey)
    {
        var existing = await _supabase
            .From<WallReaction>()
            .Where(x => x.PostId == postId && x.ClientId == clientId)
            .Get();

        var current = existing.Models.FirstOrDefault();
        if (current is not null)
        {
            await _supabase
                .From<WallReaction>()
                .Where(x => x.Id == current.Id)
                .Delete();

            if (current.Reaction == reactionKey)
                return;
        }

        await _supabase.From<WallReaction>().Insert(new WallReaction
        {
            PostId = postId,
            ClientId = clientId,
            Reaction = reactionKey,
            CreatedAt = DateTime.UtcNow
        });
    }
}
