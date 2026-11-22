using MoodGarden.Client.Models;

namespace MoodGarden.Client.Services;

/// <summary>Reads and updates <see cref="UserStreak"/> records in Supabase.</summary>
public class StreakService
{
    private readonly Supabase.Client _supabase;

    public StreakService(Supabase.Client supabase) => _supabase = supabase;

    public async Task<UserStreak?> GetByUserIdAsync(Guid userId)
    {
        var response = await _supabase
            .From<UserStreak>()
            .Where(x => x.UserId == userId && x.DeletedAt == null)
            .Get();

        return response.Models.FirstOrDefault();
    }

    public async Task<UserStreak> RecordPlantingAsync(Guid userId)
    {
        var today = DateTime.UtcNow.Date;
        var existing = await GetByUserIdAsync(userId);

        if (existing is null)
        {
            var created = new UserStreak
            {
                UserId = userId,
                CurrentStreak = 1,
                LongestStreak = 1,
                TotalPlantings = 1,
                LastPlantedDate = today,
                UpdatedAt = DateTime.UtcNow
            };
            var insertRes = await _supabase.From<UserStreak>().Insert(created);
            return insertRes.Models.FirstOrDefault() ?? created;
        }

        var lastDate = existing.LastPlantedDate?.Date;
        if (lastDate == today.AddDays(-1))
            existing.CurrentStreak += 1;
        else if (lastDate != today)
            existing.CurrentStreak = 1;

        existing.LongestStreak = Math.Max(existing.LongestStreak, existing.CurrentStreak);
        existing.TotalPlantings += 1;
        existing.LastPlantedDate = today;
        existing.UpdatedAt = DateTime.UtcNow;

        var updateRes = await _supabase.From<UserStreak>().Update(existing);
        return updateRes.Models.FirstOrDefault() ?? existing;
    }
}
