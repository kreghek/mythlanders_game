using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Arrival;

internal class ArrivalScreen : GameScreenWithMenuBase
{
    public ArrivalScreen(EwarGame game) : base(game)
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