namespace Neolution.Extensions.DataSeeding.UnitTests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes;
    using Neolution.Extensions.DataSeeding.UnitTests.Fakes.MultiTenantSeeds;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Basic Tests.
    /// </summary>
    public class BasicTests
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        public BasicTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Asserts that the seeder runs without any exceptions. The seeded seeds should log to console.
        /// </summary>
        [Fact]
        public void SeedsCanBeSeededWithLoggingTest()
        {
            // Assign
            var services = this.CreateServiceCollection();
            services.AddDataSeeding(typeof(TenantsSeed).Assembly);
            services.AddTransient<DataInitializerFake>();
            var serviceProvider = services.BuildServiceProvider();
            var dataInitializer = serviceProvider.GetRequiredService<DataInitializerFake>();

            // Act
            var dataInitializerRun = dataInitializer.Run();

            // Assert
            dataInitializerRun.ShouldBeTrue();
        }

        /// <summary>
        /// Creates the service collection.
        /// </summary>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        private IServiceCollection CreateServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(this.testOutputHelper));
            return services;
        }
    }
}
