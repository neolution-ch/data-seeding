namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    public class MasterSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MasterSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MasterSeed(ILogger<MasterSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public int Priority => 2;

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(MasterSeed)}");
            return Task.CompletedTask;
        }
    }
}
