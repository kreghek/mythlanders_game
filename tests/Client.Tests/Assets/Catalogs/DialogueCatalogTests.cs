using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Client.Assets.Catalogs;
using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Core;

using CombatDicesTeam.Dialogues;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Assets.Catalogs;

[TestFixture]
public class DialogueCatalogTests
{
    [Test]
    public void GetDialogue_SimplestDialogue_ReturnsDialogueWithSingleTextNodeAndEndOption()
    {
        // ARRANGE

        var resourceProviderMock = new Mock<IDialogueResourceProvider>();
        var sourceDialogue = ReadResource("Simplest");
        resourceProviderMock.Setup(x => x.GetResource(It.IsAny<string>())).Returns(sourceDialogue);
        var resourceProvider = resourceProviderMock.Object;

        var aftermathCreator = Mock.Of<IDialogueOptionAftermathCreator>();

        var catalog = new DialogueCatalog(resourceProvider, aftermathCreator);
        catalog.Init();

        // ACT

        var factDialogue = catalog.GetDialogue(string.Empty);

        // ASSERT

        factDialogue.Root.TextBlock.Paragraphs.Should().HaveCount(1);
        factDialogue.Root.TextBlock.Paragraphs.First().Speaker.Should().Be(DialogueSpeakers.Env);
        factDialogue.Root.Options.Should().HaveCount(1);
        factDialogue.Root.Options.First().Next.Should().Be(DialogueNode<ParagraphConditionContext, AftermathContext>.EndNode);
    }

    [Test]
    public void GetDialogue_TextNodeSequence_Returns2TextFragments()
    {
        // ARRANGE

        var resourceProviderMock = new Mock<IDialogueResourceProvider>();
        var sourceDialogue = ReadResource("TextSequence");
        resourceProviderMock.Setup(x => x.GetResource(It.IsAny<string>())).Returns(sourceDialogue);
        var resourceProvider = resourceProviderMock.Object;

        var aftermathCreator = Mock.Of<IDialogueOptionAftermathCreator>();

        var catalog = new DialogueCatalog(resourceProvider, aftermathCreator);
        catalog.Init();

        // ACT

        var factDialogue = catalog.GetDialogue(string.Empty);

        // ASSERT

        factDialogue.Root.TextBlock.Paragraphs.Should().HaveCount(2);
        factDialogue.Root.TextBlock.Paragraphs[0].Speaker.Should().Be(DialogueSpeakers.Env);
        factDialogue.Root.TextBlock.Paragraphs[1].Speaker.Should().Be(DialogueSpeakers.Get(UnitName.Swordsman));
        factDialogue.Root.Options.Should().HaveCount(1);
        factDialogue.Root.Options.First().Next.Should().Be(DialogueNode<ParagraphConditionContext, AftermathContext>.EndNode);
    }

    private static string ReadResource(string name)
    {
        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"Client.Tests.Assets.Catalogs.DialogueTestResource.{name}.yaml");

        if (stream is not null)
        {
            using var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();

            return text;
        }

        throw new InvalidOperationException();
    }
}