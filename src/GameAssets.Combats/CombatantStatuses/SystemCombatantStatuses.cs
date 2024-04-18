using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

public static class SystemCombatantStatuses
{
    public static ICombatantStatus Biological { get; } = new LifeFormCombatantStatus(LifeForm.Biological);
    public static ICombatantStatus Mechanical { get; } = new LifeFormCombatantStatus(LifeForm.Cyborg);
    public static ICombatantStatus Energy { get; } = new LifeFormCombatantStatus(LifeForm.Energy);
    public static ICombatantStatus Humanoid { get; } = new LifeFormCombatantStatus(LifeForm.Humanoid);
    public static ICombatantStatus Beast { get; } = new LifeFormCombatantStatus(LifeForm.Beast);
    public static ICombatantStatus Mutant { get; } = new LifeFormCombatantStatus(LifeForm.Mutant);
    public static ICombatantStatus Construct { get; } = new LifeFormCombatantStatus(LifeForm.Construct);
}