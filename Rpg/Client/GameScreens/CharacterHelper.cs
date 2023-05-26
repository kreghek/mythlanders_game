using System;
using System.Linq;

namespace Client.GameScreens;

public static class CharacterHelper
{
    private static readonly (string CultureSid, string[] ClassSids)[] _map = new []
    {
        ("Slavic", new[]{ "Aspid", "CorruptedBear", "DigitalWolf", "HornedFrog", "Stryga", "Volkolak", "Wisp" }),
        ("Egyptian", new[]{ "Chaser" }),
        ("Chinese", new[]{ "Huapigui" }),
        ("Black", new[]{ "Marauder", "Trooper" })
    };

    public static string GetCultureSid(string classSid)
    {
        return _map.Single(x => x.ClassSids.Contains(classSid)).CultureSid;
    }
}