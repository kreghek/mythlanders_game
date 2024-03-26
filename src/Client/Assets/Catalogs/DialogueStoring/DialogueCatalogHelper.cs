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
    public static Dialogue<TParagraphConditionContext, TAftermathContext> Create<TParagraphConditionContext,
        TAftermathContext>(string dialogueSid,
        IDictionary<string, DialogueDtoScene> scenesDtoDict,
        DialogueCatalogCreationServices<TAftermathContext> services,
        Func<IDialogueSpeaker, IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>>>
            defaultSpeakerReactionConditionFactory)
    {
        var nodeListDicts =
            new List<(string nodeSid, DialogueNode<TParagraphConditionContext, TAftermathContext> node,
                List<DialogueOption<TParagraphConditionContext, TAftermathContext>> optionsList, DialogueDtoOption
                []?
                optionsDto)>();

        foreach (var (sceneSid, dtoScene) in scenesDtoDict)
        {
            var speeches = new List<DialogueSpeech<TParagraphConditionContext, TAftermathContext>>();

            for (var paragraphIndex = 0; paragraphIndex < dtoScene.Paragraphs.Length; paragraphIndex++)
            {
                var dialogueDtoParagraph = dtoScene.Paragraphs[paragraphIndex];

                var environmentEffects = CreateEnvironmentEffects(dialogueDtoParagraph.Env, services.EnvEffectCreator);

                if (dialogueDtoParagraph.Text is not null)
                {
                    // Regular paragraph
                    var paragraphContext =
                        new DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext>
                        {
                            Aftermaths = environmentEffects
                        };

                    var speach = new DialogueSpeech<TParagraphConditionContext, TAftermathContext>(
                        GetSpeaker(dialogueDtoParagraph.Speaker),
                        $"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}",
                        paragraphContext);

                    speeches.Add(speach);
                }
                else if (dialogueDtoParagraph.Reactions is not null)
                {
                    // Reactions of the heroes

                    foreach (var reaction in dialogueDtoParagraph.Reactions)
                    {
                        var paragraphContext =
                            new DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext>
                            {
                                Aftermaths = environmentEffects,
                                Conditions = defaultSpeakerReactionConditionFactory(GetSpeaker(reaction.Hero))
                            };

                        var speech = new DialogueSpeech<TParagraphConditionContext, TAftermathContext>(
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

            var options = new List<DialogueOption<TParagraphConditionContext, TAftermathContext>>();
            var dialogNode = new DialogueNode<TParagraphConditionContext, TAftermathContext>(
                new DialogueParagraph<TParagraphConditionContext, TAftermathContext>(speeches),
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

                    DialogueOption<TParagraphConditionContext, TAftermathContext> dialogueOption;
                    if (dialogueDtoOption.Next is not null)
                    {
                        var next = nodeListDicts.Single(x => x.nodeSid == dialogueDtoOption.Next).node;
                        dialogueOption =
                            new DialogueOption<TParagraphConditionContext, TAftermathContext>(
                                $"{dialogueSid}_Scene_{nodeSid}_Option_{optionIndex}", next)
                            {
                                Aftermath = aftermaths
                            };
                    }
                    else
                    {
                        dialogueOption =
                            new DialogueOption<TParagraphConditionContext, TAftermathContext>(
                                "Common_end_dialogue",
                                DialogueNode<TParagraphConditionContext, TAftermathContext>.EndNode)
                            {
                                Aftermath = aftermaths
                            };
                    }

                    optionsList.Add(dialogueOption);
                }
            }
            else
            {
                var dialogueOption = new DialogueOption<TParagraphConditionContext, TAftermathContext>(
                    "Common_end_dialogue", DialogueNode<TParagraphConditionContext, TAftermathContext>.EndNode);
                optionsList.Add(dialogueOption);
            }
        }

        return new Dialogue<TParagraphConditionContext, TAftermathContext>(nodeListDicts
            .Single(x => x.nodeSid == "root")
            .node);
    }

    private static IDialogueOptionAftermath<TAftermathContext>? CreateAftermaths<TAftermathContext>(
        DialogueDtoData[]? aftermathDtos,
        IDialogueOptionAftermathCreator<TAftermathContext> aftermathCreator)
    {
        if (aftermathDtos is null)
        {
            return null;
        }

        var list = new List<IDialogueOptionAftermath<TAftermathContext>>();

        foreach (var aftermathDto in aftermathDtos)
        {
            var aftermath = aftermathCreator.Create(aftermathDto.Type, aftermathDto.Data);
            list.Add(aftermath);
        }

        return new CompositeOptionAftermath<TAftermathContext>(list);
    }

    private static IReadOnlyCollection<IDialogueOptionAftermath<TAftermathContext>> CreateEnvironmentEffects<
        TAftermathContext>(
        DialogueDtoData[]? envs,
        IDialogueEnvironmentEffectCreator<TAftermathContext> environmentEffectCreator)
    {
        if (envs is null)
        {
            return Array.Empty<IDialogueOptionAftermath<TAftermathContext>>();
        }

        var list = new List<IDialogueOptionAftermath<TAftermathContext>>();

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