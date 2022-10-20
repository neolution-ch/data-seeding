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
