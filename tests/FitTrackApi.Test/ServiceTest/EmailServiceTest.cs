using FitTrackApi.Infrastructure.Configurations;
using FitTrackApi.Infrastructure.Services;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace FitTrackApi.Test.ServiceTest;

public class EmailServiceTest
{
    [Fact]
    public async Task SendEmail_Should_ThrowOrComplete_WhenSmtpUnavailable()
    {
        var settings = new SmtpConfiguration
        {
            Host = "localhost",
            Port = 1025,
            UseSsl = false,
            Username = "",
            Password = "",
            FromEmail = "noreply@fittrack.local",
            FromName = "FitTrack"
        };

        var emailService = new EmailService(Options.Create(settings));

        try
        {
            await emailService.SendEmail("test@example.com", "Test Subject", "<p>Test body</p>");
        }
        catch (SocketException)
        {
            // Accept socket failures in environments without a local SMTP server
        }
    }
}