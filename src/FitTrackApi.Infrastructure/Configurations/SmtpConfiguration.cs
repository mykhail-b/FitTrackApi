namespace FitTrackApi.Infrastructure.Configurations;

public class SmtpConfiguration
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool UseSsl { get; set; } = true;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromName { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
}
