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
        DialogueCatalogCreationServices<TParagraphConditionContext, TAftermathContext> services,
        Func<IDialogueSpeaker, IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>>>
            defaultSpeakerReactionConditionFactory)
    {
        var nodeListDictionaries =
            new List<(string nodeSid, DialogueNode<TParagraphConditionContext, TAftermathContext> node,
                List<DialogueOption<TParagraphConditionContext, TAftermathContext>> optionsList, DialogueDtoOption
                []?
                optionsDto)>();

        foreach (var (sceneSid, dtoScene) in scenesDtoDict)
        {
            if (dtoScene.Paragraphs is null)
            {
                continue;
            }

            var speeches = new List<DialogueSpeech<TParagraphConditionContext, TAftermathContext>>();

            for (var paragraphIndex = 0; paragraphIndex < dtoScene.Paragraphs.Length; paragraphIndex++)
            {
                var dialogueDtoParagraph = dtoScene.Paragraphs[paragraphIndex];

                var environmentEffects =
                    CreateParagraphEffects(dialogueDtoParagraph.Env, services.ParagraphEffectCreator);

                if (dialogueDtoParagraph.Text is not null)
                {
                    // Regular paragraph
                    var paragraphContext =
                        new DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext>
                        {
                            Aftermaths = environmentEffects
                        };

                    var speech = new DialogueSpeech<TParagraphConditionContext, TAftermathContext>(
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

            nodeListDictionaries.Add((sceneSid, dialogNode, options, dtoScene.Options));
        }

        // Linking scenes via player's options
        foreach (var (nodeSid, _, optionsList, optionsDto) in nodeListDictionaries)
        {
            if (optionsDto is not null)
            {
                for (var optionIndex = 0; optionIndex < optionsDto.Length; optionIndex++)
                {
                    var dialogueDtoOption = optionsDto[optionIndex];
                    var aftermaths = CreateAftermaths(dialogueDtoOption.Aftermaths, services.OptionAftermathCreator);
                    var hideConditions = CreateOptionConditions(dialogueDtoOption.HideConditions,
                        services.ParagraphConditionCreator);
                    var selectConditions = CreateOptionConditions(dialogueDtoOption.SelectConditions,
                        services.ParagraphConditionCreator);

                    DialogueOption<TParagraphConditionContext, TAftermathContext> dialogueOption;
                    if (dialogueDtoOption.Next is not null)
                    {
                        var next = nodeListDictionaries.Single(x => x.nodeSid == dialogueDtoOption.Next).node;
                        dialogueOption =
                            new DialogueOption<TParagraphConditionContext, TAftermathContext>(
                                $"{dialogueSid}_Scene_{nodeSid}_Option_{optionIndex}", next)
                            {
                                Aftermath = aftermaths,
                                HideConditions = hideConditions,
                                SelectConditions = selectConditions,
                                DescriptionSid = dialogueDtoOption.Description
                            };
                    }
                    else
                    {
                        dialogueOption =
                            new DialogueOption<TParagraphConditionContext, TAftermathContext>(
                                "Common_end_dialogue",
                                DialogueNode<TParagraphConditionContext, TAftermathContext>.EndNode)
                            {
                                Aftermath = aftermaths,
                                HideConditions = hideConditions,
                                SelectConditions = selectConditions,
                                DescriptionSid = dialogueDtoOption.Description
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

        return new Dialogue<TParagraphConditionContext, TAftermathContext>(nodeListDictionaries
            .Single(x => x.nodeSid == "root")
            .node);
    }

    private static IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>> CreateOptionConditions<TParagraphConditionContext>(
        DialogueDtoData[]? conditions,
        IDialogueConditionCreator<TParagraphConditionContext> creator)
    {
        var list = new List<IDialogueParagraphCondition<TParagraphConditionContext>>();

        if (conditions is null)
        {
            return list;
        }

        list.AddRange(conditions.Select(condition => creator.Create(condition.Type, condition.Data)));

        return list;
    }

    private static IDialogueOptionAftermath<TAftermathContext> CreateAftermaths<TAftermathContext>(
        DialogueDtoData[]? aftermathDtos,
        IDialogueOptionAftermathCreator<TAftermathContext> aftermathCreator)
    {
        if (aftermathDtos is null)
        {
            return new NullDialogueOptionAftermath<TAftermathContext>();
        }

        var list = aftermathDtos.Select(aftermathDto => aftermathCreator.Create(aftermathDto.Type, aftermathDto.Data))
            .ToList();

        return new CompositeOptionAftermath<TAftermathContext>(list);
    }

    private static IReadOnlyCollection<IDialogueOptionAftermath<TAftermathContext>> CreateParagraphEffects<
        TAftermathContext>(
        DialogueDtoData[]? paragraphEffects,
        IDialogueParagraphEffectCreator<TAftermathContext> paragraphEffectCreator)
    {
        if (paragraphEffects is null)
        {
            return Array.Empty<IDialogueOptionAftermath<TAftermathContext>>();
        }

        return paragraphEffects.Select(envDto => paragraphEffectCreator.Create(envDto.Type, envDto.Data)).ToList();
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