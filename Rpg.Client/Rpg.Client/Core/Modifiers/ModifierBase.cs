namespace Rpg.Client.Core.Modifiers
{
    internal abstract class ModifierBase
    {
        public abstract object Modify(object modifiedValue);

        public abstract ModifierType ModifierType { get; }
    }

    internal abstract class ModifierBase<TValue> : ModifierBase
    {
        public override object Modify(object modifiedValue)
        {
            if (modifiedValue is TValue value)
                return Modify(value);

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