namespace Neolution.Extensions.DataSeeding.Internal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The wrap that contains a seed type and maybe even another wrap.
    /// </summary>
    internal class Wrap
    {
        /// <summary>
        /// Gets or sets the type of the seed.
        /// </summary>
        public Type? SeedType { get; set; }

        /// <summary>
        /// Gets the wrapped seeds.
        /// </summary>
        public IList<Wrap> Wrapped { get; } = new List<Wrap>();
    }
}
