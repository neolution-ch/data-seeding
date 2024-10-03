namespace Neolution.Extensions.DataSeeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Internal;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;

    /// <summary>
    /// The Seeding singleton.
    /// </summary>
    internal sealed class Seeding : IAsyncDisposable
    {
        /// <summary>
        /// The lazy singleton instantiation.
        /// </summary>
        private static readonly Lazy<Seeding> Lazy = new(() => new Seeding());

        /// <summary>
        /// The services of the application that is using seeding.
        /// </summary>
        private IServiceCollection? services;

        /// <summary>
        /// The assembly that contains the seeds.
        /// </summary>
        private Assembly? seedsAssembly;

        /// <summary>
        /// The container.
        /// </summary>
        private Container? container;

        /// <summary>
        /// The seeds
        /// </summary>
        private IReadOnlyList<Seed> seeds = Enumerable.Empty<Seed>().ToList();

        /// <summary>
        /// The scope
        /// </summary>
        private Scope? scope;

        /// <summary>
        /// Prevents a default instance of the <see cref="Seeding"/> class from being created.
        /// </summary>
        private Seeding()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        internal static Seeding Instance => Lazy.Value;

        /// <summary>
        /// Gets the seeds.
        /// </summary>
        internal IReadOnlyList<ISeed> Seeds { get; private set; } = Enumerable.Empty<ISeed>().ToList();

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (this.scope is not null)
            {
                await this.scope.DisposeAsync().ConfigureAwait(false);
            }

            if (this.container is not null)
            {
                await this.container.DisposeAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Configures the services with the internal dependency injection container and scans the specified assembly for data seeds.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly containing the <see cref="ISeed"/> implementations.</param>
        internal void Configure(IServiceCollection services, Assembly assembly)
        {
            this.services = services;
            this.seedsAssembly = assembly;
        }

        /// <summary>
        /// Uses the <see cref="IServiceProvider"/> with the internal dependency injection container.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        internal void UseServiceProvider(IServiceProvider serviceProvider)
        {
            if (this.services is null)
            {
                throw new InvalidOperationException("The service collection of the application is null. Did you call the Configure() method before calling this?");
            }

            if (this.seedsAssembly is null)
            {
                throw new InvalidOperationException("Cannot find the assembly containing the seeds. Did you call the Configure() method before calling this?");
            }

            // Create an isolated DI container just for the seeds but register the services from the application to let the seeds inject them.
            this.container = new Container();
            this.services.AddSimpleInjector(this.container);
            this.container.Collection.Register<ISeed>(new[] { this.seedsAssembly }, Lifestyle.Transient);
            this.container.Collection.Register<Seed>(new[] { this.seedsAssembly }, Lifestyle.Transient);

            serviceProvider.UseSimpleInjector(this.container);
            var logger = serviceProvider.GetRequiredService<ILogger<Seeding>>();

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Scan configured assembly for seeds");
                logger.LogTrace($"Assembly full name: '{this.seedsAssembly?.FullName}'");
            }

            // Always verify the container to avoid some runtime headaches
            this.container.Verify();

            // Open scope for container and resolve all seeds that are registered
            this.scope = AsyncScopedLifestyle.BeginScope(this.container);
            this.Seeds = this.container.GetAllInstances<ISeed>().ToList();
            this.seeds = this.container.GetAllInstances<Seed>().ToList();

            logger.LogDebug($"{this.Seeds.Count} seeds have been found and loaded");
            logger.LogDebug($"Seeding instance ready");
        }

        /// <summary>
        /// Wraps up all the seeds that do not depend on another seed.
        /// </summary>
        /// <returns>The wrapped seeds.</returns>
        internal IList<Wrap> WrapSeeds()
        {
            return this.FindDependentSeeds()
                .OrderBy(seed => seed.Priority)
                .Select(seed => this.Wrap(seed.GetType()))
                .ToList();
        }

        /// <summary>
        /// Recursively unwraps the containing seeds and push them into a sorted list.
        /// </summary>
        /// <param name="wraps">The wraps.</param>
        /// <param name="sortedSeeds">The list of already sorted seeds.</param>
        internal void RecursiveUnwrap(IEnumerable<Wrap> wraps, List<ISeed> sortedSeeds)
        {
            foreach (var wrap in wraps)
            {
                var seed = this.Unwrap(wrap);
                sortedSeeds.Add(seed);
                this.RecursiveUnwrap(wrap.Wrapped, sortedSeeds);
            }
        }

        /// <summary>
        /// Finds the seed.
        /// </summary>
        /// <typeparam name="T">The type of the seed.</typeparam>
        /// <returns>The found <see cref="Seed"/>.</returns>
        internal Seed FindSeed<T>()
            where T : Seed
        {
            return this.seeds.Single(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// Finds the seeds that depend on the specified seed type.
        /// </summary>
        /// <param name="seedType">Type of the seed.</param>
        /// <returns>The dependent seeds.</returns>
        private IEnumerable<ISeed> FindDependentSeeds(Type? seedType = null)
        {
            return this.Seeds.Where(x => x.DependsOn == seedType).ToList();
        }

        /// <summary>
        /// Recursively Wraps the specified seed type.
        /// </summary>
        /// <param name="seedType">Type of the seed.</param>
        /// <returns>The wrapped seed(s).</returns>
        private Wrap Wrap(Type? seedType)
        {
            var wrap = new Wrap { SeedType = seedType };

            var dependentSeeds = this.FindDependentSeeds(wrap.SeedType);
            foreach (var seed in dependentSeeds)
            {
                var wrapped = this.Wrap(seed.GetType());
                wrap.Wrapped.Add(wrapped);
            }

            return wrap;
        }

        /// <summary>
        /// Unwraps the specified wrap.
        /// </summary>
        /// <param name="wrap">The wrap.</param>
        /// <returns>The containing seed.</returns>
        private ISeed Unwrap(Wrap wrap)
        {
            return this.Seeds.Single(x => x.GetType() == wrap.SeedType);
        }
    }
}
