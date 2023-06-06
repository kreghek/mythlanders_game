using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal interface ICombatTemplateFactory
{
    IReadOnlyCollection<MonsterCombatantTempate> CreateSet();
}