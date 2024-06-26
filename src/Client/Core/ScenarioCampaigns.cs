﻿using System;
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
            return CreateTutorialCampaign(player);
        }

        if (sid == "MainPlotEpisode1Scene1")
        {
            return CreateMainPlotEpisode1Scene1Campaign(player);
        }

        throw new ArgumentException("Invalid campaign sid", nameof(sid));
    }

    private HeroCampaign CreateMainPlotEpisode1Scene1Campaign(Player player)
    {
        var locationSid = LocationSids.Thicket;

        const string DIALOGUE_SID = "main_plot_e1_scene1";

        var graph = new DirectedGraph<ICampaignStageItem>();

        var combatSequenceT1 = new CombatSequence
        {
            Combats = new[]
            {
                new CombatSource(new[]
                {
                    new PerkMonsterCombatantPrefab(
                        new MonsterCombatantPrefab("DigitalWolf", 0, new FieldCoords(0, 0)),
                        Array.Empty<MonsterPerk>()),
                    new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("Aspid", 0, new FieldCoords(0, 1)),
                        Array.Empty<MonsterPerk>())
                }, new CombatReward(Array.Empty<IDropTableScheme>()))
            }
        };

        var node2 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT1,
            CombatStageHelper.CreateMetadata(combatSequenceT1.Combats.First())));
        graph.AddNode(node2);

        var node8 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem(DIALOGUE_SID, locationSid,
            _eventCatalog)
        {
            IsGoalStage = true
        });
        graph.AddNode(node8);

        graph.ConnectNodes(node2, node8);

        return new HeroCampaign(new[]
            {
                (player.Heroes.ToArray()[0], new FieldCoords(0, 1)),
                (player.Heroes.ToArray()[1], new FieldCoords(1, 1)),
                (player.Heroes.ToArray()[2], new FieldCoords(1, 0))
            },
            new HeroCampaignLocation(LocationSids.Thicket, graph),
            new ICampaignEffect[]
            {
                new CompleteDemoCampaignEffect()
            },
            ArraySegment<ICampaignEffect>.Empty,
            10);
    }

    private HeroCampaign CreateTutorialCampaign(Player player)
    {
        var locationSid = LocationSids.Thicket;

        var tutorialHero = GetTutorialHero(player);

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
                            new MonsterPerk(
                                new CombatStatusFactory(source =>
                                    new AutoRestoreModifyStatCombatantStatus(
                                        new ModifyStatCombatantStatus(new CombatantStatusSid("Wound"),
                                            new OwnerBoundCombatantEffectLifetime(), source,
                                            CombatantStatTypes.HitPoints, -4))),
                                "Wound"
                            )
                        })
                }, new CombatReward(Array.Empty<IDropTableScheme>()))
            }
        };

        var node2 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT1,
            CombatStageHelper.CreateMetadata(combatSequenceT1.Combats.First())));
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
                        Array.Empty<MonsterPerk>()),
                    new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("Aspid", 0, new FieldCoords(0, 1)),
                        Array.Empty<MonsterPerk>())
                }, new CombatReward(Array.Empty<IDropTableScheme>()))
            }
        };

        var node4 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT2,
            CombatStageHelper.CreateMetadata(combatSequenceT2.Combats.First())));
        graph.AddNode(node4);

        var node5 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem(tutorialDialogueSid, locationSid,
            _eventCatalog));
        graph.AddNode(node5);

        var node5Rest = new GraphNode<ICampaignStageItem>(new RestStageItem(_eventCatalog));
        graph.AddNode(node5Rest);

        var combatSequenceT3Easy = new CombatSequence
        {
            Combats = new[]
            {
                new CombatSource(new[]
                {
                    new PerkMonsterCombatantPrefab(
                        new MonsterCombatantPrefab("DigitalWolf", 0, new FieldCoords(0, 0)),
                        Array.Empty<MonsterPerk>()),
                    new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("Aspid", 0, new FieldCoords(0, 2)),
                        Array.Empty<MonsterPerk>())
                }, new CombatReward(Array.Empty<IDropTableScheme>()))
            }
        };

        var combatSequenceT3Medium = new CombatSequence
        {
            Combats = new[]
            {
                new CombatSource(new[]
                {
                    new PerkMonsterCombatantPrefab(
                        new MonsterCombatantPrefab("DigitalWolf", 0, new FieldCoords(0, 0)),
                        Array.Empty<MonsterPerk>()),
                    new PerkMonsterCombatantPrefab(new MonsterCombatantPrefab("Aspid", 0, new FieldCoords(0, 1)),
                        Array.Empty<MonsterPerk>()),
                    new PerkMonsterCombatantPrefab(
                        new MonsterCombatantPrefab("DigitalWolf", 1, new FieldCoords(1, 1)),
                        Array.Empty<MonsterPerk>())
                }, new CombatReward(Array.Empty<IDropTableScheme>()))
            }
        };

        var node6_1 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT3Easy,
            CombatStageHelper.CreateMetadata(combatSequenceT3Easy.Combats.First(), CombatEstimateDifficulty.Easy)));
        graph.AddNode(node6_1);

        var node6_2 = new GraphNode<ICampaignStageItem>(new CombatStageItem(locationSid, combatSequenceT3Medium,
            CombatStageHelper.CreateMetadata(combatSequenceT3Medium.Combats.First(), CombatEstimateDifficulty.Hard)));
        graph.AddNode(node6_2);

        var node8 = new GraphNode<ICampaignStageItem>(new DialogueEventStageItem(tutorialDialogueSid, locationSid,
            _eventCatalog)
        {
            IsGoalStage = true
        });
        graph.AddNode(node8);

        graph.ConnectNodes(node1, node2);
        graph.ConnectNodes(node2, node3);
        graph.ConnectNodes(node3, node4);
        graph.ConnectNodes(node4, node5);
        graph.ConnectNodes(node5, node5Rest);
        graph.ConnectNodes(node5Rest, node6_1);
        graph.ConnectNodes(node5Rest, node6_2);
        graph.ConnectNodes(node6_1, node8);
        graph.ConnectNodes(node6_2, node8);

        return new HeroCampaign(new[]
            {
                (tutorialHero, new FieldCoords(0, 1))
            },
            new HeroCampaignLocation(LocationSids.Thicket, graph),
            new ICampaignEffect[]
            {
                new UnlockFeatureCampaignEffect(GameFeatures.ExecutableQuests)
            },
            ArraySegment<ICampaignEffect>.Empty,
            1);
    }

    private static string GetTutorialDialogueByTutorialHero(HeroState tutorialHero)
    {
        if (tutorialHero.ClassSid == "Bogatyr")
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

    private static HeroState GetTutorialHero(Player player)
    {
        return player.Heroes.Single();
    }
}