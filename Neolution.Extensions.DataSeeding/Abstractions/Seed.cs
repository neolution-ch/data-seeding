namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The seed.
    /// </summary>
    public abstract class Seed
    {
        /// <summary>
        /// Seeds the seed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public abstract Task SeedAsync();

        /// <summary>
        /// Seeds the seed component.
        /// </summary>
        /// <typeparam name="T">The seed.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected static async Task SeedAsync<T>()
            where T : ISeed
        {
            var seed = Seeding.Instance.Seeds.FirstOrDefault(x => x.GetType() == typeof(T));
            if (seed != null)
            {
                await seed.SeedAsync().ConfigureAwait(false);
            }
        }
    }
}
