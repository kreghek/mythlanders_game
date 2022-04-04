using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Hero.Ui
{
    internal sealed class SkillsInfoPanel : PanelBase
    {
        private const int ICON_SIZE = 32;
        private const int MARGIN = 5;
        private readonly SpriteFont _mainFont;

        private readonly IList<EntityIconButton<ISkill>> _skillList;

        public SkillsInfoPanel(Texture2D texture, SpriteFont titleFont, Unit hero, Texture2D controlTexture,
            Texture2D skillIconsTexture, SpriteFont mainFont) : base(
            texture, titleFont)
        {
            _skillList = new List<EntityIconButton<ISkill>>();
            for (var index = 0; index < hero.Skills.Take(3).Count(); index++)
            {
                var skill = hero.Skills[index];

                var iconRect = UnsortedHelpers.GetIconRect(skill.Sid);
                var iconData = new IconData(skillIconsTexture, iconRect);

                var skillIconButton = new EntityIconButton<ISkill>(controlTexture, iconData, skill);
                _skillList.Add(skillIconButton);

                skillIconButton.OnClick += SkillIconButton_OnClick;
            }

            _mainFont = mainFont;
        }

        protected override string TitleResourceId => nameof(UiResource.HeroSkillsInfoTitle);

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            for (var index = 0; index < _skillList.Count; index++)
            {
                var skillButton = _skillList[index];

                skillButton.Rect = new Rectangle(
                    contentRect.Location + new Point(MARGIN, MARGIN + index * (ICON_SIZE + MARGIN)),
                    new Point(ICON_SIZE, ICON_SIZE));
                skillButton.Draw(spriteBatch);

                var skillNameText = GameObjectHelper.GetLocalized(skillButton.Entity.Sid);
                spriteBatch.DrawString(_mainFont, skillNameText,
                    skillButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);

                if (skillButton.Entity.BaseEnergyCost is not null)
                {
                    var manaCostText = string.Format(UiResource.ManaCostLabelTemplate, skillButton.Entity.BaseEnergyCost);
                    spriteBatch.DrawString(_mainFont, manaCostText,
                        skillButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 20), Color.Cyan);
                }
            }
        }

        private void SkillIconButton_OnClick(object? sender, EventArgs e)
        {
        }
    }
}