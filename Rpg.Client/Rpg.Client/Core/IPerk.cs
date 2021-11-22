namespace Rpg.Client.Core
{
    public interface IPerk
    {
        void ApplyToStats(ref int maxHitpoints);
    }
}