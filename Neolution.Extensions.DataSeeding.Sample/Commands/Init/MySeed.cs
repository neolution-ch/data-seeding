namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds;

    /// <inheritdoc />
    public class MySeed : Seed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MySeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MySeed(ILogger<MySeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public override async Task SeedAsync()
        {
            this.logger.LogInformation("Start Seed");

            await SeedAsync<MasterSeed>().ConfigureAwait(true);
            await SeedAsync<TenantsSeed>().ConfigureAwait(true);
            await SeedAsync<UsersSeed>().ConfigureAwait(true);

            this.logger.LogInformation("Seed finished!");
        }
    }
}
