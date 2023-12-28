using Client.Assets.Catalogs.Crises;
using Client.Core;

using CombatDicesTeam.Dices;

using Core.Crises;

namespace Client.Assets.StageItems;

internal sealed class FindingStageItem : SmallTextEventStageItem
{
    public FindingStageItem(IDice dice, ICrisesCatalog crisesCatalog, IEventCatalog eventCatalog) : base(dice, crisesCatalog, eventCatalog)
    {
    }

    protected override EventType EventType => EventType.Treasues;
}