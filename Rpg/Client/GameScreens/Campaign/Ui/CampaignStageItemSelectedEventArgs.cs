using System;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignStageItemSelectedEventArgs : EventArgs
{
    public CampaignStageItemSelectedEventArgs(Vector2 position)
    {
        Position = position;
    }

    public Vector2 Position { get; }
}
