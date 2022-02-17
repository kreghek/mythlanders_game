using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.EventSerialization;

namespace Rpg.Client.Tests.Core
{
    public class EventCatalogHelperTests
    {
        [Test]
        public void BuildEventNode_ALotOfTextFragments_MultiplePages()
        {
            var nodeStorageModel = new EventNodeStorageModel
            {
                Fragments = Enumerable.Range(1, 10).Select(x =>
                    new EventTextFragmentStorageModel
                    {
                        Speaker = UnitName.Environment.ToString(),
                        Text = $"test text test text test text test text {x}."
                    }
                ).ToArray()
            };

            var unitSchemeCatalogMock = new Mock<IUnitSchemeCatalog>();
            var unitSchemeCatalog = unitSchemeCatalogMock.Object;

            // ACT

            var fact = EventCatalogHelper.BuildEventNode(nodeStorageModel,
                EventPosition.BeforeCombat,
                aftermath: null,
                unitSchemeCatalog,
                splitIntoPages: true);

            // ASSERT

            fact.Options.First().Next.Should().NotBeNull();
        }

        [Test]
        public void BuildEventNode_DoNotSplitIntoPages_AllTextFragmentsIntoSinglePage()
        {
            const int TEXT_FRAGMENT_COUNT = 10;
            var nodeStorageModel = new EventNodeStorageModel
            {
                Fragments = Enumerable.Range(1, TEXT_FRAGMENT_COUNT).Select(x =>
                    new EventTextFragmentStorageModel
                    {
                        Speaker = UnitName.Environment.ToString(),
                        Text = $"test text test text test text test text {x}."
                    }
                ).ToArray()
            };

            var unitSchemeCatalogMock = new Mock<IUnitSchemeCatalog>();
            var unitSchemeCatalog = unitSchemeCatalogMock.Object;

            // ACT

            var fact = EventCatalogHelper.BuildEventNode(nodeStorageModel,
                EventPosition.BeforeCombat,
                aftermath: null,
                unitSchemeCatalog,
                splitIntoPages: false);

            // ASSERT

            fact.TextBlock.Fragments.Should().HaveCount(TEXT_FRAGMENT_COUNT);
        }

        [Test]
        public void BuildEventNode_DoNotSplitIntoPages_SinglePage()
        {
            var nodeStorageModel = new EventNodeStorageModel
            {
                Fragments = Enumerable.Range(1, 10).Select(x =>
                    new EventTextFragmentStorageModel
                    {
                        Speaker = UnitName.Environment.ToString(),
                        Text = $"test text test text test text test text {x}."
                    }
                ).ToArray()
            };

            var unitSchemeCatalogMock = new Mock<IUnitSchemeCatalog>();
            var unitSchemeCatalog = unitSchemeCatalogMock.Object;

            // ACT

            var fact = EventCatalogHelper.BuildEventNode(nodeStorageModel,
                EventPosition.BeforeCombat,
                aftermath: null,
                unitSchemeCatalog,
                splitIntoPages: false);

            // ASSERT

            fact.Options.First().Next.Should().BeNull();
        }

        [Test]
        public void BuildEventNode_FewTextFragments_SinglePage()
        {
            var nodeStorageModel = new EventNodeStorageModel
            {
                Fragments = Enumerable.Range(1, 5).Select(x =>
                    new EventTextFragmentStorageModel
                    {
                        Speaker = UnitName.Environment.ToString(),
                        Text = $"test text test text test text test text {x}."
                    }
                ).ToArray()
            };

            var unitSchemeCatalogMock = new Mock<IUnitSchemeCatalog>();
            var unitSchemeCatalog = unitSchemeCatalogMock.Object;

            // ACT

            var fact = EventCatalogHelper.BuildEventNode(nodeStorageModel,
                EventPosition.BeforeCombat,
                aftermath: null,
                unitSchemeCatalog,
                splitIntoPages: true);

            // ASSERT

            fact.Options.First().Next.Should().BeNull();
        }
    }
}