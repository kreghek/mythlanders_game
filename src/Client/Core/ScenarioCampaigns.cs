using System;
using System.Linq;

using Client.Assets;
using Client.Assets.StageItems;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Graphs;

using Core.PropDrop;

namespace Client.Core;

internal class ScenarioCampaigns
{
    private readonly IEventCatalog _eventCatalog;

    public ScenarioCampaigns(IEventCatalog eventCatalog)
    {
        _eventCatalog = eventCatalog;
    }

    public HeroCampaign GetCampaign(string sid, Player player)
    {
        if (sid == "tutorial")
        {
            var locationSid = LocationSids.Thicket;
            var startHero = player.Heroes.Units.Single(x => x.ClassSid == "Swordsman");

            var graph = new DirectedGraph<ICampaignStageItem>();

            var node1 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem("slavic_tutorial", locationSid, _eventCatalog));
            graph.AddNode(node1);

            var combatSequence = new CombatSequence
            {
                Combats = new[]
                {
                    new CombatSource(new[]
                    {
                        new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("AmbushDrone", 0, new FieldCoords(0, 1)), ArraySegment<ICombatantStatusFactory>.Empty)
                    }, new CombatReward(Array.Empty<IDropTableScheme>()))
                }
            };

            var node2 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequence));
            graph.AddNode(node2);

            var node3 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem("slavic_tutorial", locationSid, _eventCatalog));
            graph.AddNode(node3);

            graph.ConnectNodes(node1, node2);
            graph.ConnectNodes(node2, node3);

            return new HeroCampaign(new[]
                {
                    (startHero, new FieldCoords(0, 1))
                }, new HeroCampaignLocation(LocationSids.Thicket, graph), ArraySegment<ICampaignEffect>.Empty,
                ArraySegment<ICampaignEffect>.Empty, 0);
        }

        throw new ArgumentException();
    }
}
