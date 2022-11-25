namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Internal;

    /// <inheritdoc cref="ISeeder" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Resolved as a singleton by DI container")]
    internal sealed class Seeder : ISeeder, IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<Seeder> logger;

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
            this.logger.LogDebug("Determine seeding dependencies and wrap the seeds up accordingly");
            var wraps = Seeding.Instance.WrapSeeds();

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Resolved dependency tree of the seeds:");
                for (var index = 0; index < wraps.Count; index++)
                {
                    var wrap = wraps[index];
                    this.LogWrapTree(wrap, index == wrap.Wrapped.Count - 1);
                }
            }

            this.logger.LogTrace("Create a list of sorted seeds based on the dependency tree");
            var sortedSeeds = new List<ISeed>();
            Seeding.Instance.RecursiveUnwrap(wraps, sortedSeeds);

            if (this.logger.IsEnabled(LogLevel.Trace))
            {
                this.logger.LogTrace("The seeds will be seeded in the following order:");
                for (var index = 0; index < sortedSeeds.Count; index++)
                {
                    var seed = sortedSeeds[index];
                    this.logger.LogTrace($"{index + 1}.\t{seed.GetType().Name}");
                }
            }

            this.logger.LogDebug("Start seeding...");
            foreach (var seed in sortedSeeds)
            {
                await seed.SeedAsync().ConfigureAwait(false);
            }

            this.logger.LogDebug("All seeds have been seeded!");
        }

        public async Task SeedOrderedAsync(Type orderedSeedType)
        {
            var orderedSeed = Seeding.Instance.FindOrderedSeed(orderedSeedType);
            orderedSeed.InitSeeds(Seeding.Instance.Seeds);
            await orderedSeed.RunAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
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

            this.logger.LogDebug($"{indent}+- {seedTypeName}");
            indent += last ? "   " : "|  ";

            for (var i = 0; i < wrap.Wrapped.Count; i++)
            {
                this.LogWrapTree(wrap.Wrapped[i], i == wrap.Wrapped.Count - 1, indent);
            }
        }
    }

    public abstract class OrderedSeed
    {
        private bool seedsSet = false;
        private IReadOnlyList<ISeed> seeds = Enumerable.Empty<ISeed>().ToList();

        public void InitSeeds(IReadOnlyList<ISeed> seeds)
        {
            if (seedsSet)
            {
                throw new InvalidOperationException("Seeds already initiated");
            }

            this.seeds = seeds;
            this.seedsSet = true;
        }

        public async Task SeedAsync(Type seedType)
        {
            var seed = this.seeds.FirstOrDefault(x => x.GetType() == seedType);
            if (seed != null)
            {
                await seed.SeedAsync();
            }
        }

        public abstract Task RunAsync();
    }
}
