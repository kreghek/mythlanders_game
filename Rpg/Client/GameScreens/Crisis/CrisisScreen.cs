using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Crises;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Rest.Ui;

using Core.Crises;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Crisis;

internal sealed class CrisisScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly ICrisis _crisis;
    private readonly IList<ResourceTextButton> _aftermathButtons;

    public CrisisScreen(TestamentGame game, CrisisScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();

        var crisesCatalog = game.Services.GetRequiredService<ICrisesCatalog>();

        _crisis = crisesCatalog.GetAll().First();

        _aftermathButtons = new List<ResourceTextButton>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
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

        const int ACTION_BUTTON_WIDTH = 200;
        const int ACTION_BUTTON_HEIGHT = 40;

        const int HEADER_HEIGHT = 100;

        var localizedCrisisDescription = GameObjectHelper.GetLocalized(_crisis.Sid);
        var localizedNormalizedCrisisDescription = StringHelper.LineBreaking(localizedCrisisDescription, 60);
        var descriptionText = _uiContentStorage.GetTitlesFont();
        var descriptionTextSize = descriptionText.MeasureString(localizedNormalizedCrisisDescription);

        spriteBatch.DrawString(descriptionText, localizedNormalizedCrisisDescription,
            new Vector2((contentRect.Center.X - descriptionTextSize.X / 2),
                contentRect.Top + ControlBase.CONTENT_MARGIN), Color.Wheat);

        for (var buttonIndex = 0; buttonIndex < _aftermathButtons.Count; buttonIndex++)
        {
            var actionButton = _aftermathButtons[buttonIndex];
            actionButton.Rect = new Rectangle(
                contentRect.Center.X - ACTION_BUTTON_WIDTH / 2,
                HEADER_HEIGHT + buttonIndex * ACTION_BUTTON_HEIGHT + contentRect.Top + (int)descriptionTextSize.X,
                ACTION_BUTTON_WIDTH,
                ACTION_BUTTON_HEIGHT
            );

            actionButton.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var context = new CrisisAftermathContext();
        
        foreach (var aftermath in _crisis.GetItems())
        {
            var aftermathButton = new ResourceTextButton(aftermath.Sid.ResourceName);
            _aftermathButtons.Add(aftermathButton);

            aftermathButton.OnClick += (s, e) =>
            {
                var underConstructionModal = new UnderConstructionModal(
                    _uiContentStorage,
                    ResolutionIndependentRenderer);

                aftermath.Apply(context);
                
                underConstructionModal.Closed += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        new CampaignScreenTransitionArguments(_campaign));
                };

                AddModal(underConstructionModal, false);
                _campaign.CompleteCurrentStage();
            };
        }
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var actionButton in _aftermathButtons)
        {
            actionButton.Update(ResolutionIndependentRenderer);
        }
    }
}