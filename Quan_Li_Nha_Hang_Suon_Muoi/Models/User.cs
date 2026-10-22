namespace Quan_Li_Nha_Hang_Suon_Muoi.Models;
public enum UserRole
{
    Admin,
    Staff
}

public sealed class User
{
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public UserRole Role
    {
        get; init;
    }
}
