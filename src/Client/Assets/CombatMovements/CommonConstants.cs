using System.IO;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements;

internal static class CommonConstants
{
    public static Point FrameSize { get; } = new(256, 128);

    public static int FrameCount { get; } = 8;

    public static string PathToCharacterSprites { get; } = Path.Combine("Sprites", "GameObjects", "Characters");
}