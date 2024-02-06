using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Equipments;
using Client.Assets.Equipments.Archer;
using Client.Assets.Equipments.Sergeant;
using Client.Assets.Equipments.Swordsman;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Armory;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Workshop;

internal sealed class WorkshopScreen : GameScreenWithMenuBase
{
    private readonly WorkshopScreenTransitionArguments _args;

    private IEnumerable<Equipment> equipments;
    private readonly GameObjectContentStorage _contentStorage;

    private readonly VerticalStackPanel _availableEquipments;
    private readonly List<IconButton> _equipmentButtons;

    public WorkshopScreen(MythlandersGame game, WorkshopScreenTransitionArguments args) : base(game)
    {
        _contentStorage = game.Services.GetRequiredService<GameObjectContentStorage>();

        var player = game.Services.GetRequiredService<Player>();
        
        _args = args;

        equipments = args.AvailableEquipment; 

        _equipmentButtons = new List<IconButton>();

        var equipmentElements = equipments.Select(equipment =>
        {
            var metadata = (EquipmentSchemeMetadata)equipment.Scheme.Metadata;

            var button = new IconButton(new IconData(_contentStorage.GetEquipmentIcons(), GetEquipmentIconRect(metadata)));
            
            button.OnClick += (_, _) =>
            {
                args.CurrentCampaign.CompleteCurrentStage();
                
                player.AddEquipment(equipment);
                
                ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                    new CampaignScreenTransitionArguments(args.CurrentCampaign));
            };
            
            _equipmentButtons.Add(button);
            
            return new HorizontalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
                ControlTextures.Transparent,
                new ControlBase[]
                {
                    button,
                    new Text(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(), ControlTextures.Transparent,
                        UiThemeManager.UiContentStorage.GetTitlesFont(), _ => Color.White,
                        () => equipment.Scheme.Sid.ToString())
                });
        }).ToArray();

        _availableEquipments = new VerticalStackPanel(UiThemeManager.UiContentStorage.GetControlBackgroundTexture(),
            ControlTextures.Transparent, equipmentElements);
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        _availableEquipments.Rect = contentRect;
        _availableEquipments.Draw(spriteBatch);
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var equipmentButton in _equipmentButtons)
        {
            equipmentButton.Update(ResolutionIndependentRenderer);
        }
    }

    protected override void InitializeContent()
    {
        
    }
    
    private static int? GetEquipmentIconOneBasedIndex(IEquipmentSchemeMetadata metadata)
    {
        if (metadata is EquipmentSchemeMetadata equipmentSchemeMetadata)
        {
            return equipmentSchemeMetadata.IconOneBasedIndex;
        }

        return null;
    }

    private static Rectangle GetEquipmentIconRect(IEquipmentSchemeMetadata? metadata)
    {
        const int ICON_SIZE = 64;
        if (metadata is null)
        {
            return new Rectangle(0, 0, ICON_SIZE, ICON_SIZE);
        }

        var index = GetEquipmentIconOneBasedIndex(metadata);
        if (index is null)
        {
            return new Rectangle(0, 0, ICON_SIZE, ICON_SIZE);
        }

        var zeroBasedIndex = index.Value - 1;
        const int COL_COUNT = 3;
        var col = zeroBasedIndex % COL_COUNT;
        var row = zeroBasedIndex / COL_COUNT;
        return new Rectangle(col * ICON_SIZE, row * ICON_SIZE, ICON_SIZE, ICON_SIZE);
    }
}