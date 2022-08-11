# DataSeeding

## Introduction 
From the [database seeding](https://en.wikipedia.org/wiki/Database_seeding) article in Wikipedia:
> Database seeding is populating a database with an initial set of data. It's common to load seed data such as initial user accounts or dummy data upon initial setup of an application.

The code needed to create an initial state of data for an application can get overwhelmingly complex, even for smaller applications. This means that the source code file responsible for creating, generating, initializing, loading, transforming (...) your data, will eventually end up having tons of different dependencies. When not properly organized, this could create maintainability issues for your application.

This library aims to help developers to divide the whole data seeding logic of an application into small chunks of logic. 

## Getting Started
In most use-cases there are only two relevant interfaces:

### Add the functionality to the dependency injection container
For Microsoft Dependency Injection, there is already an extension method built in:

    services.AddDataSeeding(typeof(Startup).Assembly);

This configures DataSeeding to scan the passed assembly to look for `ISeed` implementations. All found implementations will then prepared and properly ordered for the data seeding. 

### The `ISeed` interface
The class that contains a chunk of the data seeding logic is called **seed**. To make the library pick up your seeds, they have to implement `ISeed` interface.

In the `SeedAsync()` method you can add your data seeding logic. The seed will be instantiated by the dependency injection container, so you can use the constructor to inject services.

Sometimes, you might want a certain seed to be seeded before another one. This can be achieved by implementing the optional `DependsOn` property in the dependent seed. Make the property return the type of the seed you want to have seeded before the current seed. As soon as the `ISeeder` is resolved, the optimal order of the seeds will be determined.

### The `ISeeder` interface
The `ISeeder` interface can be resolved from the service provider. It contains the logic to find all seeds in a specified assembly and seed them in an appropriate order.
    
    public class DataInitializer
    {
        private readonly ISeeder seeder;

        public DataInitializer(ISeeder seeder)
        {
            this.seeder = seeder;
        }

        public void Run()
        {
            this.seeder.SeedAsync();
        }
    }

### Samples
Check out the sample console application and the unit test in the repository.