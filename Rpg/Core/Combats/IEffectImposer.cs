namespace Core.Combats;

public interface IEffectImposer
{
    void Impose(IEffect effect, Combatant target, IEffectCombatContext context);
}
