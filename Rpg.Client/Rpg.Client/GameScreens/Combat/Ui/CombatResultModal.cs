using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class CombatResultModal : ModalDialogBase
    {
        private const int MARGIN = 5;

        private readonly ButtonBase _closeButton;
        private readonly CombatRewards _combatRewards;
        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly CombatResultTitle _title;
        private readonly IUiContentStorage _uiContentStorage;
        private CombatItem? _combatItemsLocal;
        private CombatRewardList? _combatRewardList;

        private double _iterationCounter;

        public CombatResultModal(IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            CombatResult combatResult,
            CombatRewards combatRewards) : base(uiContentStorage, resolutionIndependentRenderer)
        {
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            _combatRewards = combatRewards;
            CombatResult = combatResult;
            _closeButton = new ResourceTextButton(nameof(UiResource.CloseButtonTitle),
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont());
            _closeButton.OnClick += CloseButton_OnClick;

            _title = new CombatResultTitle(_uiContentStorage.GetButtonTexture(), _uiContentStorage.GetTitlesFont(),
                combatResult);
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
                    DrawNextCombatBenefits(spriteBatch, benefitsRect);
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

        protected override void InitContent()
        {
            base.InitContent();

            var biomeProgress = new AnimatedProgressionUnitItemStat(_combatRewards.BiomeProgress);
            var unitRewards = _combatRewards.InventoryRewards.Select(x => new AnimatedRewardItem(x)).ToArray();

            _combatItemsLocal = new CombatItem(biomeProgress, unitRewards);
            _combatRewardList = new CombatRewardList(_uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetTitlesFont(),
                _uiContentStorage.GetMainFont(),
                _gameObjectContentStorage.GetEquipmentIcons(),
                _combatItemsLocal
            );
        }

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            _iterationCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_iterationCounter >= 0.01)
            {
                _combatRewardList?.Update();
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
            var biomeProgress =
                string.Format(UiResource.CombatResultMonsterDangerDecreasedTemplate,
                    _combatItemsLocal.BiomeProgress.CurrentValue);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeProgress,
                new Vector2(benefitsRect.Center.X, _combatRewardList.Rect.Bottom + MARGIN),
                Color.Wheat);
        }

        private void DrawNextCombatBenefits(SpriteBatch spriteBatch, Rectangle benefitsRect)
        {
        }

        private void DrawVictoryBenefits(SpriteBatch spriteBatch, Rectangle benefitsRect)
        {
            if (_combatItemsLocal is null)
            {
                // The modal is not initialized yet.
                return;
            }

            if (_combatRewardList is not null)
            {
                _combatRewardList.Rect =
                    new Rectangle(benefitsRect.Location, new Point(benefitsRect.Width, 2 * (32 + 5)));
                _combatRewardList.Draw(spriteBatch);

                var biomeProgress =
                    string.Format(UiResource.CombatResultMonsterDangerIncreasedTemplate,
                        _combatItemsLocal.BiomeProgress.CurrentValue);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), biomeProgress,
                    new Vector2(benefitsRect.Center.X, _combatRewardList.Rect.Bottom + MARGIN),
                    Color.Wheat);
            }
        }
    }
}