using System;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;

namespace Rpg.Client.Tests.Core
{
    [TestFixture]
    public class EventCatalogTests
    {
        [Test]
        public void Constructor_NoExceptions()
        {
            // ARRANGE

            var balanceTable = new BalanceTable();
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

            // ACT

            Action act = () =>
            {
                var _ = new EventCatalog(unitSchemeCatalog);
            };

            // ASSERT
            act.Should().NotThrow();
        }
    }
}