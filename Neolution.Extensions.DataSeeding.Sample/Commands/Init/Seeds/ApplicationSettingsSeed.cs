namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    public class ApplicationSettingsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ApplicationSettingsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ApplicationSettingsSeed(ILogger<ApplicationSettingsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(ApplicationSettingsSeed)}");
            return Task.CompletedTask;
        }
    }
}
