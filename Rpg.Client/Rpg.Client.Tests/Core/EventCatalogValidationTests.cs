using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;

namespace Rpg.Client.Tests.Core
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

            var catalog = new EventCatalog(unitSchemeCatalog);

            // ACT

            catalog.Init();

            // ASSERT
            var eventsSids = catalog.Events.Select(x => x.Sid).ToArray();
            eventsSids.Should().OnlyHaveUniqueItems();
        }
    }
}