using FitTrackApi.Server.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MsSql;

namespace FitTrackApi.Test.Configuration;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private const string DatabaseName = "FitTrackTests";

    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
        .WithPassword("YourStrong!Passw0rd")
        .Build();

    private readonly SemaphoreSlim _resetLock = new(1, 1);

    private Respawner _respawner = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        ConnectionString = BuildConnectionString(DatabaseName);

        await using (var context = CreateContext())
        {
            await context.Database.MigrateAsync();
        }

        await using (var connection = new SqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = ["dbo"],
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
        }
    }

    public DataContext CreateContext()
    {
        return new DataContext(CreateOptions());
    }

    public async Task ResetDatabaseAsync()
    {
        await _resetLock.WaitAsync();
        try
        {
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            await _respawner.ResetAsync(connection);
        }
        finally
        {
            _resetLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        _resetLock.Dispose();
        await _container.DisposeAsync();
    }

    private string BuildConnectionString(string databaseName)
    {
        var builder = new SqlConnectionStringBuilder(_container.GetConnectionString())
        {
            InitialCatalog = databaseName,
            TrustServerCertificate = true
        };

        return builder.ConnectionString;
    }

    private DbContextOptions<DataContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly("FitTrackApi.Server"))
            .Options;
    }
}
