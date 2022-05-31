using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Assets.Catalogs;

namespace Rpg.Client.Tests.Core
{
    [TestFixture]
    public class DemoEventCatalogValidationTests
    {
        [Test]
        public void AllEventsHaveUniqueSids()
        {
            // ARRANGE

            var unitSchemeCatalog = new DemoUnitSchemeCatalog();

            var catalog = new DemoEventCatalog(unitSchemeCatalog);

            // ACT

            catalog.Init();

            // ASSERT
            var eventsSids = catalog.Events.Select(x => x.Sid).ToArray();
            eventsSids.Should().OnlyHaveUniqueItems();
        }
    }
}