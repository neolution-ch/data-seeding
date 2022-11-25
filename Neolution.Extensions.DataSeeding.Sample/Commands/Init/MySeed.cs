namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neolution.Extensions.DataSeeding.Abstractions;
using Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds;

public class MySeed : Seed
{
    private readonly ILogger<MySeed> logger;

    public MySeed(ILogger<MySeed> logger)
    {
        this.logger = logger;
    }

    public override async Task SeedAsync()
    {
        this.logger.LogInformation("Start Seed");

        await this.SeedAsync<MasterSeed>();
        await this.SeedAsync<TenantsSeed>();
        await this.SeedAsync<UsersSeed>();

        this.logger.LogInformation("Seed finished!");
    }

}
