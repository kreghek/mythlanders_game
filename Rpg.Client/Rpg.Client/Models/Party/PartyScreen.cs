using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Party
{
    internal sealed class PartyScreen : GameScreenBase
    {
        private readonly IList<ButtonBase> _buttonList;
        private readonly GlobeProvider _globeProvider;
        private readonly IUiContentStorage _uiContentStorage;

        private bool _isInitialized;
        private Unit? _selectedCharacter;

        public PartyScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _globeProvider = game.Services.GetService<GlobeProvider>();

            _buttonList = new List<ButtonBase>();
        }

        protected override void DoDraw(SpriteBatch spriteBatch, float zIndex)
        {
            spriteBatch.Begin();

            var contentRect = Game.GraphicsDevice.Viewport.Bounds;

            for (var characterIndex = 0; characterIndex < _buttonList.Count; characterIndex++)
            {
                var button = _buttonList[characterIndex];
                button.Rect = new Rectangle(contentRect.Left, contentRect.Top + characterIndex * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            if (_selectedCharacter is not null)
            {
                var sb = new List<string>
                {
                    _selectedCharacter.UnitScheme.Name,
                    $"HP: {_selectedCharacter.Hp}/{_selectedCharacter.MaxHp}",
                    $"Mana: {_selectedCharacter.ManaPool}/{_selectedCharacter.ManaPoolSize}",
                    $"Level: {_selectedCharacter.Level}",
                    $"Exp: {_selectedCharacter.Xp}/{_selectedCharacter.LevelupXp}",
                    $"Equipment: {_selectedCharacter.EquipmentLevel}",
                    $"Equipment items: {_selectedCharacter.EquipmentItems}/{_selectedCharacter.EquipmentLevelup}"
                };

                foreach (var skill in _selectedCharacter.Skills)
                {
                    sb.Add($"{skill.Sid}");
                    if (skill.Cost is not null)
                    {
                        sb.Add($"Cost: {skill.Cost}");
                    }

                    // TODO Display skill efficient - damages, durations, etc.
                }

                if (_globeProvider.Globe.Player.Group.Units.Contains(_selectedCharacter))
                {
                    var index = _globeProvider.Globe.Player.Group.Units.ToList().IndexOf(_selectedCharacter);
                    sb.Add($"Is in party. Slot {index + 1}.");
                }

                for (var statIndex = 0; statIndex < sb.Count; statIndex++)
                {
                    var line = sb[statIndex];
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line,
                        new Vector2(contentRect.Center.X, contentRect.Top + statIndex * 22), Color.White);
                }
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_isInitialized)
            {
                var globe = _globeProvider.Globe;
                var playerCharacters = globe.Player.Group.Units.Concat(globe.Player.Pool.Units).ToArray();

                _buttonList.Clear();
                foreach (var character in playerCharacters)
                {
                    var button = new TextButton(character.UnitScheme.Name, _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), new Rectangle());
                    button.OnClick += (s, e) =>
                    {
                        _selectedCharacter = character;
                    };
                    _buttonList.Add(button);
                }

                var biomeButton = new TextButton("Back to the map", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);
                biomeButton.OnClick += (s, e) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Biome);
                };
                _buttonList.Add(biomeButton);

                var switchUnitButton = new TextButton("Switch unit", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);
                switchUnitButton.OnClick += (s, e) =>
                {
                    if (_selectedCharacter is null)
                    {
                        return;
                    }
                    if (_globeProvider.Globe.Player.Group.Units.Contains(_selectedCharacter))
                    {
                        if (_globeProvider.Globe.Player.Group.Units.Count() > 1)
                        {
                            _globeProvider.Globe.Player.MoveToPool(_selectedCharacter);
                        }
                    }
                    else
                    {
                        if (_globeProvider.Globe.Player.Group.Units.Count() < 3)
                        {
                            _globeProvider.Globe.Player.MoveToParty(_selectedCharacter);
                        }
                    }
                };
                _buttonList.Add(switchUnitButton);

                _isInitialized = true;
            }
            else
            {
                foreach (var button in _buttonList)
                {
                    button.Update();
                }
            }
        }
    }
}