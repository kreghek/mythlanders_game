﻿using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;

namespace GameClient.Engine.Tests;

internal class ParallaxRectControlTests
{
    [Test]
    public void GetRects_ViewPointMoves_RectsMovesToViewPoint()
    {
        // ARRANGE

        var provider = Mock.Of<IViewPointProvider>(x => x.GetWorldCoords() == new Vector2(6, 5));

        var screenRect = new Rectangle(0, 0, 10, 10);
        var layerRect = new Rectangle(0, 0, 20, 10); // will placed on -10
        var relativeSpeeds = new[] 
        { 
            new Vector2(1, 0)
        };
        var rectControl = new ParallaxRectControl(screenRect, layerRect, relativeSpeeds, provider);
        
        // ACT

        var rects = rectControl.GetRects();

        // ASSERT

        rects[0].X.Should().Be(-9);
    }
}