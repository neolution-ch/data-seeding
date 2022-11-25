namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A data seed component.
    /// </summary>
    public interface ISeed
    {
        /// <summary>
        /// Gets the priority of this seed. Lower number means higher priority. Default is 1.
        /// </summary>
        public int Priority => 1;

        /// <summary>
        /// Gets the seed type this seed depends on.
        /// </summary>
        public Type? DependsOn => null;

        /// <summary>
        /// The data to seed.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SeedAsync();
    }
}
