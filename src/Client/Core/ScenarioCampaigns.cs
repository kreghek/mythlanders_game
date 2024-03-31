using System;
using System.Linq;

using Client.Assets;
using Client.Assets.StageItems;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Graphs;

using Core.PropDrop;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Core;

internal sealed class ScenarioCampaigns
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

            var tutorialHero = GetTutorialHero(player, sid);

            var tutorialDialogueSid = GetTutorialDialogueByTutorialHero(tutorialHero);

            var graph = new DirectedGraph<ICampaignStageItem>();

            var node1 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem(tutorialDialogueSid, locationSid,
                _eventCatalog));
            graph.AddNode(node1);

            var combatSequenceT1 = new CombatSequence
            {
                Combats = new[]
                {
                    new CombatSource(new[]
                    {
                        new PerkMonsterCombatantPrefab(
                            new MonsterCombatantPrefab("DigitalWolf", 0, new FieldCoords(0, 1)), new[]
                            {
                                new CombatStatusFactory(source =>
                                    new AutoRestoreModifyStatCombatantStatus(
                                        new ModifyStatCombatantStatus(new CombatantStatusSid("Wound"),
                                            new OwnerBoundCombatantEffectLifetime(), source,
                                            CombatantStatTypes.HitPoints, -4)))
                            })
                    }, new CombatReward(Array.Empty<IDropTableScheme>()))
                }
            };

            var node2 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT1));
            graph.AddNode(node2);

            var node3 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem(tutorialDialogueSid, locationSid,
                _eventCatalog));
            graph.AddNode(node3);

            var combatSequenceT2 = new CombatSequence
            {
                Combats = new[]
                {
                    new CombatSource(new[]
                    {
                        new PerkMonsterCombatantPrefab(
                            new MonsterCombatantPrefab("DigitalWolf", 0, new FieldCoords(0, 0)),
                            Array.Empty<ICombatantStatusFactory>()),
                        new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("Aspid", 0, new FieldCoords(0, 1)),
                            Array.Empty<ICombatantStatusFactory>())
                    }, new CombatReward(Array.Empty<IDropTableScheme>()))
                }
            };

            var node4 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT2));
            graph.AddNode(node3);

            graph.ConnectNodes(node1, node2);
            graph.ConnectNodes(node2, node3);
            graph.ConnectNodes(node3, node4);

            return new HeroCampaign(new[]
                {
                    (tutorialHero, new FieldCoords(0, 1))
                }, new HeroCampaignLocation(LocationSids.Thicket, graph), ArraySegment<ICampaignEffect>.Empty,
                ArraySegment<ICampaignEffect>.Empty, 0);
        }

        throw new ArgumentException();
    }

    private string GetTutorialDialogueByTutorialHero(HeroState tutorialHero)
    {
        if (tutorialHero.ClassSid == "Swordsman")
        {
            return "slavic_tutorial";
        }

        if (tutorialHero.ClassSid == "Monk")
        {
            return "chinese_tutorial";
        }

        if (tutorialHero.ClassSid == "Hoplite")
        {
            return "greek_tutorial";
        }

        if (tutorialHero.ClassSid == "Liberator")
        {
            return "egypt_tutorial";
        }

        throw new InvalidOperationException();
    }

    private static HeroState GetTutorialHero(Player player, string sid)
    {
        return player.Heroes.Units.First();
    }
}