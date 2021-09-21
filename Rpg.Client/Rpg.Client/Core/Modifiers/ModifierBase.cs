namespace Rpg.Client.Core.Modifiers
{
    internal abstract class ModifierBase
    {
        public abstract ModifierType ModifierType { get; }
        public abstract object Modify(object modifiedValue);
    }

    internal abstract class ModifierBase<TValue> : ModifierBase
    {
        public override object Modify(object modifiedValue)
        {
            if (modifiedValue is TValue value)
            {
                return Modify(value);
            }

            return modifiedValue;
        }

        public abstract TValue Modify(TValue modifiedValue);
    }

    internal enum ModifierType
    {
        GivenDamage,
        TakenDamage,
        GivenHeal,
        TakenHeal
    }
}