using System;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Campaign.Ui;

internal sealed class CampaignStageItemSelectedEventArgs : EventArgs
{
    public CampaignStageItemSelectedEventArgs(Vector2 position, string _description)
    {
        Position = position;
        Description = _description;
    }

    public string Description { get; }

    public Vector2 Position { get; }
}