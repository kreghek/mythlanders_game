using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
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
            }
        }
    }

    internal sealed class CharacterPanel : ControlBase
    {
        private readonly ButtonBase _levelUpButton;
        private readonly IDictionary<Equipment, ButtonBase> _upgradeEquipmentButtonDict;
        private readonly Unit _character;
        private readonly Texture2D _portraitTexture;

        public CharacterPanel(Texture2D texture, Unit character, Texture2D buttonTexture, SpriteFont buttonFont, Texture2D portraitTexture) : base(texture)
        {
            _levelUpButton = new TextButton("Level up", buttonTexture, buttonFont);
            _levelUpButton.OnClick += LevelUpButton_OnClick;
            _character = character;
            _portraitTexture = portraitTexture;
            _upgradeEquipmentButtonDict = new Dictionary<Equipment, ButtonBase>();
            foreach (var equipment in character.Equipments)
            {
                var upgradeEquipmentButton = new TextButton($"Upgrade {equipment.Scheme.Sid}", buttonTexture, buttonFont);
                upgradeEquipmentButton.OnClick += (_, _) =>
                {
                    equipment.LevelUp();
                };

                _upgradeEquipmentButtonDict.Add(equipment, upgradeEquipmentButton);
            }
        }

        private void LevelUpButton_OnClick(object? sender, EventArgs e)
        {
            _character.LevelUp();
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            var portraitRect = UnsortedHelpers.GetUnitPortraitRect(_character.UnitScheme.Name);
            spriteBatch.Draw(_portraitTexture, new Rectangle(contentRect.Location, new Point(32, 32)), portraitRect, Color.White);
        }
    }
}
