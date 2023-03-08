using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.DialogueStoring;
using Client.Core.Dialogues;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;

namespace Rpg.Client.Tests.Catalogs.DialogueStoring;

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

        var dict = new Dictionary<string, DialogueDtoScene>()
        {
            {"root", new DialogueDtoScene()
            {
                Paragraphs = new[]
                {
                    new DialogueDtoParagraph()
                    {
                        Text = "test text"
                    }
                }
            }}
        };
        
        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices(Mock.Of<IDialogueEnvironmentEffectCreator>(),
                Mock.Of<IDialogueOptionAftermathCreator>()));
        
        // ASSERT

        dialogue.Root.TextBlock.Paragraphs.Should().HaveCount(1);
        dialogue.Root.Options.Single().Next.Should().Be(DialogueNode.EndNode);
    }
    
    /// <summary>
    /// Test checks the simplest dialogue.
    /// </summary>
    [Test]
    public void Create_SingleScene_CreateExitOptionIfNoOptions()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>()
        {
            {"root", new DialogueDtoScene()
            {
                Paragraphs = new[]
                {
                    new DialogueDtoParagraph()
                    {
                        Text = "test text"
                    }
                }
            }}
        };
        
        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices(Mock.Of<IDialogueEnvironmentEffectCreator>(),
                Mock.Of<IDialogueOptionAftermathCreator>()));
        
        // ASSERT

        dialogue.Root.Options.Single().Next.Should().Be(DialogueNode.EndNode);
    }
    
    /// <summary>
    /// Test checks the simplest dialogue.
    /// </summary>
    [Test]
    public void Create_ParagraphWithReactions_ReturnsParagraphToEachReaction()
    {
        // ARRANGE

        var dict = new Dictionary<string, DialogueDtoScene>()
        {
            {"root", new DialogueDtoScene()
            {
                Paragraphs = new[]
                {
                    new DialogueDtoParagraph()
                    {
                        Reactions = new[]
                        {
                            new DialogueDtoReaction()
                            {
                                Hero = "Swordsman",
                                Text = "test text"
                            }
                        }
                    }
                }
            }}
        };
        
        // ACT

        var dialogue = DialogueCatalogHelper.Create("test", dict,
            new DialogueCatalogCreationServices(Mock.Of<IDialogueEnvironmentEffectCreator>(),
                Mock.Of<IDialogueOptionAftermathCreator>()));
        
        // ASSERT

        dialogue.Root.TextBlock.Paragraphs.Single().Speaker.Should().Be(UnitName.Swordsman);
    }
}