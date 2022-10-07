namespace Rpg.Client.Core
{
    public interface IUnitStatModifier
    {
        int GetBonus(int currentBaseValue);
    }
}