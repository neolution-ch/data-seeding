namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class OrderedSeed
    {
        protected async Task SeedAsync(Type seedType)
        {
            var seed = Seeding.Instance.Seeds.FirstOrDefault(x => x.GetType() == seedType);
            if (seed != null)
            {
                await seed.SeedAsync();
            }
        }

        public abstract Task RunAsync();
    }
}
