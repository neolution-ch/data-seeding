namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    public class TenantsSettingsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TenantsSettingsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantsSettingsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TenantsSettingsSeed(ILogger<TenantsSettingsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type DependsOn => typeof(TenantsSeed);

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation($"Seed: {nameof(TenantsSettingsSeed)}");
            return Task.CompletedTask;
        }
    }
}
