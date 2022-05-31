using System;

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
            if (modifiedValue is not TValue value)
            {
                return modifiedValue;
            }

            var newValue = Modify(value);
            if (newValue is null)
            {
                throw new InvalidOperationException();
            }

            return newValue;
        }

        protected abstract TValue Modify(TValue modifiedValue);
    }
}