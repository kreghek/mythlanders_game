using System.Collections.Generic;
using System.Linq;

using Client.Core;

namespace Client.Assets.MonsterPerks;

public class MonsterPerkCatalog
{
    public MonsterPerkCatalog()
    {
        var factories = CatalogHelper.GetAllFactories<IMonsterPerkFactory>(typeof(IMonsterPerkFactory).Assembly);
        Perks = factories.Select(x => x.Create()).ToArray();
    }

    public IReadOnlyCollection<MonsterPerk> Perks { get; }

    public static MonsterPerk BlackMessah { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(BlackMessah)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(BlackMessah));

    public static MonsterPerk UnitedRush { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(UnitedRush)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(UnitedRush));

    public static MonsterPerk UnitedTactics { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(UnitedTactics)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(UnitedTactics));

    public static MonsterPerk DefenderOfFaith { get; } = new(new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(nameof(DefenderOfFaith)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1)),
        nameof(DefenderOfFaith));
}