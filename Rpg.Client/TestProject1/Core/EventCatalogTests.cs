using System;

using FluentAssertions;

using NUnit.Framework;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    public class EventCatalogTests
    {
        [Test]
        public void Constructor_NoExceptions()
        {
            // ARRANGE

            var unitSchemeCatalog = new UnitSchemeCatalog();
            
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