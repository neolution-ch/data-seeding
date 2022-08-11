namespace Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiTenantSeeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <inheritdoc />
    public class PermissionsSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PermissionsSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PermissionsSeed(ILogger<PermissionsSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type DependsOn => typeof(UsersSeed);

        /// <inheritdoc />
        public Task SeedAsync()
        {
            this.logger.LogInformation("Seed() called");
            return Task.CompletedTask;
        }
    }
}
