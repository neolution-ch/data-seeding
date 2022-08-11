namespace Neolution.Extensions.DataSeeding.Sample.Commands.Init
{
    using CommandLine;

    /// <summary>
    /// The options for the start command. This verb is marked as the default which means the start command will be executed when no verb was provided on launch.
    /// </summary>
    [Verb("init", isDefault: true, HelpText = "Data initializer.")]
    public class InitOptions
    {
    }
}
