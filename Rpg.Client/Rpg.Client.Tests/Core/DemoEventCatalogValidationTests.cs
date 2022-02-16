using System.Linq;

using FluentAssertions;

using NUnit.Framework;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    public class DemoEventCatalogValidationTests
    {
        [Test]
        public void AllEventsHaveUniqueSids()
        {
            // ARRANGE

            var unitSchemeCatalog = new DemoUnitSchemeCatalog();

            // ACT

            var catalog = new DemoEventCatalog(unitSchemeCatalog);

            // ASSERT
            var eventsSids = catalog.Events.Select(x => x.Sid).ToArray();
            eventsSids.Should().OnlyHaveUniqueItems();
        }
    }
}