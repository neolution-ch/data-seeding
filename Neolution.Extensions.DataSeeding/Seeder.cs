namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Internal;

    /// <inheritdoc />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Resolved as a singleton by DI container")]
    internal class Seeder : ISeeder
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<Seeder> logger;

        /// <summary>
        /// The counter (used for tracing)
        /// </summary>
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seeder" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public Seeder(IServiceProvider serviceProvider, ILogger<Seeder> logger)
        {
            this.logger = logger;
            Seeding.Instance.UseServiceProvider(serviceProvider);
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogDebug("Determine seeding order and wrap them up accordingly");
            var wraps = Seeding.Instance.WrapSeeds();
            this.logger.LogDebug("All seeds sorted and wrapped up");

            if (this.logger.IsEnabled(LogLevel.Trace))
            {
                this.logger.LogTrace("The seeds will be seeded in the following order:");

                for (var index = 0; index < wraps.Count; index++)
                {
                    var wrap = wraps[index];
                    this.LogWrapTree(wrap, index == wrap.Wrapped.Count - 1);
                }
            }

            this.logger.LogDebug("Unwrap seeds and start seeding...");
            await Seeding.Instance.UnwrapAndSeedAsync(wraps).ConfigureAwait(false);
            this.logger.LogDebug("All seeds have been seeded!");

            Seeding.Instance.Dispose();
        }

        /// <summary>
        /// Logs the wrap tree in a pretty format.
        /// </summary>
        /// <param name="wrap">The wrap.</param>
        /// <param name="last">if set to <c>true</c> it's the last wrap.</param>
        /// <param name="indent">The indent.</param>
        private void LogWrapTree(Wrap wrap, bool last, string indent = "")
        {
            var seedTypeName = wrap.SeedType?.Name ?? string.Empty;

            // Shorten the seed type name if it was suffixed with "Seed"
            const string suffix = "Seed";
            if (seedTypeName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                seedTypeName = seedTypeName[..^suffix.Length];
            }

            this.logger.LogTrace($"{++this.counter}.\t{indent}+- {seedTypeName}");
            indent += last ? "   " : "|  ";

            for (var i = 0; i < wrap.Wrapped.Count; i++)
            {
                this.LogWrapTree(wrap.Wrapped[i], i == wrap.Wrapped.Count - 1, indent);
            }
        }
    }
}
