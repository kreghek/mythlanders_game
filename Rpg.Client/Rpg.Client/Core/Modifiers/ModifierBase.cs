namespace Rpg.Client.Core.Modifiers
{
    internal abstract class ModifierBase : IUnitModifier
    {
        public abstract ModifierType Type { get; }
    }

    internal enum ModifierType
    { 
        Undefined,
        Power
    }
}