﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal class DialogueOptions : ControlBase
    {
        public DialogueOptions()
        {
            Options = new List<DialogueOptionButton>();
        }

        protected override Point CalcTextureOffset() => ControlTextures.PanelBlack;

        protected override Color CalculateColor() => Color.White;

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            var lastTopButtonPosition = 0;
            foreach (var button in Options)
            {
                var optionButtonSize = CalcOptionButtonSize(button);
                var optionPosition = new Vector2(OPTION_BUTTON_MARGIN + contentRect.Left, lastTopButtonPosition + contentRect.Top).ToPoint();

                button.Rect = new Rectangle(optionPosition, optionButtonSize + new Point(1000, 0));

                button.Draw(spriteBatch);

                lastTopButtonPosition += optionButtonSize.Y;
            }
        }

        public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
        {
            foreach (var button in Options)
            {
                button.Update(resolutionIndependentRenderer);
            }
        }

        public IList<DialogueOptionButton> Options { get; }

        private const int OPTION_BUTTON_MARGIN = 5;

        public int GetHeight()
        {
            var sumOptionHeight = Options.Sum(x => CalcOptionButtonSize(x).Y) + OPTION_BUTTON_MARGIN;

            return sumOptionHeight;
        }

        private static Point CalcOptionButtonSize(DialogueOptionButton button)
        {
            var contentSize = button.GetContentSize();
            return (contentSize + Vector2.One * CONTENT_MARGIN + Vector2.UnitY * OPTION_BUTTON_MARGIN)
                .ToPoint();
        }
    }
}