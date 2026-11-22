using Microsoft.JSInterop;
using MoodGarden.Client.Models;

namespace MoodGarden.Client.Services;

/// <summary>Loads and saves the gardener's <see cref="UserProfile"/>, theme, and equipped items in Supabase.</summary>
public class ProfileService
{
    private readonly Supabase.Client _supabase;
    private readonly IJSRuntime _js;
    private string? _clientId;

    public ProfileService(Supabase.Client supabase, IJSRuntime js)
    {
        _supabase = supabase;
        _js = js;
    }

    public async Task<string> GetClientIdAsync()
    {
        if (!string.IsNullOrEmpty(_clientId)) return _clientId;
        _clientId = await _js.InvokeAsync<string>("garden.getClientId");
        return _clientId;
    }

    public async Task<UserProfile> GetOrCreateProfileAsync()
    {
        var clientId = await GetClientIdAsync();
        try
        {
            var res = await _supabase
                .From<UserProfile>()
                .Where(x => x.ClientId == clientId && x.DeletedAt == null)
                .Get();

            var profile = res.Models.FirstOrDefault();
            if (profile is not null)
            {
                await _js.InvokeVoidAsync("garden.setTheme", profile.NightMode);
                return profile;
            }

            var created = new UserProfile
            {
                ClientId = clientId,
                Username = "Alex",
                DisplayName = "Alex",
                AvatarEmoji = "🐱",
                NightMode = false,
                RemindersEnabled = true,
                EquippedItems = "🌻,☁️",
                CreatedAt = DateTime.UtcNow
            };

            var insertRes = await _supabase.From<UserProfile>().Insert(created);
            var saved = insertRes.Models.FirstOrDefault() ?? created;
            await _js.InvokeVoidAsync("garden.setTheme", saved.NightMode);
            return saved;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProfileService fallback: {ex.Message}");
            return new UserProfile
            {
                ClientId = clientId,
                Username = "Alex",
                DisplayName = "Alex",
                AvatarEmoji = "🐱",
                EquippedItems = "🌻,☁️"
            };
        }
    }

    public async Task<UserProfile> SaveProfileAsync(UserProfile profile)
    {
        await _js.InvokeVoidAsync("garden.setTheme", profile.NightMode);
        if (profile.Id == Guid.Empty)
        {
            var insertRes = await _supabase.From<UserProfile>().Insert(profile);
            return insertRes.Models.FirstOrDefault() ?? profile;
        }

        var res = await _supabase.From<UserProfile>().Update(profile);
        return res.Models.FirstOrDefault() ?? profile;
    }

    public static List<string> ParseEquipped(UserProfile profile) =>
        string.IsNullOrWhiteSpace(profile.EquippedItems)
            ? new List<string>()
            : profile.EquippedItems.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

    public static void SetEquipped(UserProfile profile, IEnumerable<string> items) =>
        profile.EquippedItems = string.Join(",", items);
}
