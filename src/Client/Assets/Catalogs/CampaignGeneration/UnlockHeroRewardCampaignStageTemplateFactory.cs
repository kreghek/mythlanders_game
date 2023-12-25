using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class UnlockHeroRewardCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly UnitName[] _heroInDev =
    {
        UnitName.Herbalist,

        UnitName.Sage,

        UnitName.Hoplite,
        UnitName.Engineer,

        UnitName.Priest,
        UnitName.Liberator,
        UnitName.Medjay,

        UnitName.Zoologist,
        UnitName.Assaulter
    };

    private readonly CampaignStageTemplateServices _services;

    public UnlockHeroRewardCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    private IReadOnlyCollection<UnitName> CalculateLockedHeroes()
    {
        return _services.UnitSchemeCatalog.Heroes.Select(x => x.Value.Name)
            .Except(_services.GlobeProvider.Globe.Player.Heroes.Select(x => Enum.Parse<UnitName>(x.ClassSid, true)))
            .Except(_heroInDev)
            .ToArray();
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var heroesToJoin = CalculateLockedHeroes();
        if (!heroesToJoin.Any())
        {
            return false;
        }

        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var heroesToJoin = CalculateLockedHeroes();

        var rolledHero = _services.Dice.RollFromList(heroesToJoin.ToArray());

        return new UnlockHeroRewardStageItem(_services.GlobeProvider, _services.JobProgressResolver, rolledHero,
            _services.UnitSchemeCatalog);
    }

    /// <inheritdoc />
    public IGraphNode<ICampaignStageItem> Create(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return new GraphNode<ICampaignStageItem>(Create(MapContextToCurrentStageItems(context)));
    }

    /// <inheritdoc />
    public bool CanCreate(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return CanCreate(MapContextToCurrentStageItems(context));
    }
}