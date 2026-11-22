using MoodGarden.Client.Models;

namespace MoodGarden.Client.Repositories;

/// <summary>Data access contract for MoodGarden entities stored in Supabase.</summary>
public interface IGardenRepository
{
    Task<List<Mood>> GetMoodsAsync();
    Task<List<Planting>> GetPlantingsAsync();
    Task<List<Planting>> GetRecentPlantingsAsync(TimeSpan window);
    Task<Planting> AddPlantingAsync(Planting planting);
    Task UpdatePlantingNoteAsync(Guid id, string? note);
    Task SoftDeletePlantingAsync(Guid id);

    Task<List<WallPost>> GetWallPostsAsync();
    Task<WallPost> AddWallPostAsync(WallPost post);
    Task SoftDeleteWallPostAsync(Guid id);

    Task<UserStreak?> GetStreakAsync(Guid userId);
}
