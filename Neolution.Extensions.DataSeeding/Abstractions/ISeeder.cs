namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides seeding methods for the automatically registered seed components.
    /// </summary>
    /// <seealso cref="ISeeder" />
    public interface ISeeder
    {
        /// <summary>
        /// Seeds the specified seed component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SeedAsync();

        Task SeedAsync<T>() where T : Seed;
    }
}
