namespace FplBot.Core.Helpers.Formatting.FixtureStats
{
    internal interface IFormatEvents : IFormat
    {
        string EventDescriptionSingular { get; }
        string EventDescriptionPlural { get; }

        string EventEmoji { get; }
    }
}