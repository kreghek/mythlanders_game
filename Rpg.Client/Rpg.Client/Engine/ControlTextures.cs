using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal static class ControlTextures
    {
        public static Point Button { get; } = Point.Zero;
        public static Point Panel { get; } = CalcPointByIndex(1);
        public static Point Skill { get; } = CalcPointByIndex(2);
        public static Point Speech { get; } = CalcPointByIndex(3);
        public static Point Option { get; } = CalcPointByIndex(4);
        public static Point Shadow { get; } = CalcPointByIndex(5);

        private static Point CalcPointByIndex(int index) => new Point(index % 2, index / 2);
    }
}