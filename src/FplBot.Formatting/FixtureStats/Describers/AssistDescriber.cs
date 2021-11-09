﻿using FplBot.Formatting.FixtureStats.Formatters;

namespace FplBot.Formatting.FixtureStats.Describers;

public class AssistDescriber : IDescribeTaunts
{
    public TauntType Type => TauntType.OutTransfers;

    public string EventDescriptionSingular => "{0} got an assist! {1}";
    public string EventDescriptionPlural => "{0} got {1} assists! {2}";
    public string EventEmoji => "🤝";

    public string[] JokePool => new []
    {
        "You can't spell \"assist\" without \"ass\". Think about that before you transfer players out, {0}",
        "I just checked whether anyone transferred him out. You did, didn't you {0}? Hahaha",
        "Hey everyone, someone transferred him out before this gameweek: {0}",
        "Kinda stupid decision tossing him out of your team, {0}"
    };
}