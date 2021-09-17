namespace Rpg.Client.Core.Effects
{
    internal abstract class InstantenousEffectBase : EffectBase
    {
        public InstantenousEffectBase()
        {
            Imposed += InstantenousEffect_Imposed;
        }

        private void InstantenousEffect_Imposed(object? sender, CombatUnit e)
        {
            Imposed -= InstantenousEffect_Imposed;
            Influence();
            Dispel();
        }
    }
}