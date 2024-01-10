using System;
using System.Collections.Generic;

using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Arrival;

internal class ArrivalScreen : GameScreenWithMenuBase
{
    public ArrivalScreen(MythlandersGame game) : base(game)
    {
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return Array.Empty<ButtonBase>();
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