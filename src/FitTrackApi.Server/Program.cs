using FitTrackApi.Application;
using FitTrackApi.Infrastructure;
using FitTrackApi.Infrastructure.Configurations;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.IdentityEntity;
using FitTrackApi.Server.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ====================== SERVICES ======================

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpConfiguration"));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddForwardedHeadersSetup();
builder.Services.AddClientCors(builder.Configuration);
builder.Services.AddAntiforgerySetup();
builder.Services.AddCookieAuthSetup();

// This will be temporary
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\MSSQLLocalDB;Database=FitTrackDb;Trusted_Connection=True;TrustServerCertificate=True;"));
// === Identity ===
builder.Services.AddIdentity<UserAccount, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("FitTrack API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    // Redirect root URL to Scalar documentation
    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
        .ExcludeFromDescription();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();

app.UseCors("Client");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();