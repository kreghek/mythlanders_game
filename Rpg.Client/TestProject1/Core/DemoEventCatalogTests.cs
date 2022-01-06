using System;

using FluentAssertions;

using NUnit.Framework;

namespace Rpg.Client.Core.Tests
{
    [TestFixture]
    public class DemoEventCatalogTests
    {
        [Test]
        public void Constructor_NoExceptions()
        {
            // ARRANGE

            var unitSchemeCatalog = new DemoUnitSchemeCatalog();
            
            // ACT

            Action act = () =>
            {
                var _ = new DemoEventCatalog(unitSchemeCatalog);
            };

            // ASSERT
            act.Should().NotThrow();
        }
    }
}