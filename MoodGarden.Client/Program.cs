using MoodGarden.Services;
using MoodGarden.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Supabase;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<GardenState>();
builder.Services.AddScoped<MoodService>();
builder.Services.AddScoped<PlantingService>();
builder.Services.AddScoped<WallService>();
builder.Services.AddScoped<StreakService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<ReflectionService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var url = config["Supabase:Url"]
        ?? throw new InvalidOperationException("Supabase:Url is missing from wwwroot/appsettings.json.");
    var key = config["Supabase:Key"]
        ?? throw new InvalidOperationException("Supabase:Key is missing from wwwroot/appsettings.json.");

    var options = new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = false // Prevents WebSocket header exceptions in Blazor WASM
    };

    return new Supabase.Client(url, key, options);
});

var host = builder.Build();

// Safe async initialization so the app still boots if the user opens MoodGarden offline
try
{
    var supabase = host.Services.GetRequiredService<Supabase.Client>();
    await supabase.InitializeAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Supabase initialization deferred (possibly offline): {ex.Message}");
}

await host.RunAsync();