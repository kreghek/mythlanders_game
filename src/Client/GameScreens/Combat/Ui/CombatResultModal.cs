using System;
using System.Diagnostics;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Combat.Ui.CombatResultModalModels;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatResultModal : ModalDialogBase
{
    private const int MARGIN = 5;

    private readonly ButtonBase _closeButton;
    private readonly CombatRewardList _combatRewardList;

    private readonly CombatResultTitle _title;

    private double _iterationCounter;

    public CombatResultModal(IUiContentStorage uiContentStorage,
        GameObjectContentStorage gameObjectContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer,
        CombatResult combatResult,
        CombatRewards combatRewards) : base(uiContentStorage, resolutionIndependentRenderer)
    {
        CombatResult = combatResult;
        _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle));
        _closeButton.OnClick += CloseButton_OnClick;

        _title = new CombatResultTitle(combatResult);

        //var biomeProgress = new AnimatedCountableUnitItemStat(combatRewards.BiomeProgress);

        //_biomeProgression = new CombatResultsBiomeProgression(biomeProgress);

        var resourceRewards = combatRewards.InventoryRewards.Select(x => new AnimatedCountableUnitItemStat(x))
            .ToArray();

        _combatRewardList = new CombatRewardList(
            gameObjectContentStorage.GetEquipmentIcons(),
            resourceRewards
        );
    }

    internal CombatResult CombatResult { get; }

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        _title.Rect = new Rectangle(ContentRect.Location, new Point(ContentRect.Width, 50));
        _title.Draw(spriteBatch);

        var benefitsPosition = new Vector2(ContentRect.Location.X + MARGIN, _title.Rect.Bottom + MARGIN);
        var benefitsRect = new Rectangle(benefitsPosition.ToPoint(),
            new Point(ContentRect.Width, ContentRect.Height - _title.Rect.Height - MARGIN));

        switch (CombatResult)
        {
            case CombatResult.Victory:
                DrawVictoryBenefits(spriteBatch, benefitsRect);
                break;
            case CombatResult.NextCombat:
                // Draw nothing
                break;
            case CombatResult.Defeat:
                DrawDefeatBenefits(spriteBatch, benefitsRect);
                break;
            default:
                Debug.Fail("Unknown combat result.");
                break;
        }

        _closeButton.Rect = new Rectangle(ContentRect.Center.X - 50, ContentRect.Bottom - 25, 100, 20);
        _closeButton.Draw(spriteBatch);
    }

    protected override void UpdateContent(GameTime gameTime,
        IResolutionIndependentRenderer resolutionIndependenceRenderer)
    {
        base.UpdateContent(gameTime, resolutionIndependenceRenderer);

        _iterationCounter += gameTime.ElapsedGameTime.TotalSeconds;

        if (_iterationCounter >= 0.01)
        {
            _combatRewardList.Update();
            _iterationCounter = 0;
        }

        Debug.Assert(resolutionIndependenceRenderer is not null, "RIR used everywhere.");
        _closeButton.Update(resolutionIndependenceRenderer);
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        Close();
    }

    private void DrawDefeatBenefits(SpriteBatch spriteBatch, Rectangle benefitsRect)
    {
        //_biomeProgression.Rect =
        //    new Rectangle(benefitsRect.Location, new Point(benefitsRect.Width, 32));
        //_biomeProgression.Draw(spriteBatch);
    }

    private void DrawVictoryBenefits(SpriteBatch spriteBatch, Rectangle benefitsRect)
    {
        //_biomeProgression.Rect =
        //    new Rectangle(benefitsRect.Location, new Point(benefitsRect.Width, 32));
        //_biomeProgression.Draw(spriteBatch);

        const int REWARD_ITEM_MARGIN = 5;

        _combatRewardList.Rect =
            new Rectangle(benefitsRect.Location /*+ new Point(0, _biomeProgression.Rect.Height + BLOCK_MARGIN)*/,
                new Point(benefitsRect.Width, 2 * (32 + REWARD_ITEM_MARGIN)));
        _combatRewardList.Draw(spriteBatch);
    }
}