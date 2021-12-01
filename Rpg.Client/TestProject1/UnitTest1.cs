using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.EventSerialization;

namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void Test1()
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
                unitSchemeCatalog);

            // ASSERT

            fact.Options.First().Next.Should().BeNull();
        }

        [Test]
        public void Test2()
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
                unitSchemeCatalog);

            // ASSERT

            fact.Options.First().Next.Should().NotBeNull();
        }
    }
}