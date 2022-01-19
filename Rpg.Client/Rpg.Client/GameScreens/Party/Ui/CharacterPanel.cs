using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal sealed class SelectCharacterEventArgs : EventArgs
    {
        public SelectCharacterEventArgs(Unit character)
        {
            Character = character;
        }

        public Unit Character { get; }
    }

    internal sealed class CharacterPanel : ControlBase
    {
        private readonly Unit _character;
        private readonly ButtonBase _infoButton;
        private readonly SpriteFont _mainFont;
        private readonly SpriteFont _nameFont;
        private readonly Texture2D _portraitTexture;

        public CharacterPanel(Texture2D texture, Unit character, Player player, Texture2D buttonTexture,
            SpriteFont buttonFont, Texture2D indicatorsTexture, Texture2D portraitTexture, SpriteFont nameFont, SpriteFont mainFont) : base(texture)
        {
            _character = character;
            _portraitTexture = portraitTexture;
            _nameFont = nameFont;
            _mainFont = mainFont;

            var infoButton = new IndicatorTextButton(nameof(UiResource.InfoButtonTitle), buttonTexture, buttonFont, indicatorsTexture);
            infoButton.OnClick += (_, _) =>
            {
                SelectCharacter?.Invoke(this, new SelectCharacterEventArgs(character));
            };
            infoButton.IndicatingSelector = () =>
            {
                return character.LevelUpXpAmount <=
                       player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount ||
                       IsAnyEquipmentToUpgrade(character: character, player: player);
            };

            _infoButton = infoButton;
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _infoButton.Update(resolutionIndependentRenderer);
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            DrawPortrait(spriteBatch, contentRect);
            DrawName(spriteBatch, contentRect);
            DrawInfoButton(spriteBatch, contentRect);

            spriteBatch.DrawString(_mainFont, string.Format(UiResource.CombatLevelTemplate, _character.Level),
                contentRect.Location.ToVector2() + new Vector2(32 + 5, 10), Color.Black);
        }

        private void DrawInfoButton(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int BUTTON_WIDTH = 100;
            const int BUTTON_HEIGHT = 20;
            _infoButton.Rect = new Rectangle(contentRect.Center.X - BUTTON_WIDTH / 2, contentRect.Top + 64,
                BUTTON_WIDTH, BUTTON_HEIGHT);
            _infoButton.Draw(spriteBatch);
        }

        private void DrawName(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var unitName = _character.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);
            spriteBatch.DrawString(_nameFont, name, contentRect.Location.ToVector2() + new Vector2(32 + 5, 0),
                Color.Black);
        }

        private void DrawPortrait(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var portraitRect = UnsortedHelpers.GetUnitPortraitRect(_character.UnitScheme.Name);
            spriteBatch.Draw(_portraitTexture, new Rectangle(contentRect.Location, new Point(32, 32)), portraitRect,
                Color.White);
        }

        private static bool IsAnyEquipmentToUpgrade(Unit character, Player player)
        {
            return character.Equipments.Any(equipment =>
                equipment.RequiredResourceAmountToLevelUp <= player.Inventory.Single(resource =>
                    resource.Type == equipment.Scheme.RequiredResourceToLevelUp).Amount);
        }

        public event EventHandler<SelectCharacterEventArgs> SelectCharacter;
    }
}