namespace NhjDotnetApi.Presentation.Models;

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? DisplayName { get; set; }
    public string? Role { get; set; }
}
