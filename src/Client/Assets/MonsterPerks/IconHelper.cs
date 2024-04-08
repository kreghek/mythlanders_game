namespace Client.Assets.MonsterPerks;

public static class IconHelper
{
    private const int COL_COUNT = 3;

    public static int GetMonsterPerkIconIndex(int column, int row)
    {
        return column + COL_COUNT * row;
    }
}