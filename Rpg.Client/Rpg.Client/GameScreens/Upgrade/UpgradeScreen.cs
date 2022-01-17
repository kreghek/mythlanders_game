using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Upgrade.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Upgrade
{
    internal sealed class UpgradeScreen : GameScreenBase
    {
        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GameObjectContentStorage _gameObjectsContentStorage;
        private bool _isInitialized;
        private IList<CharacterPanel> _characterPanels;

        public UpgradeScreen(EwarGame game) : base(game)
        {
            _globeProvider = game.Services.GetService<GlobeProvider>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _gameObjectsContentStorage = game.Services.GetService<GameObjectContentStorage>();

            _characterPanels = new List<CharacterPanel>();
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (!_isInitialized)
            {
                return;
            }

            for (var i = 0; i < _characterPanels.Count; i++)
            {
                var panel = _characterPanels[i];
                panel.Rect = new Rectangle(new Point(0, i * 100), new Point());
            }
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            if (!_isInitialized)
            {
                foreach (var character in _globeProvider.Globe.Player.GetAll())
                {
                    var panel = new CharacterPanel(_uiContentStorage.GetButtonTexture(), character, _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetTitlesFont(), _gameObjectsContentStorage.GetUnitPortrains());
                    _characterPanels.Add(panel);
                }
            }
            else
            {
                foreach (var characterPanel in _characterPanels)
                {
                    characterPanel.Update(ResolutionIndependentRenderer);
                }
            }
        }
    }
}
