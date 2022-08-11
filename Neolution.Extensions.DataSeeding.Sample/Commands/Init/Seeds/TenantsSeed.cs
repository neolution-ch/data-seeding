namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    public class TenantsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TenantsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TenantsSeed(ILogger<TenantsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(TenantsSeed)}");
            return Task.CompletedTask;
        }
    }
}
