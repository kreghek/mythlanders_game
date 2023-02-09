namespace Core.Combats;

public interface IEffectImposer
{
    void Impose(IEffect effect, Combatant target, IEffectCombatContext context);

    void Update(EffectImposerUpdateType updateType);
}

public enum EffectImposerUpdateType
{
    StartRound,
    EndRound
}