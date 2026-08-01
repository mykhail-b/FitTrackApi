using FitTrackApi.Infrastructure.Data;

namespace FitTrackApi.Test.Configuration;

[Collection(DatabaseCollection.Name)]
public abstract class IntegrationTestBase(DatabaseFixture fixture) : IAsyncLifetime
{
    protected DatabaseFixture Fixture { get; } = fixture;

    protected DataContext DbContext { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();
        DbContext = Fixture.CreateContext();
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}
