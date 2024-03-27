using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal static class DialogueCatalogHelper
{
    public static Dialogue<ParagraphConditionContext, CampaignAftermathContext> Create(string dialogueSid,
        IDictionary<string, DialogueDtoScene> scenesDtoDict,
        DialogueCatalogCreationServices services)
    {
        var nodeListDicts =
            new List<(string nodeSid, DialogueNode<ParagraphConditionContext, CampaignAftermathContext> node,
                List<DialogueOption<ParagraphConditionContext, CampaignAftermathContext>> optionsList, DialogueDtoOption
                []?
                optionsDto)>();

        foreach (var (sceneSid, dtoScene) in scenesDtoDict)
        {
            var speeches = new List<DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext>>();

            for (var paragraphIndex = 0; paragraphIndex < dtoScene.Paragraphs.Length; paragraphIndex++)
            {
                var dialogueDtoParagraph = dtoScene.Paragraphs[paragraphIndex];

                var environmentEffects = CreateEnvironmentEffects(dialogueDtoParagraph.Env, services.EnvEffectCreator);

                if (dialogueDtoParagraph.Text is not null)
                {
                    // Regular paragraph
                    var paragraphContext =
                        new DialogueParagraphConfig<ParagraphConditionContext, CampaignAftermathContext>
                        {
                            Aftermaths = environmentEffects
                        };

                    var speech = new DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext>(
                        GetSpeaker(dialogueDtoParagraph.Speaker),
                        $"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}",
                        paragraphContext);

                    speeches.Add(speech);
                }
                else if (dialogueDtoParagraph.Reactions is not null)
                {
                    // Reactions of the heroes

                    foreach (var reaction in dialogueDtoParagraph.Reactions)
                    {
                        var paragraphContext =
                            new DialogueParagraphConfig<ParagraphConditionContext, CampaignAftermathContext>
                            {
                                Aftermaths = environmentEffects,
                                Conditions = new[] { new HasHeroParagraphCondition(GetSpeaker(reaction.Hero)) }
                            };

                        var speech = new DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext>(
                            GetSpeaker(reaction.Hero),
                            $"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}_reaction_{reaction.Hero}",
                            paragraphContext);

                        speeches.Add(speech);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Text or reactions must be assigned.");
                }
            }

            var options = new List<DialogueOption<ParagraphConditionContext, CampaignAftermathContext>>();
            var dialogNode = new DialogueNode<ParagraphConditionContext, CampaignAftermathContext>(
                new DialogueParagraph<ParagraphConditionContext, CampaignAftermathContext>(speeches),
                options);

            nodeListDicts.Add((sceneSid, dialogNode, options, dtoScene.Options));
        }

        // Linking scenes via player's options
        foreach (var (nodeSid, _, optionsList, optionsDto) in nodeListDicts)
        {
            if (optionsDto is not null)
            {
                for (var optionIndex = 0; optionIndex < optionsDto.Length; optionIndex++)
                {
                    var dialogueDtoOption = optionsDto[optionIndex];
                    var aftermaths = CreateAftermaths(dialogueDtoOption.Aftermaths, services.OptionAftermathCreator);

                    DialogueOption<ParagraphConditionContext, CampaignAftermathContext> dialogueOption;
                    if (dialogueDtoOption.Next is not null)
                    {
                        var next = nodeListDicts.Single(x => x.nodeSid == dialogueDtoOption.Next).node;
                        dialogueOption =
                            new DialogueOption<ParagraphConditionContext, CampaignAftermathContext>(
                                $"{dialogueSid}_Scene_{nodeSid}_Option_{optionIndex}", next)
                            {
                                Aftermath = aftermaths
                            };
                    }
                    else
                    {
                        dialogueOption =
                            new DialogueOption<ParagraphConditionContext, CampaignAftermathContext>(
                                "Common_end_dialogue",
                                DialogueNode<ParagraphConditionContext, CampaignAftermathContext>.EndNode)
                            {
                                Aftermath = aftermaths
                            };
                    }

                    optionsList.Add(dialogueOption);
                }
            }
            else
            {
                var dialogueOption = new DialogueOption<ParagraphConditionContext, CampaignAftermathContext>(
                    "Common_end_dialogue", DialogueNode<ParagraphConditionContext, CampaignAftermathContext>.EndNode);
                optionsList.Add(dialogueOption);
            }
        }

        return new Dialogue<ParagraphConditionContext, CampaignAftermathContext>(nodeListDicts
            .Single(x => x.nodeSid == "root")
            .node);
    }

    private static IDialogueOptionAftermath<CampaignAftermathContext>? CreateAftermaths(
        DialogueDtoData[]? aftermathDtos,
        IDialogueOptionAftermathCreator aftermathCreator)
    {
        if (aftermathDtos is null)
        {
            return null;
        }

        var list = new List<IDialogueOptionAftermath<CampaignAftermathContext>>();

        foreach (var aftermathDto in aftermathDtos)
        {
            var aftermath = aftermathCreator.Create(aftermathDto.Type, aftermathDto.Data);
            list.Add(aftermath);
        }

        return new CompositeOptionAftermath(list);
    }

    private static IReadOnlyCollection<IDialogueOptionAftermath<CampaignAftermathContext>> CreateEnvironmentEffects(
        DialogueDtoData[]? envs,
        IDialogueParagraphEffectCreator environmentEffectCreator)
    {
        if (envs is null)
        {
            return Array.Empty<IDialogueOptionAftermath<CampaignAftermathContext>>();
        }

        var list = new List<IDialogueOptionAftermath<CampaignAftermathContext>>();

        foreach (var envDto in envs)
        {
            var envEffect = environmentEffectCreator.Create(envDto.Type, envDto.Data);
            list.Add(envEffect);
        }

        return list;
    }

    private static IDialogueSpeaker GetSpeaker(string? dtoSpeaker)
    {
        if (!Enum.TryParse<UnitName>(dtoSpeaker, ignoreCase: true, out var unitName))
        {
            unitName = UnitName.Environment;
        }

        return new DialogueSpeaker(unitName);
    }
}