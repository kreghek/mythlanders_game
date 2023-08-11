using System;
using System.Collections.Generic;

using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Armory;

internal sealed class ArmoryScreen : GameScreenWithMenuBase
{
    private readonly ArmoryScreenTransitionArguments _args;

    public ArmoryScreen(TestamentGame game, ArmoryScreenTransitionArguments args) : base(game)
    {
        _args = args;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        throw new NotImplementedException();
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        throw new NotImplementedException();
    }

    protected override void InitializeContent()
    {
        throw new NotImplementedException();
    }
}