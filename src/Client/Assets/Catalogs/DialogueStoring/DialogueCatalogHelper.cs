using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.DialogueOptionAftermath;
using Client.Core;
using Client.Core.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal static class DialogueCatalogHelper
{
    public static Dialogue Create(string dialogueSid, IDictionary<string, DialogueDtoScene> scenesDtoDict,
        DialogueCatalogCreationServices services)
    {
        var nodeListDicts =
            new List<(string nodeSid, DialogueNode node, List<DialogueOption> optionsList, DialogueDtoOption[]?
                optionsDto)>();

        foreach (var (sceneSid, dtoScene) in scenesDtoDict)
        {
            var paragraphs = new List<DialogueParagraph>();

            for (var paragraphIndex = 0; paragraphIndex < dtoScene.Paragraphs.Length; paragraphIndex++)
            {
                var dialogueDtoParagraph = dtoScene.Paragraphs[paragraphIndex];

                var environmentEffects = CreateEnvironmentEffects(dialogueDtoParagraph.Env, services.EnvEffectCreator);

                if (dialogueDtoParagraph.Text is not null)
                {
                    // Regular paragraph
                    var paragraphContext = new DialogueParagraphConfig
                    {
                        EnvironmentEffects = environmentEffects
                    };

                    var paragraph = new DialogueParagraph(
                        GetSpeaker(dialogueDtoParagraph.Speaker),
                        $"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}",
                        paragraphContext);

                    paragraphs.Add(paragraph);
                }
                else if (dialogueDtoParagraph.Reactions is not null)
                {
                    // Reactions of the heroes

                    foreach (var reaction in dialogueDtoParagraph.Reactions)
                    {
                        var paragraphContext = new DialogueParagraphConfig
                        {
                            EnvironmentEffects = environmentEffects,
                            Conditions = new[] { new HasHeroParagraphCondition(GetSpeaker(reaction.Hero)) }
                        };

                        var paragraph = new DialogueParagraph(
                            GetSpeaker(reaction.Hero),
                            $"{dialogueSid}_Scene_{sceneSid}_Paragraph_{paragraphIndex}_reaction_{reaction.Hero}",
                            paragraphContext);

                        paragraphs.Add(paragraph);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Text or reactions must be assigned.");
                }
            }

            var options = new List<DialogueOption>();
            var dialogNode = new DialogueNode(new DialogueParagraphContainer(paragraphs), options);

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

                    DialogueOption dialogueOption;
                    if (dialogueDtoOption.Next is not null)
                    {
                        var next = nodeListDicts.Single(x => x.nodeSid == dialogueDtoOption.Next).node;
                        dialogueOption = new DialogueOption($"{dialogueSid}_Scene_{nodeSid}_Option_{optionIndex}", next)
                        {
                            Aftermath = aftermaths
                        };
                    }
                    else
                    {
                        dialogueOption = new DialogueOption("Common_end_dialogue", DialogueNode.EndNode)
                        {
                            Aftermath = aftermaths
                        };
                    }

                    optionsList.Add(dialogueOption);
                }
            }
            else
            {
                var dialogueOption = new DialogueOption("Common_end_dialogue", DialogueNode.EndNode);
                optionsList.Add(dialogueOption);
            }
        }

        return new Dialogue(nodeListDicts.Single(x => x.nodeSid == "root").node);
    }

    private static IDialogueOptionAftermath? CreateAftermaths(DialogueDtoData[]? aftermathDtos,
        IDialogueOptionAftermathCreator aftermathCreator)
    {
        if (aftermathDtos is null)
        {
            return null;
        }

        var list = new List<IDialogueOptionAftermath>();

        foreach (var aftermathDto in aftermathDtos)
        {
            var aftermath = aftermathCreator.Create(aftermathDto.Type, aftermathDto.Data);
            list.Add(aftermath);
        }

        return new CompositeOptionAftermath(list);
    }

    private static IReadOnlyCollection<IDialogueEnvironmentEffect> CreateEnvironmentEffects(DialogueDtoData[]? envs,
        IDialogueEnvironmentEffectCreator environmentEffectCreator)
    {
        if (envs is null)
        {
            return Array.Empty<IDialogueEnvironmentEffect>();
        }

        var list = new List<IDialogueEnvironmentEffect>();

        foreach (var envDto in envs)
        {
            var envEffect = environmentEffectCreator.Create(envDto.Type, envDto.Data);
            list.Add(envEffect);
        }

        return list;
    }

    private static UnitName GetSpeaker(string? dtoSpeaker)
    {
        if (!Enum.TryParse<UnitName>(dtoSpeaker, ignoreCase: true, out var unitName))
        {
            unitName = UnitName.Environment;
        }

        return unitName;
    }
}

internal sealed class HasHeroParagraphCondition : IDialogueParagraphCondition
{
    private readonly UnitName _hero;

    public HasHeroParagraphCondition(UnitName hero)
    {
        _hero = hero;
    }

    public bool Check(IDialogueParagraphConditionContext context)
    {
        return context.CurrentHeroes.Contains(_hero.ToString().ToLower());
    }
}