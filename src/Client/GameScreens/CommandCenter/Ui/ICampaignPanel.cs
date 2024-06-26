﻿using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter.Ui;

internal interface ICampaignPanel
{
    bool Hover { get; }
    ILocationSid? Location { get; }

    void Draw(SpriteBatch spriteBatch);

    void SetRect(Rectangle value);

    void Update(IResolutionIndependentRenderer resolutionIndependentRenderer);
}