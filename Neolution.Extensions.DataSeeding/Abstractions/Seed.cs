namespace Neolution.Extensions.DataSeeding.Abstractions
{
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Seed
    {
        protected async Task SeedAsync<T>() where T : ISeed
        {
            var seed = Seeding.Instance.Seeds.FirstOrDefault(x => x.GetType() == typeof(T));
            if (seed != null)
            {
                await seed.SeedAsync();
            }
        }

        public abstract Task SeedAsync();
    }
}
