using Supabase.Gotrue;

namespace MoodGarden.Client.Services;

/// <summary>Handles Supabase Authentication (sign up, sign in, password reset, and sign out).</summary>
public class AuthService
{
    private readonly Supabase.Client _supabase;

    public AuthService(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    public User? CurrentUser => _supabase.Auth.CurrentUser;
    public bool IsSignedIn => CurrentUser is not null;
    public string? Email => CurrentUser?.Email;

    public Guid? CurrentUserGuid =>
        Guid.TryParse(CurrentUser?.Id, out var id) ? id : null;

    public event Action? AuthStateChanged;

    public async Task<Session?> SignInAsync(string email, string password)
    {
        var session = await _supabase.Auth.SignIn(email.Trim(), password);
        AuthStateChanged?.Invoke();
        return session;
    }

    public async Task<Session?> SignUpAsync(string email, string password, string displayName)
    {
        var options = new SignUpOptions
        {
            Data = new Dictionary<string, object>
            {
                ["display_name"] = string.IsNullOrWhiteSpace(displayName) ? "Alex" : displayName.Trim()
            }
        };
        var session = await _supabase.Auth.SignUp(email.Trim(), password, options);
        AuthStateChanged?.Invoke();
        return session;
    }

    public async Task SendPasswordResetAsync(string email)
    {
        await _supabase.Auth.ResetPasswordForEmail(email.Trim());
    }

    public async Task SignOutAsync()
    {
        await _supabase.Auth.SignOut();
        AuthStateChanged?.Invoke();
    }
}
