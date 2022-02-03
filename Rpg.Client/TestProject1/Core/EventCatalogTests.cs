using System;

using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.Assets;

namespace Rpg.Client.Core.Tests
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