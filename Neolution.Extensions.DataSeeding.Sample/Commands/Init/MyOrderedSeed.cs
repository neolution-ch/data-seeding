namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Neolution.Extensions.DataSeeding.Abstractions;
using Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds;

public class MyOrderedSeed : OrderedSeed
{
    private readonly ILogger<MyOrderedSeed> logger;

    public MyOrderedSeed(ILogger<MyOrderedSeed> logger)
    {
        this.logger = logger;
    }

    public override async Task RunAsync()
    {
        this.logger.LogInformation("Start OrderedSeed");

        await this.SeedAsync(typeof(MasterSeed));
        await this.SeedAsync(typeof(TenantsSeed));
        await this.SeedAsync(typeof(UsersSeed));

        this.logger.LogInformation("OrderedSeed finished!");
    }

}
