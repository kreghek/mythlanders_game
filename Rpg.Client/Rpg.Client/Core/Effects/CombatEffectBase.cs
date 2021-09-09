namespace Rpg.Client.Core.Effects
{
    internal abstract class CombatEffectBase
    {
        public abstract void Execute(ActiveCombat combat, CombatUnit target);
    }
}