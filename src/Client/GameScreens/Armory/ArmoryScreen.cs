using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Equipments;
using Client.Assets.Equipments.Archer;
using Client.Assets.Equipments.Sergeant;
using Client.Assets.Equipments.Swordsman;
using Client.Core;
using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Armory;

internal sealed class ArmoryScreen : GameScreenWithMenuBase
{
    private readonly ArmoryScreenTransitionArguments _args;

    private IEnumerable<Equipment> equipments;
    private readonly GameObjectContentStorage _contentStorage;

    public ArmoryScreen(MythlandersGame game, ArmoryScreenTransitionArguments args) : base(game)
    {
        _contentStorage = game.Services.GetRequiredService<GameObjectContentStorage>();
        
        _args = args;

        equipments =new[]
        {
            new Equipment(new CombatSword()),
            new Equipment(new ArcherPulsarBow()),
            new Equipment(new MultifunctionalClocks())
        };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        foreach (var equipment in equipments.ToArray())
        {
            var iconIndex = (EquipmentSchemeMetadata)equipment.Scheme.Metadata;
            spriteBatch.Draw(_contentStorage.GetEquipmentIcons(), );
        }
    }

    protected override void InitializeContent()
    {
        
    }
}