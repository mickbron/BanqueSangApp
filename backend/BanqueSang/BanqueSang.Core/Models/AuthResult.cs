namespace BanqueSang.Core.Models;

public class AuthResult
{
    public string Token { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }
}