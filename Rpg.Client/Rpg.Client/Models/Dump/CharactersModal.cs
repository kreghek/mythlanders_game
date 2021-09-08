using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Dump
{
    internal sealed class CharactersModal : ModalDialogBase
    {
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GlobeProvider _globeProvider;

        private readonly IList<ButtonBase> _buttonList;
        private Core.Unit? _selectedCharacter;

        public CharactersModal(
            IUiContentStorage uiContentStorage,
            GraphicsDevice graphicsDevice,
            GlobeProvider globeProvider) : base(uiContentStorage, graphicsDevice)
        {
            _uiContentStorage = uiContentStorage;
            _globeProvider = globeProvider;

            _buttonList = new List<ButtonBase>();
        }

        protected override void InitContent()
        {
            base.InitContent();

            var globe = _globeProvider.Globe;
            var playerCharacters = globe.Player.Group.Units;

            _buttonList.Clear();
            foreach (var character in playerCharacters)
            {
                var button = new TextButton(character.UnitScheme.Name, _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), new Rectangle());
                button.OnClick += (s, e) =>
                {
                    _selectedCharacter = character;
                };
                _buttonList.Add(button);
            }
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            for (var characterIndex = 0; characterIndex < _buttonList.Count; characterIndex++)
            {
                var button = _buttonList[characterIndex];
                button.Rect = new Rectangle(ContentRect.Left, ContentRect.Top + characterIndex * 21, 100, 20);
                button.Draw(spriteBatch);
            }

            if (_selectedCharacter is not null)
            {
                var sb = new[]
                {
                    _selectedCharacter.UnitScheme.Name,
                    $"Level: {_selectedCharacter.CombatLevel}",
                    $"Exp: {_selectedCharacter.Xp}/{_selectedCharacter.XpToLevelup}"
                };

                for (var statIndex = 0; statIndex < sb.Length; statIndex++)
                {
                    var line = sb[statIndex];
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), line, new Vector2(ContentRect.Center.X, ContentRect.Top + statIndex * 22), Color.White);
                }
            }
        }

        protected override void UpdateContent()
        {
            base.UpdateContent();

            foreach (var button in _buttonList)
            {
                button.Update();
            }
        }
    }
}