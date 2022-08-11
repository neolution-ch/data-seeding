namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init
{
    using System;
    using Microsoft.Extensions.Logging;
    using Neolution.DotNet.Console.Abstractions;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// The data initializer.
    /// </summary>
    /// <seealso cref="IConsoleAppCommand{TOptions}" />
    public class InitCommand : IConsoleAppCommand<InitOptions>
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

        /// <summary>
        /// Runs the command with the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Run(InitOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.logger.LogInformation("Start data initializer...");
            this.seeder.SeedAsync();
            this.logger.LogInformation("Data initializer finished!");
        }
    }
}
