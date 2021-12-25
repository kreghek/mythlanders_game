using NUnit.Framework;

using Rpg.Client.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg.Client.Core.Tests
{
    [TestFixture()]
    public class MonsterGeneratorHelperTests
    {
        [Test()]
        public void CreateMonsters_LastNodeInIncompleteBiome_BossReturned()
        {
            var node = new GlobeNode() { IsLast = true };

            var factMonsters = MonsterGeneratorHelper.CreateMonsters(node, )
        }
    }
}