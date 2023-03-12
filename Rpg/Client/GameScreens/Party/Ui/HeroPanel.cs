using System;
using System.Linq;

using Client;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Party.Ui
{
    internal sealed class HeroPanel : ControlBase
    {
        private readonly Core.Heroes.Hero _character;

        private readonly Texture2D _disabledTexture;
        private readonly ButtonBase _infoButton;
        private readonly SpriteFont _mainFont;
        private readonly SpriteFont _nameFont;
        private readonly Texture2D _portraitTexture;

        public HeroPanel(Core.Heroes.Hero character, Player player,
            HeroPanelResources characterPanelResources)
        {
            _character = character;
            _portraitTexture = characterPanelResources.PortraitTexture;
            _nameFont = characterPanelResources.NameFont;
            _mainFont = characterPanelResources.MainFont;

            var infoButton = new IndicatorTextButton(nameof(UiResource.InfoButtonTitle),
                characterPanelResources.IndicatorsTexture);
            infoButton.OnClick += (_, _) =>
            {
                SelectCharacter?.Invoke(this, new SelectHeroEventArgs(character));
            };
            infoButton.IndicatingSelector = () =>
            {
                return character.LevelUpXpAmount <=
                       player.Inventory.Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount ||
                       IsAnyEquipmentToUpgrade(character: character, player: player);
            };

            _infoButton = infoButton;

            _disabledTexture = characterPanelResources.DisabledTexture;
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _infoButton.Update(resolutionIndependentRenderer);
        }

        protected override Point CalcTextureOffset()
        {
            return Point.Zero;
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
                contentRect.Location.ToVector2() + new Vector2(32 + 5 * 2, 20 + 10), Color.White);

            var isDisabled = CheckIsDisabled(_character);
            if (isDisabled)
            {
                spriteBatch.Draw(_disabledTexture, contentRect, Color.White);
            }
        }

        private static bool CheckIsDisabled(Core.Heroes.Hero character)
        {
            foreach (var effect in character.GlobalEffects)
            {
                if (UnsortedHelpers.CheckIsDisabled(character.UnitScheme.Name, effect))
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawInfoButton(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int BUTTON_WIDTH = 100;
            const int BUTTON_HEIGHT = 20;
            _infoButton.Rect = new Rectangle(contentRect.Center.X - BUTTON_WIDTH / 2, contentRect.Top + 64 + 10,
                BUTTON_WIDTH, BUTTON_HEIGHT);
            _infoButton.Draw(spriteBatch);
        }

        private void DrawName(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var unitName = _character.UnitScheme.Name;
            var name = GameObjectHelper.GetLocalized(unitName);
            spriteBatch.DrawString(_nameFont, name, contentRect.Location.ToVector2() + new Vector2(32 + 5 * 2, 10),
                Color.White);
        }

        private void DrawPortrait(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var portraitRect = UnsortedHelpers.GetUnitPortraitRect(_character.UnitScheme.Name);
            spriteBatch.Draw(_portraitTexture, new Rectangle(contentRect.Location + new Point(5, 5), new Point(32, 32)),
                portraitRect,
                Color.White);
        }

        private static bool IsAnyEquipmentToUpgrade(Core.Heroes.Hero character, Player player)
        {
            return character.Equipments.Any(equipment =>
                equipment.RequiredResourceAmountToLevelUp <= player.Inventory.Single(resource =>
                    resource.Type == equipment.Scheme.RequiredResourceToLevelUp).Amount);
        }

        public event EventHandler<SelectHeroEventArgs>? SelectCharacter;
    }
}