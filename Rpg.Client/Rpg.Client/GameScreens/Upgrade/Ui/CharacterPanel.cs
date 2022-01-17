using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Upgrade.Ui
{
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

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            _levelUpButton.Update(resolutionIndependentRenderer);

            foreach (var buttonBase in _upgradeEquipmentButtonDict)
            {
                buttonBase.Value.Update(resolutionIndependentRenderer);
            }
        }
    }
}