namespace Neolution.Extensions.DataSeeding.Abstractions
{
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
        /// <returns>An awaitable task.</returns>
        Task SeedAsync();
    }
}
