using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Hosting;
using Miru.Sqlite;

namespace MiruNext.Tests.Config;

public class TestsConfig : ITestConfig
{
    public virtual void ConfigureTestServices(IServiceCollection services)
    {
        services
            .AddFeatureTesting()
            .AddTestingUserSession<User>()
            .AddDatabaseCleaner<SqliteDatabaseCleaner>()
            .AddQueueCleaner<LiteDbQueueCleaner>();

        // Mock your services that talk with external apps
        // services.Mock<IService>();
    }
    
    public void ConfigureRun(TestRunConfig run)
    {
        // This run configurations only works for tests that inherits MiruTest
        // It includes FeatureTest, PageTest and all other Miru's types of tests
            
        run.TestingDefault();
    }
        
    public virtual IHostBuilder GetHostBuilder() =>
        MiruHost.CreateMiruHost();
}