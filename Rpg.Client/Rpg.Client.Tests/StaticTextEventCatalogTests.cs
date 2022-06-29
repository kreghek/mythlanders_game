using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Tests
{
    [TestFixture]
    public class StaticTextEventCatalogTests
    {
        [Test]
        public void StaticTextEventCatalogTestsDiag()
        {
            // ARRANGE

            var unitCatalog = Mock.Of<IUnitSchemeCatalog>();

            var catalog = new StaticTextEventCatalog(unitCatalog);
            catalog.Init();

            // ACT

            catalog.GetDialogue("SlavicMain1_Before");

            // ASSERT
            
        }
    }
}