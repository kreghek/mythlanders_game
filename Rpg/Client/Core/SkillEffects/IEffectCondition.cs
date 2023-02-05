namespace Rpg.Client.Core.SkillEffects
{
    internal interface IEffectCondition
    {
        bool Check(ICombatUnit target, CombatEffectContext effectContext);
        string GetDescription();
    }
}