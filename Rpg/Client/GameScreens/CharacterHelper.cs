using System;
using System.Linq;

using Client.Assets.GraphicConfigs.Monsters;

namespace Client.GameScreens;

internal static class CharacterHelper
{
    private static readonly (CharacterCultureSid CharacterCultureSid, string[] ClassSids)[] _map = new[]
    {
        (CharacterCultureSid.Slavic, new[]{ "Aspid", "CorruptedBear", "DigitalWolf", "HornedFrog", "Stryga", "Volkolak", "Wisp" }),
        (CharacterCultureSid.Egyptian, new[]{ "Chaser" }),
        (CharacterCultureSid.Chinese, new[]{ "Huapigui" }),
        (CharacterCultureSid.Black, new[]{ "Marauder", "BoldMarauder", "BlackTrooper" })
    };

    public static CharacterCultureSid GetCultureSid(string classSid)
    {
        return _map.Single(x => x.ClassSids.Contains(classSid)).CharacterCultureSid;
    }
}