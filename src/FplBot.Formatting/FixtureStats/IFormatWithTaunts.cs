namespace FplBot.Formatting.FixtureStats
{
    internal interface IFormatWithTaunts : IFormatEvents
    {
        public TauntType Type { get; }
        public string[] JokePool { get; }

    }
}
