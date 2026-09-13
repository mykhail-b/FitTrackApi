using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Server.Extensions;
using FitTrackApi.Server.Services.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ====================== SERVICES ======================

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpConfiguration"));

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddForwardedHeadersSetup();
builder.Services.AddClientCors(builder.Configuration);
builder.Services.AddAntiforgerySetup();
builder.Services.AddCookieAuthSetup();
builder.Services.AddServicesSetup();

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