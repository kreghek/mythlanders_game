using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Core;

using CombatDicesTeam.Dialogues;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Catalogs.DialogueStoring;

[TestFixture]
public class DialogueCatalogHelperTests
{
    /// <summary>
    /// Test checks the simplest dialogue.
    /// </summary>
    [Test]
    public void Create_SingleScene_ReturnsSingleNode()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>
        {
            {
                "root", new DialogueDtoScene
                {
                    Paragraphs = new[]
                    {
                        new DialogueDtoParagraph
                        {
                            Text = "test text"
                        }
                    }
                }
            }
        };

        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices<CampaignAftermathContext>(
                Mock.Of<IDialogueParagraphEffectCreator<CampaignAftermathContext>>(),
                Mock.Of<IDialogueOptionAftermathCreator<CampaignAftermathContext>>()),
            _ => ArraySegment<IDialogueParagraphCondition<ParagraphConditionContext>>.Empty);

        // ASSERT

        dialogue.Root.TextBlock.Paragraphs.Should().HaveCount(1);
        dialogue.Root.Options.Single().Next.Should()
            .Be(DialogueNode<ParagraphConditionContext, CampaignAftermathContext>.EndNode);
    }

    /// <summary>
    /// Test checks the simplest dialogue.
    /// </summary>
    [Test]
    public void Create_SingleScene_CreateExitOptionIfNoOptions()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>
        {
            {
                "root", new DialogueDtoScene
                {
                    Paragraphs = new[]
                    {
                        new DialogueDtoParagraph
                        {
                            Text = "test text"
                        }
                    }
                }
            }
        };

        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices<CampaignAftermathContext>(
                Mock.Of<IDialogueParagraphEffectCreator<CampaignAftermathContext>>(),
                Mock.Of<IDialogueOptionAftermathCreator<CampaignAftermathContext>>()),
            _ => ArraySegment<IDialogueParagraphCondition<ParagraphConditionContext>>.Empty);

        // ASSERT

        dialogue.Root.Options.Single().Next.Should()
            .Be(DialogueNode<ParagraphConditionContext, CampaignAftermathContext>.EndNode);
    }

    /// <summary>
    /// Test checks the simplest dialogue.
    /// </summary>
    [Test]
    public void Create_ParagraphWithReactions_ReturnsParagraphToEachReaction()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>
        {
            {
                "root", new DialogueDtoScene
                {
                    Paragraphs = new[]
                    {
                        new DialogueDtoParagraph
                        {
                            Reactions = new[]
                            {
                                new DialogueDtoReaction
                                {
                                    Hero = "Swordsman",
                                    Text = "test text"
                                }
                            }
                        }
                    }
                }
            }
        };

        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices<CampaignAftermathContext>(
                Mock.Of<IDialogueParagraphEffectCreator<CampaignAftermathContext>>(),
                Mock.Of<IDialogueOptionAftermathCreator<CampaignAftermathContext>>()),
            _ => ArraySegment<IDialogueParagraphCondition<ParagraphConditionContext>>.Empty);

        // ASSERT

        dialogue.Root.TextBlock.Paragraphs.Single().Speaker.Should().Be(DialogueSpeakers.Get(UnitName.Swordsman));
    }

    /// <summary>
    /// Test checks environment was set as speaker if not specified.
    /// </summary>
    [Test]
    public void Create_EnvironmentParagraph_ReturnsEnvironmentAsSpeaker()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>
        {
            {
                "root", new DialogueDtoScene
                {
                    Paragraphs = new[]
                    {
                        new DialogueDtoParagraph
                        {
                            Text = "test environment description"
                        }
                    }
                }
            }
        };

        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices<CampaignAftermathContext>(
                Mock.Of<IDialogueParagraphEffectCreator<CampaignAftermathContext>>(),
                Mock.Of<IDialogueOptionAftermathCreator<CampaignAftermathContext>>()),
            _ => ArraySegment<IDialogueParagraphCondition<ParagraphConditionContext>>.Empty);

        // ASSERT

        dialogue.Root.TextBlock.Paragraphs.Single().Speaker.Should().Be(DialogueSpeakers.Env);
    }
}