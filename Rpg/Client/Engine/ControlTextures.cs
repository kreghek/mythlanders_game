using Microsoft.Xna.Framework;

namespace Rpg.Client.Engine
{
    internal static class ControlTextures
    {
        private const int COL_COUNT = 3;
        private const int TEXTURE_SIZE = 32;

        public static Point Button { get; } = Point.Zero;
        public static Point CombatMove { get; } = CalcPointByIndex(2);
        public static Point OptionHover { get; } = CalcPointByIndex(5);
        public static Point OptionNormal { get; } = CalcPointByIndex(4);
        public static Point Panel { get; } = CalcPointByIndex(1);
        public static Point PanelBlack { get; } = CalcPointByIndex(6);
        public static Point Shadow { get; } = CalcPointByIndex(6);
        public static Point Speech { get; } = CalcPointByIndex(3);
        public static Point Transparent { get; } = CalcPointByIndex(8);
        public static Point Button2 { get; } = CalcPointByIndex(7);

        private static Point CalcPointByIndex(int index)
        {
            return new(index % COL_COUNT * TEXTURE_SIZE, index / COL_COUNT * TEXTURE_SIZE);
        }
    }
}