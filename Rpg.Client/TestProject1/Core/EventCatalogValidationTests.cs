using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Assets;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    [Category("VALIDATION")]
    public class EventCatalogValidationTests
    {
        [Test]
        public void AllEventsHaveUniqueSids()
        {
            // ARRANGE

            var balanceTable = new BalanceTable();
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

            // ACT

            var catalog = new EventCatalog(unitSchemeCatalog);

            // ASSERT
            var eventsSids = catalog.Events.Select(x => x.Sid).ToArray();
            eventsSids.Should().OnlyHaveUniqueItems();
        }
    }
}