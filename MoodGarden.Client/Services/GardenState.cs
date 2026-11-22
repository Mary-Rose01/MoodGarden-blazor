using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MoodGarden.Client.Models;

namespace MoodGarden.Services;

public record MoodPreset(string Key, string Emoji, string Name, string ColorHex, string Desc, int Score);

public record DecorationItem(string Emoji, string Name, string RequirementType, int RequiredCount);

public record AchievementDef(string Title, string Description, string MetricType, int Target);

public class GardenState
{
    private const string StorageKey = "cat-mood-garden:plantings";
    private readonly IJSRuntime _js;
    private bool _loaded;

    public GardenState(IJSRuntime js) => _js = js;

    public static readonly IReadOnlyList<MoodPreset> Moods = new List<MoodPreset>
    {
        new("happy",   "😸", "Happy",   "#FFE5A3", "Sunny and light",  5),
        new("sad",     "😿", "Sad",     "#C8E3F5", "Heavy heart",      1),
        new("angry",   "😡", "Angry",   "#F7A8B8", "Fired up",         1),
        new("tired",   "😴", "Tired",   "#DCD0FF", "Low battery",      2),
        new("anxious", "😰", "Anxious", "#FFE5A3", "Buzzing inside",   2),
        new("calm",    "😌", "Calm",    "#BFD8BD", "Peaceful",         4),
        new("excited", "🤩", "Excited", "#F7A8B8", "Sparkling",        5),
        new("lonely",  "🥺", "Lonely",  "#C8E3F5", "Missing someone",  2),
    };

    public static readonly IReadOnlyList<DecorationItem> Decorations = new List<DecorationItem>
    {
        new("🌻", "Sunflower",     "cats",   1),
        new("☁️", "Cloud",         "cats",   0),
        new("🌷", "Tulips",        "cats",   3),
        new("🦋", "Butterflies",   "cats",   5),
        new("🌈", "Rainbow",       "streak", 3),
        new("🌳", "Tree",          "cats",   10),
        new("🐈", "Cat companion", "streak", 7),
    };

    public static readonly IReadOnlyList<AchievementDef> AchievementDefs = new List<AchievementDef>
    {
        new("First Plant",        "Plant your first mood cat",    "cats",   1),
        new("Growing Garden",     "Plant 10 mood cats",           "cats",   10),
        new("Flourishing Garden", "Plant 50 mood cats",           "cats",   50),
        new("Little Sprout",      "Check in 3 days in a row",     "streak", 3),
        new("Blooming",           "Reach a 7-day streak",         "streak", 7),
        new("Flourishing",        "Reach a 30-day streak",        "streak", 30),
        new("First Words",        "Write your first diary note",  "notes",  1),
        new("Storyteller",        "Write 10 diary notes",         "notes",  10),
        new("Mood Explorer",      "Experience all 8 moods",       "moods",  8),
    };

    public static readonly string[] ReflectionPrompts =
    {
        "What made you smile today?",
        "What challenged you today?",
        "What are you grateful for?",
        "What would you tell yourself right now?"
    };

    public static readonly string[] KindMessages =
    {
        "Naming a feeling helps it settle 💗",
        "All feelings are welcome here 🌿",
        "Thank you for noticing. That takes care."
    };

    public static MoodPreset Find(string nameOrKey) =>
        Moods.FirstOrDefault(m =>
            string.Equals(m.Name, nameOrKey, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(m.Key, nameOrKey, StringComparison.OrdinalIgnoreCase))
        ?? Moods[0];

    public static string Css(double v) => v.ToString("0.##", CultureInfo.InvariantCulture);

    public static string TimeAgo(DateTime utc)
    {
        var d = DateTime.UtcNow - utc;
        if (d.TotalMinutes < 1) return "Just now";
        if (d.TotalHours < 1) return $"{(int)d.TotalMinutes} min ago";
        if (d.TotalDays < 1) return $"{(int)d.TotalHours} hr ago";
        if (d.TotalDays < 2) return "Yesterday, " + utc.ToLocalTime().ToString("h:mm tt", CultureInfo.InvariantCulture);
        if (d.TotalDays < 7) return $"{(int)d.TotalDays} days ago";
        return utc.ToLocalTime().ToString("MMM d, yyyy", CultureInfo.InvariantCulture);
    }

    public static string FormatDate(DateTime utc) =>
        utc.ToLocalTime().ToString("MMM d, h:mm tt", CultureInfo.InvariantCulture);

    public static int CalculateStreak(IEnumerable<Planting> plantings)
    {
        var days = plantings.Select(p => p.At.ToLocalTime().Date).ToHashSet();
        var d = DateTime.Today;
        if (!days.Contains(d)) d = d.AddDays(-1);
        var streak = 0;
        var grace = true;
        while (true)
        {
            if (days.Contains(d))
            {
                streak++;
            }
            else if (grace && days.Contains(d.AddDays(-1)))
            {
                grace = false;
            }
            else
            {
                break;
            }
            d = d.AddDays(-1);
        }
        return streak;
    }

    public static int AchievementProgress(AchievementDef def, IReadOnlyList<Planting> plantings) =>
        def.MetricType switch
        {
            "cats"   => plantings.Count,
            "streak" => CalculateStreak(plantings),
            "notes"  => plantings.Count(p => !string.IsNullOrWhiteSpace(p.Note)),
            "moods"  => plantings.Select(p => Find(p.MoodName).Key).Distinct().Count(),
            _        => 0
        };

    public List<Planting> Plantings { get; private set; } = new();
    public IReadOnlyList<Planting> Recent =>
        Plantings.Where(p => p.At > DateTime.UtcNow.AddHours(-24)).ToList();

    public event Action? Changed;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        _loaded = true;
        try
        {
            var json = await _js.InvokeAsync<string?>("garden.load", StorageKey);
            if (!string.IsNullOrEmpty(json))
                Plantings = JsonSerializer.Deserialize<List<Planting>>(json) ?? new();
        }
        catch { Plantings = new(); }
        Changed?.Invoke();
    }

    public async Task AddAsync(Planting planting)
    {
        Plantings.Add(planting);
        await _js.InvokeVoidAsync("garden.save", StorageKey, JsonSerializer.Serialize(Plantings));
        Changed?.Invoke();
    }
}

public abstract class GardenPageBase : ComponentBase, IDisposable
{
    [Inject] protected GardenState Garden { get; set; } = default!;

    protected override void OnInitialized() => Garden.Changed += OnGardenChanged;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await Garden.EnsureLoadedAsync();
    }

    private void OnGardenChanged() => InvokeAsync(StateHasChanged);

    public void Dispose() => Garden.Changed -= OnGardenChanged;
}
