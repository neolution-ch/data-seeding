namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.DotNet.Console.Abstractions;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// The data initializer.
    /// </summary>
    public class InitCommand : IAsyncConsoleAppCommand<InitOptions>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<InitCommand> logger;

        /// <summary>
        /// The seeder.
        /// </summary>
        private readonly ISeeder seeder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitCommand" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="seeder">The seeder.</param>
        public InitCommand(ILogger<InitCommand> logger, ISeeder seeder)
        {
            this.logger = logger;
            this.seeder = seeder;
        }

        /// <inheritdoc />
        public Task RunAsync(InitOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return this.RunInternalAsync();
        }

        /// <summary>
        /// Runs the command asynchronously.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        private async Task RunInternalAsync()
        {
            /*
            this.logger.LogInformation("Start data initializer...");
            await this.seeder.SeedAsync().ConfigureAwait(true);
            this.logger.LogInformation("Data initializer finished!");
            */

            // Bisher
            await this.seeder.SeedAsync().ConfigureAwait(true);

            // Zusätzliche Möglichkeit
            await this.seeder.SeedAsync(typeof(MyOrderedSeed)).ConfigureAwait(true);
        }
    }
}
