namespace Rpg.Client.Core
{
    public interface IStatValue
    {
        int ActualMax { get; }
        int Current { get; }
        void AddModifier(IUnitStatModifier modifier);
        void ChangeBase(int newBase);
        void Consume(int value);
        void CurrentChange(int newCurrent);
        void RemoveModifier(StatModifier modifier);
        void Restore(int value);
    }

    public static class IStatValueExtensions
    {
        public static double GetShare(this IStatValue source)
        {
            return (double)source.Current / source.ActualMax;
        }

        public static void Restore(this IStatValue source)
        {
            source.Restore(source.ActualMax);
        }
    }
}