using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.DialogueOptionAftermath;
using Client.Core.Dialogues;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal static class DialogueCatalogHelper
{
    public static Dialogue Create(string dialogueSid, IDictionary<string, DialogueDtoScene> scenesDtoDict, DialogueCatalogCreationServices services)
    {
        var nodeListDicts = new List<(string nodeSid, DialogueNode node, List<DialogueOption> optionsList, DialogueDtoOption[]? optionsDto)>();

        foreach (var dtoScene in scenesDtoDict)
        {
            var paragraphs = new List<DialogueParagraph>();

            for (var paragraphIndex = 0; paragraphIndex < dtoScene.Value.Paragraphs.Length; paragraphIndex++)
            {
                var dialogueDtoParagraph = dtoScene.Value.Paragraphs[paragraphIndex];

                var environmentEffects = CreateEnvironmentEffects(dialogueDtoParagraph.Env, services.EnvEffectCreator);

                if (dialogueDtoParagraph.Text is not null)
                {
                    // Regular paragraph
                    var paragraph = new DialogueParagraph(GetSpeaker(dialogueDtoParagraph.Speaker),
                        $"{dialogueSid}_Scene_{dtoScene.Key}_Paragraph_{paragraphIndex}")
                    {
                        EnvironmentEffects = environmentEffects
                    };

                    paragraphs.Add(paragraph);
                }
                else if (dialogueDtoParagraph.Reactions is not null)
                {
                    // Reactions of the heroes

                    foreach (var reaction in dialogueDtoParagraph.Reactions)
                    {
                        // TODO Check hero in the party

                        var paragraph = new DialogueParagraph(GetSpeaker(reaction.Hero),
                            $"{dialogueSid}_Scene_{dtoScene.Key}_Paragraph_{paragraphIndex}_reaction_{reaction.Hero}")
                        {
                            EnvironmentEffects = environmentEffects
                        };

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
            
            nodeListDicts.Add((dtoScene.Key, dialogNode, options, dtoScene.Value.Options));
        }

        // Linking scenes via player's options
        foreach (var nodeListDict in nodeListDicts)
        {
            if (nodeListDict.optionsDto is not null)
            {
                foreach (var dialogueDtoOption in nodeListDict.optionsDto)
                {
                    var aftermaths = CreateAftermaths(dialogueDtoOption.Aftermaths, services.OptionAftermathCreator);
                    
                    DialogueOption dialogueOption;
                    if (dialogueDtoOption.Next is not null)
                    {
                        var next = nodeListDicts.Single(x => x.nodeSid == dialogueDtoOption.Next).node;
                        dialogueOption = new DialogueOption($"{dialogueSid}_Scene_{nodeListDict.nodeSid}", next)
                        {
                            Aftermath = aftermaths
                        };
                    }
                    else
                    {
                        dialogueOption = new DialogueOption($"Common_end_dialogue", DialogueNode.EndNode)
                        {
                            Aftermath = aftermaths
                        };
                    }

                    nodeListDict.optionsList.Add(dialogueOption);
                }
            }
            else
            {
                var dialogueOption = new DialogueOption($"Common_end_dialogue", DialogueNode.EndNode);
                nodeListDict.optionsList.Add(dialogueOption);
            }
        }

        return new Dialogue(nodeListDicts.Single(x => x.nodeSid == "root").node);
    }

    private static IReadOnlyCollection<IDialogueEnvironmentEffect> CreateEnvironmentEffects(DialogueDtoData[]? envs, IDialogueEnvironmentEffectCreator environmentEffectCreator)
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
    
    private static IDialogueOptionAftermath? CreateAftermaths(DialogueDtoData[]? aftermathDtos, IDialogueOptionAftermathCreator aftermathCreator)
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

    private static UnitName GetSpeaker(string? dtoSpeaker)
    {
        if (!Enum.TryParse<UnitName>(dtoSpeaker, ignoreCase: true, out var unitName))
        {
            unitName = UnitName.Environment;
        }

        return unitName;
    }
}