using System;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Rpg.Client.Core;

namespace TestProject1
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void TakeDamage_UnitHasTransformation_SchemeChanged()
        {
            // ARRANGE

            var nextScheme = new UnitScheme();

            var sourceScheme = new UnitScheme
            {
                SchemeAutoTransition = new UnitSchemeAutoTransition()
                {
                    HpShare = 0.5f,
                    NextScheme = nextScheme
                }
            };

            var unit = new Unit(sourceScheme, 0);

            var halfOfHitPoints = Math.Round(unit.MaxHitPoints * 0.5f, MidpointRounding.AwayFromZero);
            var damage = (int)halfOfHitPoints;

            var damageDealer = Mock.Of<ICombatUnit>();

            // ACT

            unit.TakeDamage(damageDealer, damage);
            
            // ASSERT

            unit.UnitScheme.Should().BeSameAs(nextScheme);
        }
        
        [Test]
        public void TakeDamage_UnitHasTransformation_EventFired()
        {
            // ARRANGE

            var nextScheme = new UnitScheme();

            var sourceScheme = new UnitScheme
            {
                SchemeAutoTransition = new UnitSchemeAutoTransition()
                {
                    HpShare = 0.5f,
                    NextScheme = nextScheme
                }
            };

            var unit = new Unit(sourceScheme, 0);

            var halfOfHitPoints = Math.Round(unit.MaxHitPoints * 0.5f, MidpointRounding.AwayFromZero);
            var damage = (int)halfOfHitPoints;

            var damageDealer = Mock.Of<ICombatUnit>();

            // ACT

            using var monitor = unit.Monitor();
            unit.TakeDamage(damageDealer, damage);
            
            // ASSERT

            monitor.Should().Raise(nameof(unit.SchemeAutoTransition));
        }
    }
}