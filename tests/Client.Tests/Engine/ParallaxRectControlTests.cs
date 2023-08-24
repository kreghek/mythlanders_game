using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.Engine;

using FluentAssertions;

using Microsoft.Xna.Framework;

using Moq;

using NUnit.Framework;

namespace Client.Tests.Engine
{
    internal class ParallaxRectControlTests
    {
        [Test]
        public void GetRects_ViewPointMoves_RectsMovesToViewPoint()
        {
            // ARRANGE

            var provider = Mock.Of<IViewPointProvider>(x => x.GetWorldCoords() == new Vector2(6, 5));

            var rectControl = new ParallaxRectControl(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 20, 10), new[] { new Vector2(-1, 0) }, 0, provider);
        
            // ACT

            var rects = rectControl.GetRects();

            // ASSERT

            rects[0].X.Should().Be(-9);
        }
    }
}
