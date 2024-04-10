using Microsoft.Xna.Framework;

namespace Client.Assets.MonsterPerks;

public static class IconHelper
{
    public static Point GetMonsterPerkIconIndex(int column, int row)
    {
        return new Point(column, row);
    }
}