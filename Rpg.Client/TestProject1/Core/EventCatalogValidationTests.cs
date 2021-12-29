using System;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

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

            var unitSchemeCatalog = new UnitSchemeCatalog();
            
            // ACT

            var catalog = new EventCatalog(unitSchemeCatalog);

            // ASSERT
            var eventsSids = catalog.Events.Select(x => x.Sid).ToArray();
            eventsSids.Should().OnlyHaveUniqueItems();
        }
    }
}