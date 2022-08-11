namespace Neolution.Extensions.DataSeeding
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Neolution.Extensions.DataSeeding;
    using Neolution.Extensions.DataSeeding.Abstractions;
    using Neolution.Extensions.DataSeeding.Internal;

    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the data seeding functionality to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly.</param>
        public static void AddDataSeeding(this IServiceCollection services, Assembly assembly)
        {
            services.AddSingleton<ISeeder, Seeder>();
            Seeding.Instance.Configure(services, assembly);
        }
    }
}
