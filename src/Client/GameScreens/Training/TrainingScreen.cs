using System;
using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Training;

internal sealed class TrainingScreen : GameScreenWithMenuBase
{
    private readonly IReadOnlyList<HeroState> _availableHeroes;
    private readonly HeroCampaign _campaign;
    private IReadOnlyList<(ButtonBase, HeroState)> _trainingButtons = null!;

    public TrainingScreen(MythlandersGame game, TrainingScreenTransitionArguments args) : base(game)
    {
        _availableHeroes = args.AvailableHeroes;

        _campaign = args.Campaign;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new TextButton("Close");
        closeButton.OnClick += CloseButton_OnClick;

        return new ButtonBase[]
        {
            closeButton
        };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        for (var buttonIndex = 0; buttonIndex < _trainingButtons.Count; buttonIndex++)
        {
            var button = _trainingButtons[buttonIndex];

            button.Item1.Rect = new Rectangle(
                contentRect.Left + ControlBase.CONTENT_MARGIN,
                contentRect.Top + ControlBase.CONTENT_MARGIN + (buttonIndex * (25 + ControlBase.CONTENT_MARGIN)),
                100,
                20);

            button.Item1.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var buttonList = new List<(ButtonBase, HeroState)>();

        foreach (var hero in _availableHeroes)
        {
            var trainingButton = new TextButton(hero.ClassSid);
            buttonList.Add((trainingButton, hero));

            // var xpAmount = _player.Inventory.Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount;
            // if (xpAmount < character.LevelUpXpAmount)
            // {
            //     trainingButton.IsEnabled = false;
            // }
            // else
            // {
            //     trainingButton.OnClick += (s, e) =>
            //     {
            //         _player.Inventory.Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount -=
            //             character.LevelUpXpAmount;
            //         character.LevelUp();
            //
            //         MarkButtonAsUsed(trainingButton);
            //         RefreshButtons();
            //     };
            // }
        }

        _trainingButtons = buttonList;
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}