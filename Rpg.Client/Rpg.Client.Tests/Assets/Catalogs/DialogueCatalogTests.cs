using System;
using System.IO;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Tests
{
    [TestFixture]
    public class DialogueCatalogTests
    {
        private string ReadResource(string name)
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"Rpg.Client.Tests.Assets.Catalogs.DialogueTestResource.{name}.json");
            
            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();

                return json;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        
        [Test]
        public void GetDialogue_SingleTextNodeAndOption_ReturnsDialogueWithSingleTextNode()
        {
            // ARRANGE

            var resourceProviderMock = new Mock<IDialogueResourceProvider>();
            var sourceDialogue = ReadResource("SingleTextWithSingleOption");
            resourceProviderMock.Setup(x => x.GetResource(It.IsAny<string>())).Returns(sourceDialogue);
            var resourceProvider = resourceProviderMock.Object;

            var aftermathCreator = Mock.Of<IDialogueOptionAftermathCreator>();

            var catalog = new DialogueCatalog(resourceProvider, aftermathCreator);
            catalog.Init();

            // ACT

            var factDialogue = catalog.GetDialogue(string.Empty);

            // ASSERT

            factDialogue.Root.TextBlock.Fragments.Should().HaveCount(1);
            factDialogue.Root.TextBlock.Fragments.First().Speaker.Should().Be(UnitName.Environment);
            factDialogue.Root.Options.Should().HaveCount(1);
            factDialogue.Root.Options.First().Next.Should().Be(EventNode.EndNode);
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

            factDialogue.Root.TextBlock.Fragments.Should().HaveCount(2);
            factDialogue.Root.TextBlock.Fragments[0].Speaker.Should().Be(UnitName.Environment);
            factDialogue.Root.TextBlock.Fragments[1].Speaker.Should().Be(UnitName.Swordsman);
            factDialogue.Root.Options.Should().HaveCount(1);
            factDialogue.Root.Options.First().Next.Should().Be(EventNode.EndNode);
        }
    }
}