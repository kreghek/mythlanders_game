﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Bestiary
{
    internal sealed class BestiaryScreen : GameScreenBase
    {
        private readonly IList<ButtonBase> _buttonList;

        private readonly Camera2D _camera;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;

        private bool _isInitialized;
        private UnitScheme? _selectedMonster;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public BestiaryScreen(EwarGame game)
            : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();

            _camera = game.Services.GetService<Camera2D>();
            _resolutionIndependentRenderer = game.Services.GetService<ResolutionIndependentRenderer>();

            _unitSchemeCatalog = game.Services.GetService<IUnitSchemeCatalog>();

            _buttonList = new List<ButtonBase>();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _resolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());

            var contentRect = _resolutionIndependentRenderer.VirtualBounds;

            for (var characterIndex = 0; characterIndex < _buttonList.Count; characterIndex++)
            {
                var button = _buttonList[characterIndex];
                button.Rect = new Rectangle(contentRect.Left, contentRect.Top + characterIndex * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            if (_selectedMonster is not null)
            {
                var sb = CollectMonsterStats(_selectedMonster);

                for (var statIndex = 0; statIndex < sb.Count; statIndex++)
                {
                    var line = sb[statIndex];
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line,
                        new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
                }
            }

            spriteBatch.End();
        }

        private static IList<string> CollectMonsterStats(UnitScheme monsterScheme)
        {
            var unitName = monsterScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);

            var sb = new List<string>
            {
                name,
                string.Format(UiResource.HitPointsLabelTemplate, monsterScheme.HitPointsBase)
            };
            return sb;
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                var monsterSchemes = _unitSchemeCatalog.AllMonsters;

                _buttonList.Clear();

                foreach (var monsterScheme in monsterSchemes)
                {
                    var unitName = monsterScheme.Name;
                    var name = GameObjectHelper.GetLocalized(unitName);

                    var button = new TextButton(name, _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), new Rectangle());
                    button.OnClick += (_, _) =>
                    {
                        _selectedMonster = monsterScheme;
                    };
                    _buttonList.Add(button);
                }

                var biomeButton = new TextButton("Back to the map", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);
                biomeButton.OnClick += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                };
                _buttonList.Add(biomeButton);

                _isInitialized = true;
            }
            else
            {
                foreach (var button in _buttonList)
                {
                    button.Update(_resolutionIndependentRenderer);
                }
            }
        }
    }
}