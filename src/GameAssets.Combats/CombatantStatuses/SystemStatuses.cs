using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

public static class SystemStatuses
{
    public static ICombatantStatus Biological { get; } = new LifeFormCombatantStatus(LifeForm.Biological);
    public static ICombatantStatus Cyborg { get; } = new LifeFormCombatantStatus(LifeForm.Cyborg);
    public static ICombatantStatus Energy { get; } = new LifeFormCombatantStatus(LifeForm.Energy);
    public static ICombatantStatus Humanoid { get; } = new LifeFormCombatantStatus(LifeForm.Humanoid);
    public static ICombatantStatus Beast { get; } = new LifeFormCombatantStatus(LifeForm.Beast);
}