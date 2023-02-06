using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Armory;

internal sealed class ArmoryScreenTransitionArguments : IScreenTransitionArguments
{
    public ArmoryScreenTransitionArguments(HeroCampaign currentCampaign, IReadOnlyList<Equipment> availableEquipment)
    {
        CurrentCampaign = currentCampaign;
        AvailableEquipment = availableEquipment;
    }

    public IReadOnlyList<Equipment> AvailableEquipment { get; }

    public HeroCampaign CurrentCampaign { get; }
}

internal sealed class ArmoryScreen : GameScreenWithMenuBase
{
    private readonly ArmoryScreenTransitionArguments _args;

    public ArmoryScreen(EwarGame game, ArmoryScreenTransitionArguments args) : base(game)
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